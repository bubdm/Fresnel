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
    public class SearchResultsFilterApplier
    {
        private ClassHierarchyBuilder _ClassHierarchyBuilder;

        public SearchResultsFilterApplier
            (
            ClassHierarchyBuilder classHierarchyBuilder
            )
        {
            _ClassHierarchyBuilder = classHierarchyBuilder;
        }

        public IQueryable ApplyFilter(SearchRequest request, IQueryable searchResults, ClassTemplate tElement)
        {
            if (request.PageNumber < 1)
                throw new UiCoreException("The Page number must be at least 1");

            if (request.PageSize < 1)
                throw new UiCoreException("The Page Size must be at least 1");

            if (request.OrderBy.IsEmpty())
            {
                request.OrderBy = tElement.Properties.Values.First(p => p.IsNonReference).Name;
                request.IsDescendingOrder = true;
            }

            this.CheckOrderByIsValid(request, tElement);

            var maxLimit = request.PageSize + 1;
            var rowsToSkip = request.PageSize * (request.PageNumber - 1);
            var classType = tElement.RealType;

            var orderBy = request.IsDescendingOrder ?
                            string.Concat(request.OrderBy, " DESC") :
                            string.Concat(request.OrderBy, " ASC ");

            var whereClause = this.BuildWhereClauseString(request, tElement);
            var whereParams = this.BuildWhereClauseParameters(request, tElement);
            IQueryable results = searchResults
                                    .OrderBy(orderBy)
                                    .Where(whereClause, whereParams)
                                    .Skip(rowsToSkip)
                                    .Take(maxLimit);

            var matches = results.ToList<object>();
            if (matches.Count < maxLimit)
            {
                // We may have rows with NULL values, so include those too:
                var nullMatches = searchResults
                                    .Where(string.Concat(request.OrderBy, " == null"))
                                    .OrderBy(orderBy)
                                    .Take(maxLimit - matches.Count)
                                    .ToList<object>();

                matches.AddRange(nullMatches);
                results = matches.AsQueryable();
            }

            return results;
        }

        private void CheckOrderByIsValid(SearchRequest request, ClassTemplate tClass)
        {
            var allKnownProperties = _ClassHierarchyBuilder
                                        .GetProperties(tClass)
                                        .ToDictionary(i => i.Name, StringComparer.OrdinalIgnoreCase);
            var tProp = allKnownProperties[request.OrderBy];

            if (tProp.OuterClass != tClass)
            {
                throw new UiCoreException("Unable to sort by properties on subclasses, as this hasn't been implemented yet");
            }

            if (!tProp.IsNonReference)
                throw new UiCoreException("Unable to sort by Objects or Collections, as this hasn't been implemented yet");
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
            if (tProp.PropertyType.IsEnum && ((EnumTemplate)tProp.InnerClass).IsBitwiseEnum)
            {
                // See http://stackoverflow.com/a/10193218/80369 for explanation:
                var result = string.Concat("(", tProp.Name, " != null AND ((",
                                                tProp.Name, " % @", parameterPosition++, " >= @", parameterPosition++, "))",
                                           ")");
                return result;
            }

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
            if (tProp.PropertyType.IsEnum)
            {
                if (((EnumTemplate)tProp.InnerClass).IsBitwiseEnum)
                {
                    // See http://stackoverflow.com/a/10193218/80369 for explanation:
                    var flagValue = Convert.ToInt32(filterValue);
                    var modulus = (int)Math.Pow(2, flagValue);
                    return new List<object>() { modulus, flagValue };
                }
                else
                {
                    var typedValue = Enum.Parse(tProp.PropertyType, filterValue.ToStringOrNull(), true);
                    filterValue = typedValue;
                }
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