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
        private ObserverCache _ObserverCache;
        private SearchResultsVmBuilder _SearchResultsVmBuilder;
        private SearchCommand _SearchCommand;
        private SearchResultsFilterApplier _SearchResultsFilterApplier;
        private IClock _Clock;

        public SearchPropertyCommand
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

        public SearchResponse Invoke(SearchPropertyRequest request)
        {
            try
            {
                var oObj = (ObjectObserver)_ObserverCache.GetObserverById(request.ObjectID);
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

        private IQueryable FetchObjects(SearchRequest request, BasePropertyObserver oProp, ClassTemplate tElement)
        {
            var searchResults = _SearchCommand.Search(oProp);
            var filteredResults = _SearchResultsFilterApplier.ApplyFilter(request, searchResults, tElement);
            return filteredResults;
        }

    }
}