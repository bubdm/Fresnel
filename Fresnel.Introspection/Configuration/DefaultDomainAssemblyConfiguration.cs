




using System.Reflection;

namespace Envivo.TrueView.Domain.Configuration
{

    internal class DefaultDomainAssemblyConfiguration : AssemblyConfiguration<object>
    {
        public DefaultDomainAssemblyConfiguration(Assembly domainAssembly)
        {
            this.ConfigurePersistence(new Persistence.InMemoryDtoConfiguration());
            this.ConfigureQueryRepository(new Persistence.QueryTokenRepositoryConfiguration());
            this.ConfigurePreferencesRepository(new Preferences.UserPreferencesFileRepositoryConfiguration());
            this.ConfigureSecurityService(new Security.ClassConfiguredSecurityServiceConfiguration());
            this.ConfigureExportService(new Export.DefaultConfiguration());
        }

    }

}
