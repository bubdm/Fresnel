using Envivo.Fresnel.UiCore.Model;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class InvokeMethodRequest
    {
        public Guid ObjectID { get; set; }

        public string MethodName { get; set; }

        public IEnumerable<ValueVM> Parameters { get; set; }

    }
}