using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core.Proxies
{
    public interface IProxyBuilder
    {
        IFresnelProxy BuildFor(object obj, BaseObjectObserver observer);

    }
}
