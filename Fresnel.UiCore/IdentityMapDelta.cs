using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore
{
    public class IdentityMapDelta
    {
        public Dictionary<Guid, object> NewItems { get; set; }

        public Dictionary<Guid, object> ModifiedItems { get; set; }

        public Dictionary<Guid, object> RemovedItems { get; set; }
    }
}
