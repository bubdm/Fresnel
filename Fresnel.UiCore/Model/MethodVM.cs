using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class MethodVM : BaseViewModel
    {
        public Guid? ObjectID { get; set; }

        public int Index { get; set; }

        public string InternalName { get; set; }

        public IEnumerable<SettableMemberVM> Parameters { get; set; }

        public IEnumerable<SettableMemberVM> ParametersSetByUser { get; set; }

        public bool IsAsync { get; set; }
    }
}