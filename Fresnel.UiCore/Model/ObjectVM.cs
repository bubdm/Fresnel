using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class ObjectVM : BaseViewModel
    {
        public Guid ID { get; set; }

        public string Type { get; set; }

        public IEnumerable<SettableMemberVM> Properties { get; set; }

        public IEnumerable<MethodVM> Methods { get; set; }
    }
}