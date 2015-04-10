using System;
using System.Collections.Generic;
using System.ComponentModel;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class MethodVM : BaseViewModel
    {
        public Guid? ObjectID { get; set; }

        public int Index { get; set; }

        public string InternalName { get; set; }

        public ParameterVM[] Parameters { get; set; }

        public ParameterVM[] ParametersSetByUser { get; set; }

        [DefaultValue(false)]
        public bool IsAsync { get; set; }

        public override string ToString()
        {
            return string.Concat(this.Index, "/", this.InternalName);
        }
    }
}