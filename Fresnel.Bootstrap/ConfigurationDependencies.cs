using Autofac;
using System;

namespace Envivo.Fresnel.Bootstrap
{
    public class ConfigurationDependencies : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(this.GetSingleInstanceTypes())
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .SingleInstance()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetPerDependencyInstanceTypes())
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .InstancePerDependency()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetPerSessionInstanceTypes())
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .InstancePerLifetimeScope()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }

        private Type[] GetSingleInstanceTypes()
        {
            return new Type[] {
                typeof(Fresnel.Configuration.AttributesMapBuilder),
                typeof(Fresnel.Configuration.AssemblyConfigurationMapBuilder),

                typeof(Fresnel.Configuration.DisplayBooleanAttributeBuilder),
                typeof(Fresnel.Configuration.VisibilityAttributeBuilder),
            };
        }

        private Type[] GetPerDependencyInstanceTypes()
        {
            return new Type[] {
                typeof(Fresnel.Configuration.AttributesMap),
                typeof(Fresnel.Configuration.AssemblyConfigurationMap),
            };
        }

        private Type[] GetPerSessionInstanceTypes()
        {
            return new Type[] {
            };
        }
    }
}