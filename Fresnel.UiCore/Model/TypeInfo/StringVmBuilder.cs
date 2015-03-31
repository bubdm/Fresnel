using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class StringVmBuilder : ISettableVmBuilder
    {
        private DataTypeToUiControlMapper _DataTypeToUiControlMapper;

        public StringVmBuilder
            (
            DataTypeToUiControlMapper dataTypeToUiControlMapper
            )
        {
            _DataTypeToUiControlMapper = dataTypeToUiControlMapper;
        }

        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return actualType == typeof(char) ||
                   actualType == typeof(string);
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var tClass = tProp.InnerClass;

            targetVM.Info = this.CreateInfoVM(tProp.Attributes, actualType);
        }

        public void Populate(ParameterVM targetVM, ParameterTemplate tParam, Type actualType)
        {
            var tClass = tParam.InnerClass;

            targetVM.Info = this.CreateInfoVM(tParam.Attributes, actualType);
        }

        private ITypeInfo CreateInfoVM(AttributesMap attributesMap, Type actualType)
        {
            var minLength = attributesMap.Get<MinLengthAttribute>();
            var maxLength = attributesMap.Get<MaxLengthAttribute>();
            var displayFormat = attributesMap.Get<DisplayFormatAttribute>();
            var dataType = attributesMap.Get<DataTypeAttribute>();
            var preferredControl = attributesMap.Get<UiControlHintAttribute>().PreferredUiControl;
            if (preferredControl == UiControlType.None)
            {
                preferredControl = _DataTypeToUiControlMapper.Convert(dataType.DataType);
            }
            if (preferredControl == UiControlType.None)
            {
                preferredControl = UiControlType.Text;
            }

            return new StringVM()
            {
                Name = "string",
                MinLength = minLength.Length,
                MaxLength = actualType == typeof(char) ? 1 : maxLength.Length,
                EditMask = displayFormat.DataFormatString,
                PreferredControl = preferredControl
            };
        }
    }
}