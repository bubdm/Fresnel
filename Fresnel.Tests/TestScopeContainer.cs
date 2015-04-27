using Autofac.Integration.WebApi;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;
using Envivo.Fresnel.SampleModel.TestTypes;
using Fresnel.SampleModel.Persistence;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace Fresnel.Tests
{
    public class TestScopeContainer
    {
        private HttpConfiguration _HttpConfiguration = null;
        private IDependencyScope _Scope = null;

        public TestScopeContainer(params Autofac.Module[] additionalModules)
        {
            var container = new ContainerFactory().Build(additionalModules);
            _HttpConfiguration = new HttpConfiguration
            {
                DependencyResolver = new AutofacWebApiDependencyResolver(container)
            };
        }

        public IDependencyScope BeginScope()
        {
            _Scope = _HttpConfiguration.DependencyResolver.BeginScope();
            return _Scope;
        }

        public T Resolve<T>()
        {
            return (T)_Scope.GetService(typeof(T));
        }

    }

}
