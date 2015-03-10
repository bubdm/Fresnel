using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class EnumTemplateMapBuilder
    {
        private EnumTemplateBulider _EnumTemplateBulider;
        private AttributesMapBuilder _AttributesMapBuilder;

        public EnumTemplateMapBuilder
        (
            EnumTemplateBulider enumTemplateBulider,
            AttributesMapBuilder attributesMapBuilder
        )
        {
            _EnumTemplateBulider = enumTemplateBulider;
            _AttributesMapBuilder = attributesMapBuilder;
        }

        public EnumTemplateMap BuildFrom(ClassTemplate tClass, IClassConfiguration classConfiguration)
        {
            var nestedTypes = tClass.RealType.GetNestedTypes(BindingFlags.Public | BindingFlags.DeclaredOnly);

            var results = new Dictionary<Type, EnumTemplate>();

            foreach (var nestedType in nestedTypes)
            {
                if (nestedType.IsEnum)
                {
                    var enumAttributes = _AttributesMapBuilder.BuildFor(nestedType, tClass.Configuration);
                    var tEnum = _EnumTemplateBulider.BuildFor(nestedType, enumAttributes);
                    results.Add(nestedType, tEnum);
                }
            }

            return new EnumTemplateMap(results);
        }
    }
}