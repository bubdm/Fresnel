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
    public class GetObjectsCommand : ICommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private IPersistenceService _PersistenceService;
        private IClock _Clock;

        public GetObjectsCommand
            (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache,
            ObserverCache observerCache,
            AbstractObjectVmBuilder objectVMBuilder,
            IPersistenceService persistenceService,
            IClock clock
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _ObjectVMBuilder = objectVMBuilder;
            _PersistenceService = persistenceService;
            _Clock = clock;
        }

        public GetObjectsResponse Invoke(GetObjectsRequest request)
        {
            try
            {
                var tClass = (ClassTemplate)_TemplateCache.GetTemplate(request.TypeName);
                var classType = tClass.RealType;

                var maxLimit = request.PageSize + 1;

                IEnumerable objects = null;
                if (request.OrderBy.IsNotEmpty())
                {
                    objects = _PersistenceService
                                    .GetObjects(classType)
                                    .OrderBy(request.OrderBy)
                                    .Skip(request.PageSize * request.PageNumber)
                                    .Take(maxLimit);
                }
                else
                {
                    objects = _PersistenceService
                                    .GetObjects(classType)
                                    .Take(maxLimit);
                }

                var areMoreItemsAvailable = objects.Count() > request.PageNumber;

                // Only return back the number of items actually requested:
                var results = new List<object>(objects.Cast<object>().Take(request.PageNumber));
                var oColl = (CollectionObserver)_ObserverCache.GetObserver(results, results.GetType());

                // Done:
                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("Returned ", results.Count, " ", tClass.FriendlyName, " instances (", areMoreItemsAvailable ? "more are" : "no more", " available)")
                };
                return new GetObjectsResponse()
                {
                    Passed = true,
                    Result = (CollectionVM)_ObjectVMBuilder.BuildForCollection(oColl, tClass),
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

                return new GetObjectsResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }
    }
}