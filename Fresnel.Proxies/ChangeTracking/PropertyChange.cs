using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.Proxies.ChangeTracking
{
    public class PropertyChange : BaseChange
    {
        public Guid ObjectID { get; set; }

        public string PropertyName { get; set; }
    }
}
