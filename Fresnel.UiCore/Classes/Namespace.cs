using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Classes
{
    public class Namespace : BaseViewModel
    {
        public string FullName { get; set; }

        public ClassItem[] Classes { get; set; }
    }
}
