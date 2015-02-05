using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetObjectsCommand : ICommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        //private IPersistenceService _PersistenceService;
        private IClock _Clock;

        public GetObjectsCommand
            (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache,
            ObserverCache observerCache,
            AbstractObjectVmBuilder objectVMBuilder,
            //IPersistenceService persistenceService,
            IClock clock
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _ObjectVMBuilder = objectVMBuilder;
            //_PersistenceService = persistenceService;
            _Clock = clock;
        }

        public GetObjectsResponse Invoke(GetObjectsRequest request)
        {
            try
            {
                var classType = _TemplateCache.GetTemplate(request.TypeName).RealType;

                //var objects = _PersistenceService
                //                .GetObjects(classType)
                //                .AsQueryable();

                //var results = new List<ObjectVM>();

                //foreach (var obj in objects)
                //{
                //    var realType = _RealTypeResolver.GetRealType(obj);
                //    var oObject = _ObserverCache.GetObserver(obj, realType);
                //    var objectVM = _ObjectVMBuilder.BuildFor(oObject);
                //    results.Add(objectVM);
                //}

                // Done:
                return new GetObjectsResponse()
                {
                    Passed = true,
                    //Results = results
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