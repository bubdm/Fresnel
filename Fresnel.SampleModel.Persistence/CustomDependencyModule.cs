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
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ModelConfigurator>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ModelContext>()
                    .WithParameter("nameOrConnectionString", connectionString)
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .InstancePerRequest();

            builder.RegisterType<EFPersistenceService>()
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .InstancePerRequest();
        }
    }
}