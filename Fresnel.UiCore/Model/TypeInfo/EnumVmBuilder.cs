using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class EnumVmBuilder : IPropertyVmBuilder
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

        public void Populate(SettableMemberVM targetVM, PropertyTemplate tProp, Type actualType)
        {
            var tEnum = (EnumTemplate)tProp.InnerClass;
            var attr = tProp.Configurations.Get<EnumAttribute>();

            targetVM.Info = this.CreateInfoVM(tEnum, attr);
        }

        public void Populate(SettableMemberVM targetVM, ParameterTemplate tParam, Type actualType)
        {
            var tEnum = (EnumTemplate)tParam.InnerClass;
            var attr = tParam.Configurations.Get<EnumAttribute>();

            targetVM.Info = this.CreateInfoVM(tEnum, attr);
        }

        private ITypeInfo CreateInfoVM(EnumTemplate tEnum, EnumAttribute attr)
        {
            return new EnumVM()
            {
                Name = "enum",
                IsBitwiseEnum = tEnum.IsBitwiseEnum,
                Items = this.CreateEnumItems(tEnum),
                PreferredControl = tEnum.IsBitwiseEnum ? UiControlType.Checkbox :
                                   attr.PreferredInputControl != UiControlType.None ?
                                   attr.PreferredInputControl :
                                   UiControlType.Select
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