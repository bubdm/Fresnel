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
    public class SearchFilterPropertyVmBuilder
    {
        private IEnumerable<IPropertyVmBuilder> _Builders;
        private EmptyPropertyVmBuilder _EmptyPropertyVmBuilder;
        private PropertyStateVmBuilder _PropertyStateVmBuilder;

        public SearchFilterPropertyVmBuilder
            (
            IEnumerable<IPropertyVmBuilder> builders,
            EmptyPropertyVmBuilder emptyPropertyVmBuilder,
            PropertyStateVmBuilder propertyStateVmBuilder
            )
        {
            _Builders = builders;
            _EmptyPropertyVmBuilder = emptyPropertyVmBuilder;
            _PropertyStateVmBuilder = propertyStateVmBuilder;
        }

        public SettableMemberVM BuildFor(PropertyTemplate tProp)
        {
            var propVM = _EmptyPropertyVmBuilder.BuildFor(tProp);
            propVM.State = _PropertyStateVmBuilder.BuildFor(tProp, null);

            // Allow the user to edit the values, regardless of permissions:
            propVM.State.Get = new InteractionPoint()
            {
                IsEnabled = true,
            };
            propVM.State.Set = new InteractionPoint()
            {
                IsEnabled = true,
            };

            return propVM;
        }

    }
}