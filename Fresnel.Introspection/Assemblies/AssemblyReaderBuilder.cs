using Envivo.Fresnel.Introspection.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Assemblies
{

    public class AssemblyReaderBuilder
    {
        private IsFrameworkAssemblySpecification _IsFrameworkAssemblySpecification;

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
            _IsFrameworkAssemblySpecification = isFrameworkAssemblySpecification;
            _ConfigurationMapBuilder = configurationMapBuilder;
            _AssemblyReaderFactory = assemblyReaderFactory;
            _AbstractClassTemplateBuilder = abstractClassTemplateBuilder;
        }

        public AssemblyReader BuildFor(Assembly domainAssembly)
        {
            if (domainAssembly == null)
                return null;

            var reader = _AssemblyReaderFactory();
            reader.Assembly = domainAssembly;
            reader.ConfigurationMap = _ConfigurationMapBuilder.BuildFor(domainAssembly);
            reader.XmlDocReader = new AssemblyDocsReader();

            this.Initialise(reader);

            reader.XmlDocReader.InitialiseFrom(reader);

            return reader;
        }


        private void Initialise(AssemblyReader reader)
        {
            reader.IsFrameworkAssembly = _IsFrameworkAssemblySpecification
                                            .IsSatisfiedBy(reader.Assembly.GetName())
                                            .Passed;


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
