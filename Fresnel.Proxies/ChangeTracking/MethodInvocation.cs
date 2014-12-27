using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.Proxies.ChangeTracking
{
    [DebuggerDisplay("ObjectID: {Method.OuterObserver.ID}.{Method.Template.Name}")]
    public class MethodInvocation : BaseChange
    {
        public MethodObserver Method { get; set; }
    }
}
