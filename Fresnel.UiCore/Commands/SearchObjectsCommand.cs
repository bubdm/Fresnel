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
            IQueryable results = null;

            if (request.OrderBy.IsEmpty())
            {
                request.OrderBy = tClass.Properties.Values.First(p => p.IsNonReference).Name;
                request.IsDescendingOrder = true;
            }

            var tProp = tClass.Properties[request.OrderBy];
            if (tProp.IsCollection)
                throw new UiCoreException("Sorting by Collection properties is not allowed");

            if (request.PageNumber < 1)
                throw new UiCoreException("The Page number must be at least 1");

            if (request.PageSize < 1)
                throw new UiCoreException("The Page Size must be at least 1");

            var maxLimit = request.PageSize + 1;
            var rowsToSkip = request.PageSize * (request.PageNumber - 1);
            var classType = tClass.RealType;

            var orderBy = request.IsDescendingOrder ?
                            string.Concat(request.OrderBy, " DESC") :
                            string.Concat(request.OrderBy, " ASC ");

            var whereClause = this.BuildWhereClauseString(request, tClass);
            var whereParams = this.BuildWhereClauseParameters(request, tClass);
            results = _PersistenceService
                            .GetObjects(classType)
                            .OrderBy(orderBy)
                            .Where(whereClause, whereParams)
                            .Skip(rowsToSkip)
                            .Take(maxLimit);

            var matches = results.ToList<object>();
            if (matches.Count < maxLimit)
            {
                // We may have rows with NULL values, so include those too:
                var nullMatches = _PersistenceService
                                    .GetObjects(classType)
                                    .Where(string.Concat(request.OrderBy, " == null"))
                                    .OrderBy(orderBy)
                                    .Take(maxLimit - matches.Count)
                                    .ToList<object>();

                matches.AddRange(nullMatches);
                results = matches.AsQueryable();
            }

            return results;
        }

        private string BuildWhereClauseString(SearchRequest request, ClassTemplate tClass)
        {
            var parts = new List<string>();

            if (request.SearchFilters != null)
            {
                var parameterPosition = 0;
                foreach (var filter in request.SearchFilters)
                {
                    // Make sure we recognise the PropertyName:
                    var tProp = tClass.Properties[filter.PropertyName];

                    var where = this.BuildWhereClauseForProperty(tProp, ref parameterPosition);
                    parts.Add(where);
                }
            }

            if (request.OrderBy != null)
            {
                parts.Add(string.Concat(request.OrderBy, " != null"));
            }

            return string.Join(" AND ", parts);
        }

        private string BuildWhereClauseForProperty(PropertyTemplate tProp, ref int parameterPosition)
        {
            switch (Type.GetTypeCode(tProp.PropertyType))
            {
                case TypeCode.String:
                case TypeCode.Char:
                    {
                        var result = string.Concat("(", tProp.Name, " != null AND ",
                                                        tProp.Name, @".Contains(@", parameterPosition++, ")",
                                                   ")");
                        return result;
                    }

                case TypeCode.DateTime:
                    {
                        var result = string.Concat("(", tProp.Name, " >= @", parameterPosition++, " AND ", 
                                                        tProp.Name, " < @", parameterPosition++, 
                                                   ")");
                        return result;
                    }

                default:
                    {
                        var result = string.Concat(tProp.Name, " == @", parameterPosition++);
                        return result;
                    }
            }
        }

        private object[] BuildWhereClauseParameters(SearchRequest request, ClassTemplate tClass)
        {
            var results = new List<object>();

            if (request.SearchFilters != null)
            {
                foreach (var filter in request.SearchFilters)
                {
                    // Make sure we recognise the PropertyName:
                    var tProp = tClass.Properties[filter.PropertyName];

                    var propertyParams = this.BuildParametersForProperty(tProp, filter.FilterValue);
                    results.AddRange(propertyParams);
                }
            }

            return results.ToArray();
        }

        private IEnumerable<object> BuildParametersForProperty(PropertyTemplate tProp, object filterValue)
        {
            if (tProp.PropertyType.IsEnum &&
                filterValue != null)
            {
                var typedValue = Enum.Parse(tProp.PropertyType, filterValue.ToStringOrNull(), true);
                filterValue = typedValue;
            }

            switch (Type.GetTypeCode(tProp.PropertyType))
            {
                case TypeCode.DateTime:
                    var dateValue = Convert.ToDateTime(filterValue);
                    var startOfDay = dateValue.Date;
                    var endOfDay = startOfDay.AddDays(1);
                    return new List<object>() { startOfDay, endOfDay };

                default:
                    var value = Convert.ChangeType(filterValue, tProp.PropertyType);
                    return new List<object>() { value };
            }
        }

    }
}