using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.Proxies.ChangeTracking
{
    public class ObjectCreation : BaseChange
    {

        public ObjectObserver Object { get; set; }

    }
}
