using System;
using System.Linq;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;

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

            result.RealObjectType = _RealTypeResolver.GetRealType(enumType);
            result.Name = result.RealObjectType.Name;
            result.FriendlyName = result.RealObjectType.Name.CreateFriendlyName();
            result.FullName = result.RealObjectType.FullName;
            result.Attributes = enumAttributes;

            result.IsBitwiseEnum = result.RealObjectType.GetCustomAttributes(typeof(FlagsAttribute), true).Any();

            // We don't want Generics to spoil the name format, hence the manual formatting:
            result.FullName = enumType.DeclaringType != null ?
                                string.Concat(enumType.Namespace, ".", enumType.DeclaringType.Name, ".", enumType.Name) :
                                string.Concat(enumType.Namespace, ".", enumType.Name);

            result.FinaliseConstruction();

            return result;
        }

    }
}
