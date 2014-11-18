using System;
using System.Linq;
using Envivo.Fresnel.Introspection.Configuration;


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
        }

        internal override void FinaliseConstruction()
        {
            base.FinaliseConstruction();

            _EnumItemTemplateMap = new Lazy<EnumItemTemplateMap>(
                                () => _EnumItemTemplateMapBuilder.BuildFor(this),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public bool IsBitwiseEnum { get; internal set; }

        public EnumItemTemplateMap EnumItems
        {
            get { return _EnumItemTemplateMap.Value; }
        }

    }
}
