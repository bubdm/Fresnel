





using System.Reflection;

namespace Envivo.TrueView.Domain.Configuration
{

    internal class DefaultFrameworkAssemblyConfiguration : AssemblyConfiguration<object>
    {
        public DefaultFrameworkAssemblyConfiguration(Assembly domainAssembly)
        {
            this.ConfigurePersistence(new Persistence.InMemoryDtoConfiguration());
            this.ConfigureQueryRepository(new Persistence.QueryTokenRepositoryConfiguration());
            this.ConfigurePreferencesRepository(new Preferences.UserPreferencesFileRepositoryConfiguration());
            this.ConfigureSecurityService(new Security.SuperUserSecurityConfiguration());
            this.ConfigureExportService(new Export.DefaultConfiguration());
        }

    }

}
