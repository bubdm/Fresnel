using System;


namespace Envivo.Fresnel.Introspection.Templates
{

    /// <summary>
    /// A Template that represents an Enumeration
    /// </summary>

    public class EnumTemplate : NonReferenceTemplate
    {
        private EnumItemTemplateMapBuilder _EnumItemTemplateMapBuilder;

        private Lazy<EnumItemTemplateMap> _EnumItemTemplateMap;

        public EnumTemplate
        (
            EnumItemTemplateMapBuilder enumItemTemplateMapBuilder
        )
        {
            _EnumItemTemplateMapBuilder = enumItemTemplateMapBuilder;

            _EnumItemTemplateMap = new Lazy<EnumItemTemplateMap>(
                                () => _EnumItemTemplateMapBuilder.BuildFor(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _XmlComments = new Lazy<XmlComments>(
                                () => this.AssemblyReader.XmlDocReader.GetXmlCommentsFor(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public bool IsBitwiseEnum { get; internal set; }

        public EnumItemTemplateMap EnumItems
        {
            get { return _EnumItemTemplateMap.Value; }
        }

    }
}
