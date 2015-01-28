using Envivo.Fresnel.UiCore.Model;
using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class InvokeMethodRequest
    {
        public Guid ObjectID { get; set; }

        public string MethodName { get; set; }

        public IEnumerable<SettableMemberVM> Parameters { get; set; }

    }
}