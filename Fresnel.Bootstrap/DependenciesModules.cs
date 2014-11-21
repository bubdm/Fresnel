using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Bootstrap
{
    public class DependenciesModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(this.GetIntrospectionSingleInstanceTypes())
                    .SingleInstance()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetIntrospectionPerDependencyInstanceTypes())
                    .InstancePerDependency()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            //-----

            builder.RegisterTypes(this.GetEngineSingleInstanceTypes())
                    .SingleInstance()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetEnginePerDependencyInstanceTypes())
                    .InstancePerDependency()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetPerSessionInstanceTypes())
                    .InstancePerLifetimeScope()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }

        private Type[] GetIntrospectionSingleInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.Introspection.RealTypeResolver),

                typeof(Fresnel.Configuration.AttributesMapBuilder),
                typeof(Fresnel.Configuration.ConfigurationMapBuilder),

                typeof(Fresnel.Introspection.Assemblies.AssemblyDocsReader),
                typeof(Fresnel.Introspection.Assemblies.AssemblyReaderBuilder),
                typeof(Fresnel.Introspection.Assemblies.AssemblyReaderMapBuilder),
                typeof(Fresnel.Introspection.Assemblies.IsFrameworkAssemblySpecification),
                typeof(Fresnel.Introspection.Assemblies.NamespaceHierarchyBuilder),

                typeof(Fresnel.Introspection.Commands.CreateObjectCommand),
                typeof(Fresnel.Introspection.Commands.GetPropertyCommand),
                typeof(Fresnel.Introspection.Commands.GetBackingFieldCommand),
                typeof(Fresnel.Introspection.Commands.SetPropertyCommand),
                typeof(Fresnel.Introspection.Commands.SetBackingFieldCommand),
                typeof(Fresnel.Introspection.Commands.AddToCollectionCommand),
                typeof(Fresnel.Introspection.Commands.RemoveFromCollectionCommand),
                typeof(Fresnel.Introspection.Commands.InvokeMethodCommand),

                typeof(Fresnel.Introspection.Templates.AbstractClassTemplateBuilder),
                typeof(Fresnel.Introspection.Templates.BackingFieldIdentifier),
                typeof(Fresnel.Introspection.Templates.ClassHierarchyBuilder),
                typeof(Fresnel.Introspection.Templates.ClassTemplateBuilder),
                typeof(Fresnel.Introspection.Templates.ClassHierarchyDepthComparer),
                typeof(Fresnel.Introspection.Templates.ClassTemplateBuilder),
                typeof(Fresnel.Introspection.Templates.CollectionTemplateBuilder),
                typeof(Fresnel.Introspection.Templates.CollectionTypeIdentifier),
                typeof(Fresnel.Introspection.Templates.DynamicMethodBuilder),
                typeof(Fresnel.Introspection.Templates.EnumItemTemplateBuilder),
                typeof(Fresnel.Introspection.Templates.EnumItemTemplateMapBuilder),
                typeof(Fresnel.Introspection.Templates.EnumTemplateBulider),
                typeof(Fresnel.Introspection.Templates.EnumTemplateMapBuilder),
                typeof(Fresnel.Introspection.Templates.FieldInfoMapBuilder),
                typeof(Fresnel.Introspection.Templates.MethodInfoMapBuilder),
                typeof(Fresnel.Introspection.Templates.MethodTemplateBuilder),
                typeof(Fresnel.Introspection.Templates.MethodTemplateMapBuilder),
                typeof(Fresnel.Introspection.Templates.NonReferenceTemplateBuilder),
                typeof(Fresnel.Introspection.Templates.ParameterTemplateBuilder),
                typeof(Fresnel.Introspection.Templates.ParameterTemplateMapBuilder),
                typeof(Fresnel.Introspection.Templates.PropertyInfoMapBuilder),
                typeof(Fresnel.Introspection.Templates.PropertyTemplateBuilder),
                typeof(Fresnel.Introspection.Templates.PropertyTemplateMapBuilder),
                typeof(Fresnel.Introspection.Templates.StaticMethodInfoMapBuilder),
                typeof(Fresnel.Introspection.Templates.StaticMethodTemplateMapBuilder),
                typeof(Fresnel.Introspection.Templates.TrackingPropertiesIdentifier),

                typeof(Fresnel.Introspection.Templates.IsObjectAuditableSpecification),
                typeof(Fresnel.Introspection.Templates.IsObjectTrackableSpecification),
                typeof(Fresnel.Introspection.Templates.IsObjectValidatableSpecification),

                typeof(Fresnel.Introspection.Assemblies.AssemblyReaderMap),
                typeof(Fresnel.Introspection.TemplateCache),
            };
        }

        private Type[] GetEngineSingleInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.Engine.Engine),

                typeof(Fresnel.Engine.OuterObjectsIdentifier),

                typeof(Fresnel.Engine.Observers.AbstractObserverBuilder),
                typeof(Fresnel.Engine.Observers.MethodObserverBuilder),
                typeof(Fresnel.Engine.Observers.MethodObserverMapBuilder),
                typeof(Fresnel.Engine.Observers.ObjectIdResolver),
                typeof(Fresnel.Engine.Observers.ParameterObserverMapBuilder),
                typeof(Fresnel.Engine.Observers.PropertyObserverBuilder),
                typeof(Fresnel.Engine.Observers.PropertyObserverMapBuilder),

                typeof(Fresnel.Engine.Observers.NullObserver),
            };
        }

        private Type[] GetIntrospectionPerDependencyInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.Introspection.Assemblies.AssemblyReader),

                typeof(Fresnel.Introspection.Templates.ClassTemplate),
                typeof(Fresnel.Introspection.Templates.CollectionTemplate),
                typeof(Fresnel.Introspection.Templates.EnumItemTemplate),
                typeof(Fresnel.Introspection.Templates.EnumTemplate),
                typeof(Fresnel.Introspection.Templates.MethodTemplate),
                typeof(Fresnel.Introspection.Templates.NonReferenceTemplate),
                typeof(Fresnel.Introspection.Templates.ParameterTemplate),
                typeof(Fresnel.Introspection.Templates.PropertyTemplate),
            };
        }

        private Type[] GetEnginePerDependencyInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.Engine.Observers.CollectionObserver),
                typeof(Fresnel.Engine.Observers.EnumObserver),
                typeof(Fresnel.Engine.Observers.MethodObserver),
                typeof(Fresnel.Engine.Observers.NonReferenceObserver),
                typeof(Fresnel.Engine.Observers.ObjectObserver),
                typeof(Fresnel.Engine.Observers.ObjectPropertyObserver),
                typeof(Fresnel.Engine.Observers.ParameterObserver),
            };
        }

        private Type[] GetPerSessionInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.Engine.Observers.ObserverCache),
                typeof(Fresnel.Engine.Persistence.UnitOfWork),
                typeof(Fresnel.Engine.IdentityMap),
                typeof(Fresnel.Engine.UserSession),
            };
        }

    }
}
