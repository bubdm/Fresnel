
using System;
using System.Collections.Generic;
using System.Text;

namespace Envivo.Fresnel.Introspection.IoC
{
    public interface IDomainObjectFactory
    {

        object Create(Type classType, params object[] args);

    }

}
