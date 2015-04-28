using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Assemblies
{
    public class AssemblyReaderBuilder
    {
        private AssemblyConfigurationMapBuilder _ConfigurationMapBuilder;
        private Func<AssemblyReader> _AssemblyReaderFactory;
        private AbstractClassTemplateBuilder _AbstractClassTemplateBuilder;
        private IDomainDependencyRegistrar _DomainDependencyRegistrar;

        public AssemblyReaderBuilder
        (
            AssemblyConfigurationMapBuilder configurationMapBuilder,
            Func<AssemblyReader> assemblyReaderFactory,
            AbstractClassTemplateBuilder abstractClassTemplateBuilder,
            IDomainDependencyRegistrar domainDependencyRegistrar
        )
        {
            _ConfigurationMapBuilder = configurationMapBuilder;
            _AssemblyReaderFactory = assemblyReaderFactory;
            _AbstractClassTemplateBuilder = abstractClassTemplateBuilder;
            _DomainDependencyRegistrar = domainDependencyRegistrar;
        }

        public AssemblyReader BuildFor(Assembly domainAssembly, bool isSystemAssembly)
        {
            if (domainAssembly == null)
                return null;

            var reader = _AssemblyReaderFactory();
            reader.Assembly = domainAssembly;
            reader.ConfigurationMap = _ConfigurationMapBuilder.BuildFor(domainAssembly);
            reader.XmlDocReader = new XmlDocsReader();
            reader.IsFrameworkAssembly = isSystemAssembly;

            this.Initialise(reader);
            this.InitialiseDomainDependencies(domainAssembly);
            reader.XmlDocReader.InitialiseFrom(reader);

            return reader;
        }

        private void Initialise(AssemblyReader reader)
        {
            if (reader.IsFrameworkAssembly)
            {
            }
            else
            {
                // We need to ensure that all top-level Domain Classes are recognised:
                reader.PreLoadClassTemplates();
            }
        }

        private void InitialiseDomainDependencies(Assembly domainAssembly)
        {
            var publicTypes = domainAssembly.GetExportedTypes();

            var dependencyTypes = publicTypes
                                    .Where(t => t.IsDerivedFrom<IDomainDependency>())
                                    .ToArray();

            _DomainDependencyRegistrar.RegisterTypes(dependencyTypes);
        }

    }
}