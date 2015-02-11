using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
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
    public class SearchObjectsCommand : ICommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private SearchResultsVmBuilder _SearchResultsVmBuilder;
        private IPersistenceService _PersistenceService;
        private IClock _Clock;

        public SearchObjectsCommand
            (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache,
            ObserverCache observerCache,
            SearchResultsVmBuilder searchResultsVmBuilder,
            IPersistenceService persistenceService,
            IClock clock
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _SearchResultsVmBuilder = searchResultsVmBuilder;
            _PersistenceService = persistenceService;
            _Clock = clock;
        }

        public SearchObjectsResponse Invoke(SearchObjectsRequest request)
        {
            try
            {
                var tClass = (ClassTemplate)_TemplateCache.GetTemplate(request.SearchType);
                var classType = tClass.RealType;

                var maxLimit = request.PageSize + 1;

                IEnumerable objects = null;
                if (request.OrderBy != null && request.OrderBy.Any())
                {
                    var orderBy = string.Join(",", request.OrderBy);
                    objects = _PersistenceService
                                    .GetObjects(classType)
                                    .OrderBy(orderBy)
                                    .Skip(request.PageSize * request.PageNumber)
                                    .Take(maxLimit);
                }
                else
                {
                    objects = _PersistenceService
                                    .GetObjects(classType)
                                    .Take(maxLimit);
                }

                var areMoreItemsAvailable = objects.Count() > request.PageSize;

                // Only return back the number of items actually requested:
                var results = new List<object>(objects.Cast<object>().Take(request.PageSize));
                var oColl = (CollectionObserver)_ObserverCache.GetObserver(results, results.GetType());

                // Done:
                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("Returned ", results.Count, " ", tClass.FriendlyName, " instances (", areMoreItemsAvailable ? "more are" : "no more", " available)")
                };
                return new SearchObjectsResponse()
                {
                    Passed = true,
                    Results = _SearchResultsVmBuilder.BuildForCollection(oColl, tClass),
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

                return new SearchObjectsResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }
    }
}