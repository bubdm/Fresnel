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

        public SearchResponse Invoke(SearchObjectsRequest request)
        {
            try
            {
                var tClass = (ClassTemplate)_TemplateCache.GetTemplate(request.SearchType);
                var classType = tClass.RealType;

                if (!_PersistenceService.IsTypeRecognised(classType))
                    throw new UiCoreException(string.Concat(_PersistenceService.GetType().Name, " does not recognise ", tClass.FriendlyName));

                var objects = this.FetchObjects(request, tClass);
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

        private IQueryable FetchObjects(SearchObjectsRequest request, ClassTemplate tClass)
        {
            IQueryable results;

            var maxLimit = request.PageSize + 1;
            var rowsToSkip = request.PageSize * request.PageNumber;
            var classType = tClass.RealType;

            if (request.OrderBy.IsEmpty())
            {
                request.OrderBy = tClass.Properties.First().Value.Name;
                request.IsDescendingOrder = true;
            }

            if (request.IsDescendingOrder)
            {
                var orderBy = string.Concat(request.OrderBy, " DESC");
                var where = string.Concat(request.OrderBy, " != null");
                results = _PersistenceService
                                .GetObjects(classType)
                                .OrderBy(orderBy)
                                .Where(where)
                                .Skip(rowsToSkip)
                                .Take(maxLimit);

                var matches = results.ToList<object>();
                if (matches.Count < maxLimit)
                {
                    // We may have rows will NULL values, so include those too:
                    var nullMatches = _PersistenceService
                                        .GetObjects(classType)
                                        .Where(string.Concat(request.OrderBy, " == null"))
                                        .OrderBy(orderBy)
                                        .Take(maxLimit - matches.Count)
                                        .ToList<object>();

                    matches.AddRange(nullMatches);
                    results = matches.AsQueryable();
                }
            }
            else
            {
                var orderBy = string.Concat(request.OrderBy, " ASC");
                var where = string.Concat(request.OrderBy, " != null");
                results = _PersistenceService
                                .GetObjects(classType)
                                .Where(where)
                                .OrderBy(orderBy)
                                .Skip(rowsToSkip)
                                .Take(maxLimit);

                var matches = results.ToList<object>();
                if (matches.Count < maxLimit)
                {
                    // We may have rows will NULL values, so include those too:
                    var nullMatches = _PersistenceService
                                        .GetObjects(classType)
                                        .Where(string.Concat(request.OrderBy, " == null"))
                                        .OrderBy(orderBy)
                                        .Take(maxLimit - matches.Count)
                                        .ToList<object>();

                    matches.AddRange(nullMatches);
                    results = matches.AsQueryable();
                }
            }

            return results;
        }
    }
}