using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.Proxies.ChangeTracking
{
    public class MethodInvocation : BaseChange
    {
        public Guid ObjectID { get; set; }

        public string MethodName { get; set; }
    }
}
