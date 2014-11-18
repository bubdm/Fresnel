
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Configuration
{
    public interface IAssemblyConfiguration
    {
        //IInfrastructureServiceConfiguration PersistenceConfig { get; }
        //IInfrastructureServiceConfiguration QueryRepositoryConfig { get; }
        //IInfrastructureServiceConfiguration PreferencesRepositoryConfig { get; }
        //IInfrastructureServiceConfiguration SecurityServiceConfig { get; }
        //IInfrastructureServiceConfiguration ExportServiceConfig { get; }

        //void ConfigurePersistence(IInfrastructureServiceConfiguration configuration);

        //void ConfigureQueryRepository(IInfrastructureServiceConfiguration configuration);

        //void ConfigurePreferencesRepository(IInfrastructureServiceConfiguration configuration);

        //void ConfigureSecurityService(IInfrastructureServiceConfiguration configuration);

        //void ConfigureExportService(IInfrastructureServiceConfiguration configuration);

        ICollection<Assembly> AssociatedAssemblies { get; }
    }
}
