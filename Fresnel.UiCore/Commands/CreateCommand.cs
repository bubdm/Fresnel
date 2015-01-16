using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.Classes;
using Envivo.Fresnel.UiCore.Messages;
using Envivo.Fresnel.UiCore.Objects;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CreateCommand
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private CreateObjectCommand _CreateObjectCommand;
        private AbstractObjectVMBuilder _ObjectVMBuilder;
        private IClock _Clock;

        public CreateCommand
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            CreateObjectCommand createObjectCommand,
            AbstractObjectVMBuilder objectVMBuilder,
            IClock clock
            )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _CreateObjectCommand = createObjectCommand;
            _ObjectVMBuilder = objectVMBuilder;
            _Clock = clock;
        }

        public CreateCommandResponse Invoke(string fullyQualifiedName)
        {
            try
            {
                var tClass = _TemplateCache.GetTemplate(fullyQualifiedName);
                if (tClass == null)
                    return null;

                var oObject = _CreateObjectCommand.Invoke(tClass.RealType, null);

                var vm = _ObjectVMBuilder.BuildFor(oObject);

                return new CreateCommandResponse()
                {
                    Passed = true,
                    NewObject = vm
                };
            }
            catch (Exception ex)
            {
                var errorVM = new ErrorVM(ex) { OccurredAt = _Clock.Now };

                return new CreateCommandResponse()
                {
                    Failed = true,
                    Messages = new MessageSetVM(null, null, new ErrorVM[] { errorVM })
                };
            }
        }


    }
}
