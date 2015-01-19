using Envivo.Fresnel.DomainTypes.Interfaces;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Used to configure a Domain Assembly
    /// </summary>
    /// <typeparam name="TClass">Specify any public class in the assembly being configured</typeparam>
    public abstract class AssemblyConfiguration<T> : IAssemblyConfiguration
    {
        public AssemblyConfiguration()
        {
            this.AssociatedAssemblies = new List<Assembly>();
            this.AssociatedAssemblies.Add(Assembly.GetCallingAssembly());
            this.AssociatedAssemblies.Add(typeof(IDomainObject).Assembly);
        }

        //public IInfrastructureServiceConfiguration PersistenceConfig { get; private set; }
        //public IInfrastructureServiceConfiguration QueryRepositoryConfig { get; private set; }
        //public IInfrastructureServiceConfiguration PreferencesRepositoryConfig { get; private set; }
        //public IInfrastructureServiceConfiguration SecurityServiceConfig { get; private set; }
        //public IInfrastructureServiceConfiguration ExportServiceConfig { get; private set; }

        ///// <summary>
        ///// Configures the Persistence Service to use for the associated Assembly(s)
        ///// </summary>
        ///// <param name="configuration"></param>
        //public void ConfigurePersistence(IInfrastructureServiceConfiguration configuration)
        //{
        //    this.PersistenceConfig = configuration;
        //}

        ///// <summary>
        ///// Configures the Query Repository to use for the associated Assembly(s)
        ///// </summary>
        ///// <param name="configuration"></param>
        //public void ConfigureQueryRepository(IInfrastructureServiceConfiguration configuration)
        //{
        //    this.QueryRepositoryConfig = configuration;
        //}

        ///// <summary>
        ///// Configures the User Preferences Repository to use for the associated Assembly(s)
        ///// </summary>
        ///// <param name="configuration"></param>
        //public void ConfigurePreferencesRepository(IInfrastructureServiceConfiguration configuration)
        //{
        //    this.PreferencesRepositoryConfig = configuration;
        //}

        ///// <summary>
        ///// Configures the Security Service to use for the associated Assembly(s)
        ///// </summary>
        ///// <param name="configuration"></param>
        //public void ConfigureSecurityService(IInfrastructureServiceConfiguration configuration)
        //{
        //    this.SecurityServiceConfig = configuration;
        //}

        ///// <summary>
        ///// Configures the Export Service to use for the associated Assembly(s)
        ///// </summary>
        ///// <param name="configuration"></param>
        //public void ConfigureExportService(IInfrastructureServiceConfiguration configuration)
        //{
        //    this.ExportServiceConfig = configuration;
        //}

        /// <summary>
        /// A list of all Assemblies that this Configuration applies to
        /// </summary>
        public ICollection<Assembly> AssociatedAssemblies { get; private set; }
    }
}