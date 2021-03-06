using Autofac;
using System;

namespace Envivo.Fresnel.CompositionRoot
{
    public class IntrospectionDependencies : Module
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
        }

        private Type[] GetSingleInstanceTypes()
        {
            return new Type[] {
                typeof(Fresnel.Introspection.RealTypeResolver),

                typeof(Fresnel.Configuration.AttributesMapBuilder),
                typeof(Fresnel.Configuration.AssemblyConfigurationMapBuilder),

                typeof(Fresnel.Introspection.Assemblies.XmlDocsReader),
                typeof(Fresnel.Introspection.Assemblies.AssemblyReaderBuilder),
                typeof(Fresnel.Introspection.Assemblies.AssemblyReaderMapBuilder),
                typeof(Fresnel.Introspection.Assemblies.NamespaceHierarchyBuilder),

                typeof(Fresnel.Introspection.Commands.CreateObjectCommand),
                typeof(Fresnel.Introspection.Commands.CloneObjectCommand),
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
                typeof(Fresnel.Introspection.Templates.CollectionAddMethodIdentifier),
                typeof(Fresnel.Introspection.Templates.CollectionRemoveMethodIdentifier),
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

        private Type[] GetPerDependencyInstanceTypes()
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
    }
}