using Envivo.Fresnel.Engine.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Engine.Commands
{
    public class CloneObjectCommand
    {
        private Introspection.Commands.CloneObjectCommand _CloneObjectCommand;
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;

        public CloneObjectCommand
        (
            Introspection.Commands.CloneObjectCommand cloneObjectCommand,
            TemplateCache templateCache,
            ObserverCache observerCache
        )
        {
            _CloneObjectCommand = cloneObjectCommand;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
        }

        public BaseObjectObserver Invoke(BaseObjectObserver oObject)
        {
            var clone = _CloneObjectCommand.Invoke(oObject, true);

            var tClass = oObject.TemplateAs<ClassTemplate>();
            if (tClass.IdProperty != null)
            {
                tClass.IdProperty.SetProperty(clone, GuidFactory.NewSequentialGuid());
            }

            var oClone = _ObserverCache.GetObserver(clone);

            // Make sure the clone can be edited before the user saves it:
            //oClone.ChangeTracker.IsNewInstance = true;

            return oClone;
        }

    }
}
