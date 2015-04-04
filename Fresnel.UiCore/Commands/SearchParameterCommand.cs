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
        private IClock _Clock;

        public SearchParameterCommand
            (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache,
            ObserverCache observerCache,
            SearchResultsVmBuilder searchResultsVmBuilder,
            SearchCommand searchCommand,
            SearchResultsFilterApplier searchResultsFilterApplier,
            IClock clock
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _SearchResultsVmBuilder = searchResultsVmBuilder;
            _SearchCommand = searchCommand;
            _SearchResultsFilterApplier = searchResultsFilterApplier;
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

                var objects = this.FetchObjects(request, oParam, tClass);
                var areMoreItemsAvailable = objects.Count() > request.PageSize;

                // Only return back the number of items actually requested:
                var results = objects.ToList<object>().Take(request.PageSize);
                var oColl = (CollectionObserver)_ObserverCache.GetObserver(results, results.GetType());
                var result = _SearchResultsVmBuilder.BuildForCollection(oColl, tClass);
                result.AreMoreAvailable = areMoreItemsAvailable;

                // Done:
                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("Returned ", results.Count(), " ", tClass.FriendlyName, " instances (", areMoreItemsAvailable ? "more are" : "no more", " available)")
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
                var errorVM = new MessageVM()
                {
                    IsError = true,
                    OccurredAt = _Clock.Now,
                    Text = ex.Message,
                    Detail = ex.ToString(),
                };

                return new SearchResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }

        private IQueryable FetchObjects(SearchRequest request, ParameterObserver oParam, ClassTemplate tElement)
        {
            var results = _SearchCommand.Search(oParam);
            var filteredResults = _SearchResultsFilterApplier.ApplyFilter(request, results, tElement);
            return filteredResults;
        }


    }
}