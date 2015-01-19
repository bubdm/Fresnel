using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class EnumItemTemplateMapBuilder
    {
        private EnumItemTemplateBuilder _EnumItemTemplateBuilder;

        public EnumItemTemplateMapBuilder(EnumItemTemplateBuilder enumItemTemplateBuilder)
        {
            _EnumItemTemplateBuilder = enumItemTemplateBuilder;
        }

        public EnumItemTemplateMap BuildFor(EnumTemplate tEnum)
        {
            var enumFields = tEnum.RealType.GetFields(BindingFlags.Public | BindingFlags.Static);

            var results = new Dictionary<string, EnumItemTemplate>();
            foreach (var enumField in enumFields)
            {
                var enumItem = _EnumItemTemplateBuilder.BuildFor(tEnum, enumField);
                results.Add(enumField.Name, enumItem);
            }

            return new EnumItemTemplateMap(results);
        }
    }
}