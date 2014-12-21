using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class EnumItemVM : BaseViewModel
    {
        public string EnumName { get; set; }

        public object Value { get; set; }

        public bool IsChecked { get; set; }
    }
}
