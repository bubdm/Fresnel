using Envivo.Fresnel.UiCore.Model.TypeInfo;
using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class PropertyVM : SettableMemberVM
    {
        public bool IsProperty { get { return true; } }
    }
}