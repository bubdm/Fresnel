using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects
{
    public class CollectionVM : ObjectVM
    {

        public IEnumerable<ObjectVM> Items { get; set; }

    }
}
