using Envivo.Fresnel.Configuration;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    [TypeScriptInterface]
    public class ReferenceTypeVM : ITypeInfo
    {
        public string FullTypeName { get; set; }

        public UiControlType PreferredControl
        {
            get { return UiControlType.None; }
            set { }
        }
    }
}