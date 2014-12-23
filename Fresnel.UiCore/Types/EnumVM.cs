using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Types
{
    public class EnumVM : ITypeInfo
    {
        public string Name { get; set; }

        public bool IsBitwiseEnum { get; set; }

        public IEnumerable<EnumItemVM> Items { get; set; }

        public InputControlTypes PreferredControl { get; set; }

    }
}
