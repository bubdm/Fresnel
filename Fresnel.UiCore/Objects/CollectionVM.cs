using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects
{
    public class CollectionVM : ObjectVM
    {

        public bool IsCollection { get { return true; } }

        public string ElementType { get; set; }

        public IEnumerable<PropertyVM> ElementProperties { get; set; }

        public IEnumerable<ObjectVM> Items { get; set; }

    }
}
