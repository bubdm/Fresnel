using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class InvokeMethodRequest
    {
        public Guid ObjectID { get; set; }

        public string MethodName { get; set; }

    }
}
