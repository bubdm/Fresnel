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
using System.Configuration;
using System.Reflection;

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

            var domainAssemblies = GetDomainAssemblies();

            ConfigureAutofac(domainAssemblies);

            RegisterDomainAssembies(domainAssemblies);
        }
        
        private static IEnumerable<Assembly> GetDomainAssemblies()
        {
            var results = new List<Assembly>();

            var domainAssemblyPath = ConfigurationManager.AppSettings["DomainAssemblyFile"];
            var domainAssembly = Assembly.LoadFile(domainAssemblyPath);
            results.Add(domainAssembly);

            var persistenceAssemblyPath = ConfigurationManager.AppSettings["DomainAssemblyFile"];
            var persistenceAssembly = Assembly.LoadFile(persistenceAssemblyPath);
            results.Add(persistenceAssembly);

            return results;
        }

        private static void RegisterDomainAssembies(IEnumerable<Assembly> assemblies)
        {
            var engine = DependencyResolver.Current.GetService<Core.Engine>();

            foreach (var assembly in assemblies)
            {
                engine.RegisterDomainAssembly(assembly);
            }
        }

        private static void ConfigureExternalControllers()
        {
            // HACK: This is to force the UiCore controllers to be recognised by MVC:
            // See http://stackoverflow.com/a/23862646/80369
            Type dummy = typeof(UiCore.Controllers.ExplorerController);
            Trace.TraceInformation(dummy.FullName);
        }

        private static void ConfigureAutofac(IEnumerable<Assembly> assemblies)
        {
            // Determine if any modules exist within the Domain assemblies:
            var additionalModules = assemblies
                                    .SelectMany(a => a.GetExportedTypes())
                                    .Where(t => t.IsSubclassOf(typeof(Autofac.Module)))
                                    .Select(t => Activator.CreateInstance(t))
                                    .Cast<Autofac.Module>()
                                    .ToList();
            additionalModules.Add(new UiDependencies());

            var container = new ContainerFactory()
                                .Build(additionalModules);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}