using Envivo.Fresnel.Configuration;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    [TypeScriptInterface]
    public class EnumVM : ITypeInfo
    {
        public string Name { get; set; }

        public bool IsBitwiseEnum { get; set; }

        public IEnumerable<EnumItemVM> Items { get; set; }

        public InputControlTypes PreferredControl { get; set; }
    }
}