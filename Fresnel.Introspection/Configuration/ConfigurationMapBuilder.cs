using System.Reflection;

namespace Envivo.Fresnel.Configuration
{
    public class ConfigurationMapBuilder
    {
        public ConfigurationMapBuilder
        (
        )
        {
        }

        /// <summary>
        /// Identifies and caches the Configuration objects from the given assembly.
        /// Note that the given assembly may contain configurations for _other_ assemblies
        /// </summary>
        /// <param name="assembly"></param>
        public ConfigurationMap BuildFor(Assembly assembly)
        {
            var map = new ConfigurationMap();

            var publicTypes = assembly.GetExportedTypes();

            //for (var i = 0; i < publicTypes.Length; i++)
            //{
            //    var type = publicTypes[i];

            //    if (type.IsInterface || type.IsAbstract)
            //        continue;

            //    Type objectType;
            //    try
            //    {
            //        Assembly domainAssembly = null;

            //        if (type.IsClassConfiguration(out objectType))
            //        {
            //            var classConfig = (IClassConfiguration)Activator.CreateInstance(type);
            //            // Replace the existing configuration:
            //            _ClassConfigurationMap[objectType] = classConfig;
            //        }
            //        else if (type.IsAssemblyConfiguration(out domainAssembly))
            //        {
            //            var assemblyConfig = (IAssemblyConfiguration)Activator.CreateInstance(type);
            //            // Replace the existing configuration:
            //            _AssemblyConfigurationMap[domainAssembly] = assemblyConfig;
            //        }
            //        else if (type.IsApplicationConfiguration())
            //        {
            //            var applicationConfig = (IApplicationConfiguration)Activator.CreateInstance(type);
            //            // Replace the existing configuration:
            //            _ApplicationConfiguration = applicationConfig;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Trace.TraceError(ex.ToString());
            //    }
            //}

            this.FixMissingConfigurationsFor(map, assembly);

            return map;
        }

        private void FixMissingConfigurationsFor(ConfigurationMap configurationMap, Assembly assembly)
        {
            //if (_ApplicationConfiguration == null)
            //{
            //    _ApplicationConfiguration = new DefaultApplicationConfiguration();
            //}

            ////-----

            //IAssemblyConfiguration defaultConfiguration;
            //if (_IsFrameworkAssemblySpecification.IsSatisfiedBy(assembly.GetName()).Passed)
            //{
            //    defaultConfiguration = new DefaultFrameworkAssemblyConfiguration(assembly);
            //}
            //else
            //{
            //    defaultConfiguration = new DefaultDomainAssemblyConfiguration(assembly);
            //}

            ////-----
            //var assemblyConfig = this.GetAssemblyConfigurationFor(assembly);

            //if (assemblyConfig == null)
            //{
            //    assemblyConfig = defaultConfiguration;
            //    _AssemblyConfigurationMap.Add(assembly, assemblyConfig);
            //}

            //if (assemblyConfig.PersistenceConfig == null)
            //{
            //    assemblyConfig.ConfigurePersistence(defaultConfiguration.PersistenceConfig);
            //}

            //if (assemblyConfig.QueryRepositoryConfig == null)
            //{
            //    assemblyConfig.ConfigureQueryRepository(defaultConfiguration.QueryRepositoryConfig);
            //}

            //if (assemblyConfig.PreferencesRepositoryConfig == null)
            //{
            //    assemblyConfig.ConfigurePreferencesRepository(defaultConfiguration.PreferencesRepositoryConfig);
            //}

            //if (assemblyConfig.SecurityServiceConfig == null)
            //{
            //    assemblyConfig.ConfigureSecurityService(defaultConfiguration.SecurityServiceConfig);
            //}

            //if (assemblyConfig.ExportServiceConfig == null)
            //{
            //    assemblyConfig.ConfigureExportService(defaultConfiguration.ExportServiceConfig);
            //}
        }
    }
}