using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class SearchParameterCommand : ICommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private SearchResultsVmBuilder _SearchResultsVmBuilder;
        private SearchCommand _SearchCommand;
        private SearchResultsFilterApplier _SearchResultsFilterApplier;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public SearchParameterCommand
            (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache,
            ObserverCache observerCache,
            SearchResultsVmBuilder searchResultsVmBuilder,
            SearchCommand searchCommand,
            SearchResultsFilterApplier searchResultsFilterApplier,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _SearchResultsVmBuilder = searchResultsVmBuilder;
            _SearchCommand = searchCommand;
            _SearchResultsFilterApplier = searchResultsFilterApplier;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public SearchResponse Invoke(SearchParameterRequest request)
        {
            try
            {
                var oObj = (ObjectObserver)_ObserverCache.GetObserverById(request.ObjectID);
                var oMethod = oObj.Methods[request.MethodName];
                var oParam = oMethod.Parameters[request.ParameterName];
                var tParam = oParam.Template;

                var searchType = tParam.IsCollection ?
                                 ((CollectionTemplate)tParam.InnerClass).ElementType :
                                 tParam.ParameterType;

                var tClass = (ClassTemplate)_TemplateCache.GetTemplate(searchType);

                var searchResults = this.FetchObjects(request, oParam, tClass);

                var result = _SearchResultsVmBuilder.BuildFor(searchResults, tClass, request);

                // Done:
                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("Returned ", result.Items.Count(), " ", tClass.FriendlyName, " instances (", result.AreMoreAvailable ? "more are" : "no more", " available)")
                };

                return new SearchResponse()
                {
                    Passed = true,
                    Result = result,
                    Messages = new MessageVM[] { infoVM }
                };
            }
            catch (Exception ex)
            {
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new SearchResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }

        private IQueryable FetchObjects(SearchRequest request, ParameterObserver oParam, ClassTemplate tElement)
        {
            var searchResults = _SearchCommand.Search(oParam);
            var filteredResults = _SearchResultsFilterApplier.ApplyFilter(request, searchResults, tElement);
            return filteredResults;
        }

    }
}