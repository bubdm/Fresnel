using System;
using System.Collections.Generic;
using System.ComponentModel;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class ObjectMethodVM : MethodVM
    {
        public Guid? ObjectID { get; set; }

    }
}