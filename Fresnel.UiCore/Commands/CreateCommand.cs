using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Proxies;
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
        private ProxyCache _ProxyCache;
        private CreateObjectCommand _CreateObjectCommand;
        private AbstractObjectVMBuilder _ObjectVMBuilder;
        private IClock _Clock;

        public CreateCommand
            (
            TemplateCache templateCache,
            ProxyCache proxyCache,
            CreateObjectCommand createObjectCommand,
            AbstractObjectVMBuilder objectVMBuilder,
            IClock clock
            )
        {
            _TemplateCache = templateCache;
            _ProxyCache = proxyCache;
            _CreateObjectCommand = createObjectCommand;
            _ObjectVMBuilder = objectVMBuilder;
            _Clock = clock;
        }

        public CreateCommandResult Invoke(string fullyQualifiedName)
        {
            try
            {
                var tClass = _TemplateCache.GetTemplate(fullyQualifiedName);
                if (tClass == null)
                    return null;

                var oObject = _CreateObjectCommand.Invoke(tClass.RealType, null);

                // Make sure we cache the proxy for use later in the session:
                var proxy = _ProxyCache.GetProxy(oObject.RealObject);

                var vm = _ObjectVMBuilder.BuildFor(oObject);

                return new CreateCommandResult()
                {
                    Passed = true,
                    NewObject = vm
                };
            }
            catch (Exception ex)
            {
                var errorVM = new ErrorVM(ex) { OccurredAt = _Clock.Now };

                return new CreateCommandResult()
                {
                    Failed = true,
                    ErrorMessages = new ErrorVM[] { errorVM }
                };
            }
        }


    }
}
