using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class InvokeMethodRequest
    {
        public Guid ObjectID { get; set; }

        public string MethodName { get; set; }
    }
}