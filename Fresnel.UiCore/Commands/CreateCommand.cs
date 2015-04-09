using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CreateCommand : ICommand
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private CreateObjectCommand _CreateObjectCommand;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private IClock _Clock;

        public CreateCommand
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            CreateObjectCommand createObjectCommand,
            AbstractObjectVmBuilder objectVMBuilder,
            IClock clock
            )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _CreateObjectCommand = createObjectCommand;
            _ObjectVMBuilder = objectVMBuilder;
            _Clock = clock;
        }

        public CreateCommandResponse Invoke(CreateRequest request)
        {
            try
            {
                var tClass = _TemplateCache.GetTemplate(request.ClassTypeName);
                if (tClass == null)
                    return null;

                ObjectObserver oParentObject = null;
                if (request.ParentObjectID != Guid.Empty)
                {
                    oParentObject = (ObjectObserver)_ObserverCache.GetObserverById(request.ParentObjectID);
                    if (oParentObject == null)
                        throw new UiCoreException("Cannot find object for " + request.ParentObjectID);
                }

                var classType = tClass.RealType;
                var oObject = _CreateObjectCommand.Invoke(classType, oParentObject);
                var vm = _ObjectVMBuilder.BuildFor(oObject);

                return new CreateCommandResponse()
                {
                    Passed = true,
                    NewObject = vm
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

                return new CreateCommandResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }
    }
}