using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class EnumVmBuilder : ISettableVmBuilder
    {
        private IDomainDependencyResolver _DomainDependencyResolver;

        public EnumVmBuilder
            (
            IDomainDependencyResolver domainDependencyResolver
            )
        {
            _DomainDependencyResolver = domainDependencyResolver;
        }

        public bool CanHandle(ISettableMemberTemplate template, Type actualType)
        {
            return actualType.IsEnum;
        }

        public void Populate(PropertyVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var tEnum = (EnumTemplate)tProp.InnerClass;
            targetVM.Info = this.CreateInfoVM(tProp.Attributes, tEnum);
        }

        public void Populate(ParameterVM targetVM, ParameterTemplate tParam, Type actualType)
        {
            var tEnum = (EnumTemplate)tParam.InnerClass;
            targetVM.Info = this.CreateInfoVM(tParam.Attributes, tEnum);
        }

        private ITypeInfo CreateInfoVM(AttributesMap attributesMap, EnumTemplate tEnum)
        {
            var preferredControl = attributesMap.Get<UiControlHintAttribute>().PreferredUiControl;

            if (tEnum.IsBitwiseEnum)
            {
                preferredControl = UiControlType.Checkbox;
            }
            else if (preferredControl == UiControlType.None)
            {
                preferredControl = UiControlType.Select;
            }

            return new EnumVM()
            {
                Name = "enum",
                IsBitwiseEnum = tEnum.IsBitwiseEnum,
                Items = this.CreateEnumItems(tEnum),
                PreferredControl = preferredControl,
            };
        }

        private IEnumerable<EnumItemVM> CreateEnumItems(EnumTemplate tEnum)
        {
            // TODO: Set each item's IsChecked property based on the Property's value

            var results = tEnum.EnumItems.Values.Select(e => new EnumItemVM()
            {
                Name = e.FriendlyName,
                EnumName = e.Name,
                Description = e.XmlComments.Summary,
                Value = e.NumericValue,
            })
            .ToArray();

            return results;
        }
    }
}