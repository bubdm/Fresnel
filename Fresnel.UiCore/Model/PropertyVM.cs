using Envivo.Fresnel.UiCore.Model.TypeInfo;
using System;
using T4TS;
using System.ComponentModel;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class PropertyVM : SettableMemberVM
    {
        [DefaultValue(false)]
        public bool IsLoaded { get; set; }

        public bool IsProperty { get { return true; } }
    }
}