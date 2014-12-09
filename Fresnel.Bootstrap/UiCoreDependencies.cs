using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private Type[] GetSingleInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.UiCore.Commands.GetClassHierarchyCommand),
                typeof(Fresnel.UiCore.Commands.CreateCommand),
                typeof(Fresnel.UiCore.ClassHierarchy.ClassHierarchyItemBuilder),
                typeof(Fresnel.UiCore.TypeInfo.TypeInfoBuilder),
                typeof(Fresnel.UiCore.TypeInfo.BooleanVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.DateTimeVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.EnumVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.NumberVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.StringVmBuilder),
                typeof(Fresnel.UiCore.TypeInfo.ObjectSelectionVmBuilder),
            };
        }

        private Type[] GetPerDependencyInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.UiCore.Controllers.ToolboxController),
                typeof(Fresnel.UiCore.Controllers.ExplorerController),
                typeof(Fresnel.UiCore.Controllers.TestController),

                typeof(Fresnel.UiCore.Objects.AbstractObjectVMBuilder),
                typeof(Fresnel.UiCore.Objects.PropertyVmBuilder),
            };
        }

    }
}
