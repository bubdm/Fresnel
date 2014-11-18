using System;
using System.Collections.Generic;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Assemblies;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class AbstractClassTemplateBuilder
    {
        private AttributesMapBuilder _AttributesMapBuilder;
        private EnumTemplateBulider _EnumTemplateBulider;
        private NonReferenceTemplateBuilder _NonReferenceTemplateBuilder;
        private CollectionTemplateBuilder _CollectionTemplateBuilder;
        private ClassTemplateBuilder _ClassTemplateBuilder;

        public AbstractClassTemplateBuilder
        (
            AttributesMapBuilder attributesMapBuilder,
            EnumTemplateBulider enumTemplateBulider,
            NonReferenceTemplateBuilder nonReferenceTemplateBuilder,
            CollectionTemplateBuilder collectionTemplateBuilder,
            ClassTemplateBuilder classTemplateBuilder
        )
        {
            _AttributesMapBuilder = attributesMapBuilder;
            _EnumTemplateBulider = enumTemplateBulider;
            _NonReferenceTemplateBuilder = nonReferenceTemplateBuilder;
            _CollectionTemplateBuilder = collectionTemplateBuilder;
            _ClassTemplateBuilder = classTemplateBuilder;
        }

        public IClassTemplate CreateTemplate(Type objectType)
        {
            var result = this.CreateTemplate(objectType, null);
            return result;
        }

        public IClassTemplate CreateTemplate(Type objectType, IClassConfiguration classConfiguration)
        {
            IClassTemplate result = null;

            var attributes = _AttributesMapBuilder.BuildFor(objectType, classConfiguration);

            if (objectType.IsEnum)
            {
                var enumTemplate = _EnumTemplateBulider.BuildFor(objectType, attributes);
                enumTemplate.Configuration = classConfiguration;
                result = enumTemplate;
            }
            else if (objectType.IsNonReference())
            {
                var nonReferenceTemplate = _NonReferenceTemplateBuilder.BuildFor(objectType, attributes);
                nonReferenceTemplate.Configuration = classConfiguration;
                result = nonReferenceTemplate;
            }
            else if (objectType.IsCollection())
            {
                // It's a POCO collection class:
                var collectionTemplate = _CollectionTemplateBuilder.BuildFor(objectType, attributes);
                collectionTemplate.Configuration = classConfiguration;
                result = collectionTemplate;
            }
            else
            {
                // It's a POCO class:
                var classTemplate = _ClassTemplateBuilder.BuildFor(objectType, attributes);
                classTemplate.Configuration = classConfiguration;
                result = classTemplate;
            }

            return result;
        }


    }
}
