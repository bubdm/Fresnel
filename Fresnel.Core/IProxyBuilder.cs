using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core
{
    public interface IProxyBuilder
    {
        T BuildFor<T>(T obj) where T : class;

    }
}
