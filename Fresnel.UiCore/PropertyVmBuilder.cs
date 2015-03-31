using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.TypeInfo;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class PropertyVmBuilder
    {
        private IEnumerable<ISettableVmBuilder> _Builders;
        private EmptyPropertyVmBuilder _EmptyPropertyVmBuilder;
        private PropertyStateVmBuilder _PropertyStateVmBuilder;

        public PropertyVmBuilder
            (
            IEnumerable<ISettableVmBuilder> builders,
            EmptyPropertyVmBuilder emptyPropertyVmBuilder,
            PropertyStateVmBuilder propertyStateVmBuilder
            )
        {
            _Builders = builders;
            _EmptyPropertyVmBuilder = emptyPropertyVmBuilder;
            _PropertyStateVmBuilder = propertyStateVmBuilder;
        }

        public PropertyVM BuildFor(BasePropertyObserver oProp)
        {
            var tProp = oProp.Template;
            var objectProp = oProp as ObjectPropertyObserver;

            var propVM = _EmptyPropertyVmBuilder.BuildFor(tProp);
            propVM.ObjectID = oProp.OuterObject.ID;
            propVM.IsLoaded = objectProp != null ? objectProp.IsLazyLoaded : true;

            propVM.State = _PropertyStateVmBuilder.BuildFor(oProp);

            return propVM;
        }

    }
}