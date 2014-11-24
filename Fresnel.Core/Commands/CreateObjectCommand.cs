using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core.Commands
{
    public class CreateObjectCommand
    {
        private Introspection.Commands.CreateObjectCommand _CreateObjectCommand;
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;

        public CreateObjectCommand
        (
            Introspection.Commands.CreateObjectCommand createObjectCommand,
            TemplateCache templateCache,
            ObserverCache observerCache
        )
        {
            _CreateObjectCommand = createObjectCommand;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
        }

        public BaseObjectObserver Invoke(Type classType, IEnumerable<object> args)
        {
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(classType);

            var newInstance = _CreateObjectCommand.Invoke(tClass, args);

            var oObject = _ObserverCache.GetObserver(newInstance);

            return oObject;
        }

    }
}
