using Envivo.Fresnel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Proxies
{
    public class ProxyBuilder : IProxyBuilder
    {

        public T BuildFor<T>(T obj)
            where T : class
        {
            // TODO: Implement!!
            return obj;
        }
    }
}
