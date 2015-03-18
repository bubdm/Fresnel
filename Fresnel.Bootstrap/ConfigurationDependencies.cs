using Autofac;
using System;

namespace Envivo.Fresnel.CompositionRoot
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
                typeof(Fresnel.Configuration.RangeAttributeBuilder),
                typeof(Fresnel.Configuration.DataTypeAttributeBuilder),
                typeof(Fresnel.Configuration.MinLengthAttributeBuilder),
                typeof(Fresnel.Configuration.MaxLengthAttributeBuilder),
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