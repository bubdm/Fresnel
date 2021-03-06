using Envivo.Fresnel.Configuration;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    [TypeScriptInterface]
    public class NullVM : ITypeInfo
    {
        public string Name { get; set; }

        public UiControlType PreferredControl { get; set; }
    }
}