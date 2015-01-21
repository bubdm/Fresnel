using Autofac;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.Bootstrap
{
    public class UiCoreDependencies : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(this.GetSingleInstanceTypes())
                    .SingleInstance()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetPerDependencyInstanceTypes())
                    .InstancePerDependency()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<SystemClock>().As<IClock>()
                    .SingleInstance()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }

        private Type[] GetSingleInstanceTypes()
        {
            return new Type[] {
                typeof(Fresnel.UiCore.Commands.GetClassHierarchyCommand),
                typeof(Fresnel.UiCore.Commands.CleanupSessionCommand),
                typeof(Fresnel.UiCore.Commands.CreateCommand),
                typeof(Fresnel.UiCore.Commands.GetObjectCommand),
                typeof(Fresnel.UiCore.Commands.GetPropertyCommand),
                typeof(Fresnel.UiCore.Commands.SetPropertyCommand),
                typeof(Fresnel.UiCore.Commands.InvokeMethodCommand),
                typeof(Fresnel.UiCore.Commands.CollectionAddCommand),

                typeof(Fresnel.UiCore.Changes.ModificationsBuilder),

                typeof(Fresnel.UiCore.Classes.ClassItemBuilder),
                typeof(Fresnel.UiCore.Classes.NamespacesBuilder),

                typeof(Fresnel.UiCore.TypeInfo.BooleanVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.DateTimeVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.EnumVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.NumberVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.StringVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.ObjectSelectionVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.UnknownVmBuilder),

                typeof(Fresnel.UiCore.SessionVmBuilder),
            };
        }

        private Type[] GetPerDependencyInstanceTypes()
        {
            return new Type[] {
                typeof(Fresnel.UiCore.Controllers.ToolboxController),
                typeof(Fresnel.UiCore.Controllers.ExplorerController),
                typeof(Fresnel.UiCore.Controllers.SessionController),
                typeof(Fresnel.UiCore.Controllers.TestController),

                typeof(Fresnel.UiCore.AbstractObjectVMBuilder),
                typeof(Fresnel.UiCore.AbstractPropertyVmBuilder),
                typeof(Fresnel.UiCore.PropertyStateVmBuilder),
                typeof(Fresnel.UiCore.MethodVmBuilder),
            };
        }
    }
}