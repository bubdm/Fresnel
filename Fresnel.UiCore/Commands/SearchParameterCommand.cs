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
    public class SearchParameterCommand : ICommand
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private SearchObjectsCommand _SearchObjectsCommand;
        private IClock _Clock;

        public SearchParameterCommand
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            SearchObjectsCommand searchObjectsCommand,
            IClock clock
        )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _SearchObjectsCommand = searchObjectsCommand;
            _Clock = clock;
        }

        public SearchResponse Invoke(SearchParameterRequest request)
        {
            try
            {
                var oObj = _ObserverCache.GetObserverById(request.ObjectID);
                var tClass = (ClassTemplate)oObj.Template;
                var tMethod = tClass.Methods[request.MethodName];
                var tParam = tMethod.Parameters[request.ParameterName];

                var searchType = tParam.IsCollection ?
                                 ((CollectionTemplate)tParam.InnerClass).ElementType :
                                 tParam.ParameterType;

                var subRequest = new SearchObjectsRequest()
                {
                    SearchType = searchType.FullName,
                    OrderBy = request.OrderBy,
                    SearchFilters = request.SearchFilters,
                    IsDescendingOrder = request.IsDescendingOrder,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                };

                var result = _SearchObjectsCommand.Invoke(subRequest);
                return result;
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

    }
}