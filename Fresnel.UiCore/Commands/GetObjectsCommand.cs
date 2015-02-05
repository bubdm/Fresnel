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

                IEnumerable objects = null;
                if (request.OrderBy.IsNotEmpty())
                {
                    objects = _PersistenceService
                                    .GetObjects(classType)
                                    .OrderBy(request.OrderBy)
                                    .Skip(request.Skip)
                                    .Take(request.Take);
                }
                else
                {
                    objects = _PersistenceService
                                    .GetObjects(classType)
                                    .Take(request.Take);
                }

                var results = new List<object>(objects.Cast<object>());
                var oColl = (CollectionObserver)_ObserverCache.GetObserver(results, results.GetType());

                // Done:
                return new GetObjectsResponse()
                {
                    Passed = true,
                    Matches = (CollectionVM)_ObjectVMBuilder.BuildForCollection(oColl, tClass)
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