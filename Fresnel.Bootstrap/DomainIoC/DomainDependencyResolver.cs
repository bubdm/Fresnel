using Autofac;
using Autofac.Core.Registration;
using Envivo.Fresnel.Introspection.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Bootstrap.DomainIoC
{
    public class DomainDependencyResolver : IDomainDependencyResolver
    {
        private IComponentContext _ComponentContext;

        public DomainDependencyResolver
            (
            IComponentContext componentContext
            )
        {
            _ComponentContext = componentContext;
        }

        public object Resolve(Type classType)
        {
            var result = _ComponentContext.Resolve(classType);
            return result;
        }
    }
}
