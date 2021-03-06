using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class SearchPropertyCommand : ICommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;
        private ObserverRetriever _ObserverRetriever;
        private SearchResultsVmBuilder _SearchResultsVmBuilder;
        private SearchCommand _SearchCommand;
        private SearchResultsFilterApplier _SearchResultsFilterApplier;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public SearchPropertyCommand
            (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache,
            ObserverRetriever observerRetriever,
            SearchResultsVmBuilder searchResultsVmBuilder,
            SearchCommand searchCommand,
            SearchResultsFilterApplier searchResultsFilterApplier,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
            _ObserverRetriever = observerRetriever;
            _SearchResultsVmBuilder = searchResultsVmBuilder;
            _SearchCommand = searchCommand;
            _SearchResultsFilterApplier = searchResultsFilterApplier;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public SearchResponse Invoke(SearchPropertyRequest request)
        {
            try
            {
                var oObj = (ObjectObserver)_ObserverRetriever.GetObserverById(request.ObjectID);
                var oProp = oObj.Properties[request.PropertyName];
                var tProp = oProp.Template;

                var searchType = tProp.IsCollection ?
                                 ((CollectionTemplate)tProp.InnerClass).ElementType :
                                 tProp.PropertyType;

                var tClass = (ClassTemplate)_TemplateCache.GetTemplate(searchType);

                var searchResults = this.FetchObjects(request, oProp, tClass);

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

        private IQueryable FetchObjects(SearchRequest request, BasePropertyObserver oProp, ClassTemplate tElement)
        {
            var searchResults = _SearchCommand.Search(oProp);
            var filteredResults = _SearchResultsFilterApplier.ApplyFilter(request, searchResults, tElement);
            return filteredResults;
        }

    }
}