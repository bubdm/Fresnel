using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class AbstractObjectVmBuilder
    {
        private ObjectVmBuilder _ObjectVmBuilder;
        private CollectionVmBuilder _CollectionVmBuilder;

        public AbstractObjectVmBuilder
            (
            ObjectVmBuilder objectVmBuilder,
            CollectionVmBuilder collectionVmBuilder
            )
        {
            _ObjectVmBuilder = objectVmBuilder;
            _CollectionVmBuilder = collectionVmBuilder;
        }

        public ObjectVM BuildFor(BaseObjectObserver observer)
        {
            var oCollection = observer as CollectionObserver;
            var oObject = observer as ObjectObserver;

            if (oCollection != null)
            {
                return _CollectionVmBuilder.BuildFor(oCollection);
            }
            else if (oObject != null)
            {
                return _ObjectVmBuilder.BuildFor(oObject);
            }

            return null;
        }

        public ObjectVM BuildFor(ObjectPropertyObserver oProperty, BaseObjectObserver observer)
        {
            var oCollection = observer as CollectionObserver;
            var oObject = observer as ObjectObserver;

            if (oCollection != null)
            {
                return _CollectionVmBuilder.BuildFor(oProperty, oCollection);
            }
            else if (oObject != null)
            {
                return _ObjectVmBuilder.BuildFor(oObject);
            }

            return null;
        }
    }
}