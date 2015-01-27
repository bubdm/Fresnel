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

        public string MethodName { get; set; }

        public IEnumerable<ValueVM> Parameters { get; set; }

        public bool IsAsync { get; set; }
    }
}