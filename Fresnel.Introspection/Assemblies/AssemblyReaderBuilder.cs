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
        private ConfigurationMapBuilder _ConfigurationMapBuilder;
        private Func<Assembly, ConfigurationMap, AssemblyDocsReader, AssemblyReader> _AssemblyReaderFactory;
        private AbstractClassTemplateBuilder _AbstractClassTemplateBuilder;

        public AssemblyReaderBuilder
        (
            ConfigurationMapBuilder configurationMapBuilder,
            Func<Assembly, ConfigurationMap, AssemblyDocsReader, AssemblyReader> assemblyReaderFactory,
            AbstractClassTemplateBuilder abstractClassTemplateBuilder
        )
        {
            _ConfigurationMapBuilder = configurationMapBuilder;
            _AssemblyReaderFactory = assemblyReaderFactory;
            _AbstractClassTemplateBuilder = abstractClassTemplateBuilder;
        }

        public AssemblyReader BuildFor(Assembly domainAssembly)
        {
            if (domainAssembly == null)
                return null;

            var configMap = _ConfigurationMapBuilder.BuildFor(domainAssembly);
            var docsReader = new AssemblyDocsReader();

            var reader = _AssemblyReaderFactory(domainAssembly, configMap, docsReader);
            
            this.Initialise(reader);
            this.CreateClassTemplates(reader, configMap);
            docsReader.InitialiseFrom(reader);

            return reader;
        }


        private void Initialise(AssemblyReader assemblyReader)
        {
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
