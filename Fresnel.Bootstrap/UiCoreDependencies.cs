using Autofac;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.CompositionRoot
{
    public class UiCoreDependencies : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(this.GetSingleInstanceTypes())
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .SingleInstance()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetPerRequestInstanceTypes())
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .InstancePerRequest()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<SystemClock>().As<IClock>()
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .SingleInstance()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }

        private Type[] GetSingleInstanceTypes()
        {
            return new Type[] {
                typeof(Fresnel.UiCore.SessionVmBuilder),
            };
        }

        private Type[] GetPerRequestInstanceTypes()
        {
            // These are created per request:
            return new Type[] {
                typeof(Fresnel.UiCore.Commands.GetDomainLibraryCommand),
                typeof(Fresnel.UiCore.Commands.CleanupSessionCommand),
                typeof(Fresnel.UiCore.Commands.CreateObjectCommand),
                typeof(Fresnel.UiCore.Commands.GetObjectCommand),
                typeof(Fresnel.UiCore.Commands.GetPropertyCommand),
                typeof(Fresnel.UiCore.Commands.CreateAndSetPropertyCommand),
                typeof(Fresnel.UiCore.Commands.SetPropertyCommand),
                typeof(Fresnel.UiCore.Commands.SetParameterCommand),
                typeof(Fresnel.UiCore.Commands.InvokeMethodCommand),
                typeof(Fresnel.UiCore.Commands.CollectionAddCommand),
                typeof(Fresnel.UiCore.Commands.CollectionRemoveCommand),
                typeof(Fresnel.UiCore.Commands.SaveChangesCommand),
                typeof(Fresnel.UiCore.Commands.CancelChangesCommand),
                typeof(Fresnel.UiCore.Commands.SearchObjectsCommand),
                typeof(Fresnel.UiCore.Commands.SearchPropertyCommand),
                typeof(Fresnel.UiCore.Commands.SearchParameterCommand),
                typeof(Fresnel.UiCore.Commands.SearchResultsFilterApplier),

                typeof(Fresnel.UiCore.Controllers.ToolboxController),
                typeof(Fresnel.UiCore.Controllers.ExplorerController),
                typeof(Fresnel.UiCore.Controllers.SessionController),
                typeof(Fresnel.UiCore.Controllers.TestController),

                typeof(Fresnel.UiCore.AbstractObjectVmBuilder),
                typeof(Fresnel.UiCore.ObjectVmBuilder),
                typeof(Fresnel.UiCore.CollectionVmBuilder),
                typeof(Fresnel.UiCore.CollectionInteractionsBuilder),
                typeof(Fresnel.UiCore.DirtyStateVmBuilder),
                typeof(Fresnel.UiCore.EmptyPropertyVmBuilder),
                typeof(Fresnel.UiCore.ExceptionMessagesBuilder),
                typeof(Fresnel.UiCore.PropertyVmBuilder),
                typeof(Fresnel.UiCore.PropertyStateVmBuilder),
                typeof(Fresnel.UiCore.ParameterStateVmBuilder),
                typeof(Fresnel.UiCore.MethodVmBuilder),
                typeof(Fresnel.UiCore.ParameterVmBuilder),
                typeof(Fresnel.UiCore.SearchResultsVmBuilder),
                typeof(Fresnel.UiCore.SearchFilterPropertyVmBuilder),

                typeof(Fresnel.UiCore.Model.Changes.ModificationsVmBuilder),
                typeof(Fresnel.UiCore.Model.Classes.ClassItemBuilder),
                typeof(Fresnel.UiCore.Model.Classes.DomainClassesBuilder),
                typeof(Fresnel.UiCore.Model.Classes.DomainServicesBuilder),
                typeof(Fresnel.UiCore.Model.Classes.DomainServiceItemBuilder),
                typeof(Fresnel.UiCore.Model.TypeInfo.DataTypeToUiControlMapper),
                typeof(Fresnel.UiCore.Model.TypeInfo.BooleanVmBuilder),
                typeof(Fresnel.UiCore.Model.TypeInfo.BooleanValueFormatter),
                typeof(Fresnel.UiCore.Model.TypeInfo.DateTimeVmBuilder),
                typeof(Fresnel.UiCore.Model.TypeInfo.DateTimeValueFormatter),
                typeof(Fresnel.UiCore.Model.TypeInfo.EnumVmBuilder),
                typeof(Fresnel.UiCore.Model.TypeInfo.NumberVmBuilder),
                typeof(Fresnel.UiCore.Model.TypeInfo.StringVmBuilder),
                typeof(Fresnel.UiCore.Model.TypeInfo.ObjectSelectionVmBuilder),
                typeof(Fresnel.UiCore.Model.TypeInfo.UnknownVmBuilder),
            };
        }
    }
}