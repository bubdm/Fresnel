using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class DateTimeVmBuilder : IPropertyVmBuilder
    {
        private readonly DateTime _epoch = new DateTime(1970, 1, 1);

        public bool CanHandle(PropertyTemplate tProp, Type actualType)
        {
            return actualType == typeof(DateTime) ||
                   actualType == typeof(DateTimeOffset);
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var attr = tProp.Attributes.Get<DateTimeAttribute>();

            targetVM.Info = new DateTimeVM()
            {
                Name = "datetime",
                CustomFormat = attr.CustomFormat,
                PreferredControl = attr.PreferredInputControl != InputControlTypes.None ?
                                   attr.PreferredInputControl :
                                   InputControlTypes.DateTimeLocal
            };
        }
    }
}