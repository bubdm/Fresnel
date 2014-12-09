using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects
{
    public class ObjectVM : BaseViewModel
    {
        public Guid ID { get; set; }

        public IEnumerable<PropertyVM> Properties { get; set; }

        public bool IsMaximised { get; set; }
    }
}
