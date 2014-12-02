using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Envivo.Fresnel.Bootstrap;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Diagnostics;

namespace Envivo.Fresnel.UI
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            ConfigureExternalControllers();

            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ConfigureAutofac();

            // TODO: Decouple this registration:
            var engine = DependencyResolver.Current.GetService<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);
        }

        private static void ConfigureExternalControllers()
        {
            // HACK: This is to force the UiCore controllers to be recognised by MVC:
            // See http://stackoverflow.com/a/23862646/80369
            Type dummy = typeof(UiCore.Controllers.ExplorerController);
            Trace.TraceInformation(dummy.FullName);
        }

        private static void ConfigureAutofac()
        {
            var additionalDependencies = new Module[] { new UiDependencies() };
            var container = new ContainerFactory()
                                .Build(additionalDependencies);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}