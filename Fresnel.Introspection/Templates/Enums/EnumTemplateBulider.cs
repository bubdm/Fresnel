using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class EnumTemplateBulider
    {
        private RealTypeResolver _RealTypeResolver;
        private Func<EnumTemplate> _EnumTemplateFactory;

        public EnumTemplateBulider
        (
            RealTypeResolver realTypeResolver,
            Func<EnumTemplate> enumTemplateFactory
        )
        {
            _RealTypeResolver = realTypeResolver;
            _EnumTemplateFactory = enumTemplateFactory;
        }


        public EnumTemplate BuildFor(Type enumType, AttributesMap enumAttributes)
        {
            var result = _EnumTemplateFactory();

            //result.RealType = _RealTypeResolver.GetRealType(enumType);
            result.RealType = enumType;
            result.Name = result.RealType.Name;
            result.FriendlyName = result.RealType.Name.CreateFriendlyName();
            result.FullName = result.RealType.FullName;
            result.Attributes = enumAttributes;

            result.IsBitwiseEnum = result.RealType.GetCustomAttributes(typeof(FlagsAttribute), true).Any();

            // We don't want Generics to spoil the name format, hence the manual formatting:
            result.FullName = enumType.DeclaringType != null ?
                                string.Concat(enumType.Namespace, ".", enumType.DeclaringType.Name, ".", enumType.Name) :
                                string.Concat(enumType.Namespace, ".", enumType.Name);

            result.FinaliseConstruction();

            return result;
        }

    }
}
