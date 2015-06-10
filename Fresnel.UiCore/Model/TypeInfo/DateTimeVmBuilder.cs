using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class DateTimeVmBuilder : ISettableVmBuilder
    {
        private DataTypeToUiControlMapper _DataTypeToUiControlMapper;
        private readonly DateTime _epoch = new DateTime(1970, 1, 1);

        public DateTimeVmBuilder
            (
            DataTypeToUiControlMapper dataTypeToUiControlMapper
            )
        {
            _DataTypeToUiControlMapper = dataTypeToUiControlMapper;
        }

        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return actualType == typeof(DateTime) ||
                   actualType == typeof(DateTimeOffset);
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            targetVM.Info = this.CreateInfoVM(tProp.Attributes);
        }

        public void Populate(ParameterVM targetVM, ParameterTemplate tParam, Type actualType)
        {
            targetVM.Info = this.CreateInfoVM(tParam.Attributes);
        }

        private ITypeInfo CreateInfoVM(AttributesMap attributesMap)
        {
            var displayFormat = attributesMap.Get<DisplayFormatAttribute>();
            var dataType = attributesMap.Get<DataTypeAttribute>();
            var preferredControl = attributesMap.Get<UiControlHintAttribute>().PreferredUiControl;
            if (preferredControl == UiControlType.None)
            {
                preferredControl = _DataTypeToUiControlMapper.Convert(dataType.DataType);
            }
            if (preferredControl == UiControlType.None)
            {
                preferredControl = UiControlType.DateTimeLocal;
            }

            return new DateTimeVM()
            {
                Name = "datetime",
                CustomFormat = displayFormat.DataFormatString,
                PreferredControl = preferredControl
            };
        }
    }
}