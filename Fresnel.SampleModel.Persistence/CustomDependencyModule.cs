using Autofac;

namespace Fresnel.SampleModel.Persistence
{
    /// <summary>
    /// Used to register the custom EntityFramework PersistenceService
    /// </summary>
    public class CustomDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // NB: MARS is important:
            var connectionString = @"Server=CHRONOS\SQLEXPRESS;Database=SampleModel;Integrated Security=True;MultipleActiveResultSets=true";

            builder.RegisterType<RealTypeResolver>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ModelConfigurator>()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ModelContext>()
                    .WithParameter("nameOrConnectionString", connectionString)
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .InstancePerDependency();

            builder.RegisterType<EFPersistenceService>()
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }
    }
}