using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class SessionVM
    {
        public SessionVM()
        {
            this.Messages = new List<MessageVM>();
        }

        public string UserName { get; set; }

        public DateTime LogonTime { get; set; }

        public IEnumerable<MessageVM> Messages { get; set; }
    }
}