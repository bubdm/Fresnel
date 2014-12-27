using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.Proxies.ChangeTracking
{
    [DebuggerDisplay("{Property.Template.Name}")]
    public class PropertyChange : BaseChange
    {
        public BasePropertyObserver Property { get; set; }
    
    }
}
