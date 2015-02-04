using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using Envivo.Fresnel.Core.Persistence;
using System.Data.Entity.Core;
using System.Linq.Expressions;
using Autofac;
using System.Data.Entity;

namespace Fresnel.SampleModel.Persistence
{
    public class CustomDependencyModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            var connectionString = @"Server=CHRONOS\SQLEXPRESS;Database=SampleModel;Integrated Security=True;";

            builder.RegisterType<ModelContext>()
                    .WithParameter("nameOrConnectionString", connectionString)
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .InstancePerLifetimeScope();

            builder.RegisterType<EFPersistenceService>()
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }

    }
}
