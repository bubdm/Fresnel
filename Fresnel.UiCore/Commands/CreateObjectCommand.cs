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
    public class CreateObjectCommand : ICommand
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private Core.Commands.CreateObjectCommand _CreateObjectCommand;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public CreateObjectCommand
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            Core.Commands.CreateObjectCommand createObjectCommand,
            AbstractObjectVmBuilder objectVMBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
            )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _CreateObjectCommand = createObjectCommand;
            _ObjectVMBuilder = objectVMBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public CreateCommandResponse Invoke(CreateObjectRequest request)
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
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new CreateCommandResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }
    }
}