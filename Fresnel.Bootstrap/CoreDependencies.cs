using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Bootstrap
{
    public class CoreDependencies : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterTypes(this.GetSingleInstanceTypes())
                    .SingleInstance()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetPerDependencyInstanceTypes())
                    .InstancePerDependency()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetPerSessionInstanceTypes())
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
                typeof(Fresnel.Core.Observers.ParameterObserverMapBuilder),
                typeof(Fresnel.Core.Observers.PropertyObserverBuilder),
                typeof(Fresnel.Core.Observers.PropertyObserverMapBuilder),

                typeof(Fresnel.Core.Observers.NullObserver),
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
                typeof(Fresnel.Core.Observers.ParameterObserver),
            };
        }

        private Type[] GetPerSessionInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.Core.Observers.ObserverCache),
                typeof(Fresnel.Core.Persistence.UnitOfWork),
                typeof(Fresnel.Core.IdentityMap),
                typeof(Fresnel.Core.UserSession),
            };
        }

    }
}
