using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class SetParameterRequest
    {
        public Guid ObjectID { get; set; }

        public string MethodName { get; set; }

        public string ParameterName { get; set; }

        public object NonReferenceValue { get; set; }

        public Guid ReferenceValueId { get; set; }
    }
}