using Autofac;
using System;

namespace Envivo.Fresnel.CompositionRoot
{
    public class CoreDependencies : Module
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
                typeof(Fresnel.Core.Engine),

                typeof(Fresnel.Core.OuterObjectsIdentifier),

                typeof(Fresnel.Core.Observers.AbstractObserverBuilder),
                typeof(Fresnel.Core.Observers.MethodObserverBuilder),
                typeof(Fresnel.Core.Observers.MethodObserverMapBuilder),
                typeof(Fresnel.Core.Observers.ObjectIdResolver),
                typeof(Fresnel.Core.Observers.ObserverCacheSynchroniser),
                typeof(Fresnel.Core.Observers.ParameterObserverMapBuilder),
                typeof(Fresnel.Core.Observers.PropertyObserverBuilder),
                typeof(Fresnel.Core.Observers.PropertyObserverMapBuilder),

                typeof(Fresnel.Core.Permissions.CanCreatePermission),
                typeof(Fresnel.Core.Permissions.CanGetPropertyPermission),
                typeof(Fresnel.Core.Permissions.CanSetPropertyPermission),
                typeof(Fresnel.Core.Permissions.CanInvokeMethodPermission),

                typeof(Fresnel.Core.ChangeTracking.DirtyObjectNotifier),
            };
        }

        private Type[] GetPerDependencyInstanceTypes()
        {
            return new Type[] {
                typeof(Fresnel.Core.Observers.CollectionObserver),
                typeof(Fresnel.Core.Observers.EnumObserver),
                typeof(Fresnel.Core.Observers.MethodObserver),
                typeof(Fresnel.Core.Observers.NonReferenceObserver),
                typeof(Fresnel.Core.Observers.ObjectObserver),
                typeof(Fresnel.Core.Observers.ObjectPropertyObserver),
                typeof(Fresnel.Core.Observers.NullObserver),
                typeof(Fresnel.Core.Observers.ParameterObserver),

                typeof(Fresnel.Core.ChangeTracking.AbstractChangeTrackerBuilder),
                typeof(Fresnel.Core.ChangeTracking.ObjectTracker),
                typeof(Fresnel.Core.ChangeTracking.CollectionTracker),
            };
        }

        private Type[] GetPerSessionInstanceTypes()
        {
            return new Type[] {
                // These depend on the ObserverCache, hence the need for them being Per Session:
                typeof(Fresnel.Core.Commands.AddToCollectionCommand),
                typeof(Fresnel.Core.Commands.ClearCollectionCommand),
                typeof(Fresnel.Core.Commands.CloneObjectCommand),
                typeof(Fresnel.Core.Commands.CreateObjectCommand),
                typeof(Fresnel.Core.Commands.GetCollectionItemsCommand),
                typeof(Fresnel.Core.Commands.GetPropertyCommand),
                typeof(Fresnel.Core.Commands.InvokeMethodCommand),
                typeof(Fresnel.Core.Commands.RemoveFromCollectionCommand),
                typeof(Fresnel.Core.Commands.SetParameterCommand),
                typeof(Fresnel.Core.Commands.SetPropertyCommand),
                typeof(Fresnel.Core.Commands.SaveObjectCommand),
                typeof(Fresnel.Core.Commands.SearchCommand),

                typeof(Fresnel.Core.Persistence.NullPersistenceService),

                typeof(Fresnel.Core.Observers.ObserverCache),
                typeof(Fresnel.Core.Observers.ObserverCacheSynchroniser),
                typeof(Fresnel.Core.IdentityMap),
                typeof(Fresnel.Core.UserSession),
            };
        }
    }
}