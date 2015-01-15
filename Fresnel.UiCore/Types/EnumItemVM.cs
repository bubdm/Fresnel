using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Types
{
    public class EnumItemVM : BaseViewModel
    {
        public string EnumName { get; set; }

        public int Value { get; set; }

        public bool IsChecked { get; set; }
    }
}
