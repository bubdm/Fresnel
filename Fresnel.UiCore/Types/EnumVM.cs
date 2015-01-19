using Envivo.Fresnel.Configuration;
using System.Collections.Generic;

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