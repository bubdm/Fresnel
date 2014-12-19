using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Objects;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class UnknownVmBuilder : IPropertyVmBuilder
    {
        public bool CanHandle(BasePropertyObserver oProp, Type actualType)
        {
            return false;
        }
        
        public void Populate(PropertyVM targetVM, BasePropertyObserver oProp, Type actualType)
        {
            targetVM.CanWrite = false;
        }

        public string GetFormattedValue(BasePropertyObserver oProp, object realPropertyValue)
        {
            return realPropertyValue.ToStringOrNull();
        }

    }
}
