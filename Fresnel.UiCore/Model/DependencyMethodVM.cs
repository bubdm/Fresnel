using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class DependencyMethodVM : MethodVM
    {
        public string ClassType { get; set; }

    }
}