using Envivo.Fresnel.Configuration;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    [TypeScriptInterface]
    public class StringVM : ITypeInfo
    {
        public string Name { get; set; }

        public UiControlType PreferredControl { get; set; }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public string EditMask { get; set; }
    }
}