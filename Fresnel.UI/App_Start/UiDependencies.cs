using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Integration.Mvc;
using Envivo.Fresnel.UI.Controllers;

namespace Envivo.Fresnel.CompositionRoot
{
    public class UiDependencies : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(this.GetSingleInstanceTypes())
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .SingleInstance()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterTypes(this.GetPerRequestTypes())
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .InstancePerRequest()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }

        private Type[] GetSingleInstanceTypes()
        {
            return new Type[] { 
              
            };
        }

        private Type[] GetPerRequestTypes()
        {
            return new Type[] { 
                typeof(HomeController)
            };
        }
    }
}
