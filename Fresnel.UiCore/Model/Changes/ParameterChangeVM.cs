using Envivo.Fresnel.UiCore.Model;
using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.Changes
{
    [TypeScriptInterface]
    public class ParameterChangeVM
    {
        public Guid ObjectId { get; set; }

        public string MethodName { get; set; }

        public string ParameterName { get; set; }

        public ValueStateVM State { get; set; }

    }
}