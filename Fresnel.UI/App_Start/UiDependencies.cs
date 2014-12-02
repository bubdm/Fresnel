using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Integration.Mvc;
using Envivo.Fresnel.UI.Controllers;

namespace Envivo.Fresnel.Bootstrap
{
    public class UiDependencies : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HomeController>().InstancePerLifetimeScope();
            builder.RegisterType<ToolboxController>().InstancePerLifetimeScope();
            builder.RegisterType<ObjectExplorerController>().InstancePerLifetimeScope();
        }

    }
}
