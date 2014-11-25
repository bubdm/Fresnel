using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Assemblies
{

    public class AssemblyReaderBuilder
    {
        private ConfigurationMapBuilder _ConfigurationMapBuilder;
        private Func<AssemblyReader> _AssemblyReaderFactory;
        private AbstractClassTemplateBuilder _AbstractClassTemplateBuilder;

        public AssemblyReaderBuilder
        (
            IsFrameworkAssemblySpecification isFrameworkAssemblySpecification,
            ConfigurationMapBuilder configurationMapBuilder,
            Func<AssemblyReader> assemblyReaderFactory,
            AbstractClassTemplateBuilder abstractClassTemplateBuilder
        )
        {
            _ConfigurationMapBuilder = configurationMapBuilder;
            _AssemblyReaderFactory = assemblyReaderFactory;
            _AbstractClassTemplateBuilder = abstractClassTemplateBuilder;
        }

        public AssemblyReader BuildFor(Assembly domainAssembly, bool isSystemAssembly)
        {
            if (domainAssembly == null)
                return null;

            var reader = _AssemblyReaderFactory();
            reader.Assembly = domainAssembly;
            reader.ConfigurationMap = _ConfigurationMapBuilder.BuildFor(domainAssembly);
            reader.XmlDocReader = new AssemblyDocsReader();
            reader.IsFrameworkAssembly = isSystemAssembly;

            this.Initialise(reader);

            reader.XmlDocReader.InitialiseFrom(reader);

            return reader;
        }

        private void Initialise(AssemblyReader reader)
        {
            if (reader.IsFrameworkAssembly)
            {
                //reader.AreInfrastructureServicesEnabled = false;
            }
            else
            {
                // We need to ensure that all top-level Domain Classes are recognised:
                reader.PreLoadClassTemplates();

                //this.AreInfrastructureServicesEnabled = areInfrastructureServicesEnabled;
            }

            //_ClassStructureXml = new ClassStructureBuilder(_Assembly).GetClassStructureXml();
        }

    }


}
