
using System;
using System.Collections.Generic;
using System.Text;

namespace Envivo.Fresnel.Introspection.IoC
{
    public interface IDomainDependencyResolver
    {

        object Resolve(Type classType);

    }

}
