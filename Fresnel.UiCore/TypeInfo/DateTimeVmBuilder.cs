using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class DateTimeVmBuilder : IPropertyVmBuilder
    {
        private readonly DateTime _epoch = new DateTime(1970, 1, 1);

        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return actualType == typeof(DateTime) ||
                   actualType == typeof(DateTimeOffset);
        }

        public void Populate(ValueVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var attr = tProp.Attributes.Get<DateTimeAttribute>();
        }

        public void Populate(ValueVM targetVM, ParameterTemplate tParam, Type actualType)
        {
            var attr = tParam.Attributes.Get<DateTimeAttribute>();

            targetVM.Info = this.CreateInfoVM(attr);
        }

        private ITypeInfo CreateInfoVM(DateTimeAttribute attr)
        {
            return new DateTimeVM()
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