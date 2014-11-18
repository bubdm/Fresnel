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
            reader.AssemblyDocsReader = new AssemblyDocsReader();

            this.Initialise(reader);

            this.CreateClassTemplates(reader, reader.ConfigurationMap);
            reader.AssemblyDocsReader.InitialiseFrom(reader);

            return reader;
        }


        private void Initialise(AssemblyReader assemblyReader)
        {

            assemblyReader.IsFrameworkAssembly = _IsFrameworkAssemblySpecification
                                            .IsSatisfiedBy(assemblyReader.Assembly.GetName())
                                            .Passed;


            if (assemblyReader.IsFrameworkAssembly)
            {
                //assemblyReader.AreInfrastructureServicesEnabled = false;
            }
            else
            {
                //this.AreInfrastructureServicesEnabled = areInfrastructureServicesEnabled;
            }

            //_ClassStructureXml = new ClassStructureBuilder(_Assembly).GetClassStructureXml();
        }


        private void CreateClassTemplates(AssemblyReader assemblyReader, ConfigurationMap configMap)
        {
            //using (new Utils.ExecutionTimer(string.Concat("CreateClassTemplates for ", _AssemblyName)))
            {

                var publicTypes = assemblyReader.Assembly.GetExportedTypes();

                for (var i = 0; i < publicTypes.Length; i++)
                {
                    var type = publicTypes[i];

                    // These are the kinds of Types that we're interested in:
                    if (type.IsTrackable() ||
                        type.IsFactory() ||
                        type.IsRepository() ||
                        type.IsDomainService() ||
                        type.IsEnum)
                    {
                        var classConfig = configMap.GetClassConfiguration(type);
                        var tClass = _AbstractClassTemplateBuilder.CreateTemplate(type, classConfig);
                    }
                }
            }
        }

    }


}
