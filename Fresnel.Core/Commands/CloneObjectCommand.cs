using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core.Commands
{
    public class CloneObjectCommand
    {
        private Introspection.Commands.CloneObjectCommand _CloneObjectCommand;
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private DirtyObjectNotifier _DirtyObjectNotifier;

        public CloneObjectCommand
        (
            Introspection.Commands.CloneObjectCommand cloneObjectCommand,
            TemplateCache templateCache,
            ObserverCache observerCache,
            DirtyObjectNotifier dirtyObjectNotifier
        )
        {
            _CloneObjectCommand = cloneObjectCommand;
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _DirtyObjectNotifier = dirtyObjectNotifier;
        }

        public ObjectObserver Invoke(ObjectObserver oObject)
        {
            var clone = _CloneObjectCommand.Invoke(oObject, true);

            var tClass = oObject.Template;
            if (tClass.IdProperty != null)
            {
                tClass.IdProperty.SetProperty(clone, GuidFactory.NewSequentialGuid());
            }

            var oClone = (ObjectObserver)_ObserverCache.GetObserver(clone);

            // Make sure the clone can be edited before the user saves it:
            _DirtyObjectNotifier.ObjectWasCreated(oClone);

            return oClone;
        }

    }
}
