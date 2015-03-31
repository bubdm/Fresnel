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
            this.Messages = new MessageVM[0];
        }

        public string UserName { get; set; }

        public DateTime LogonTime { get; set; }

        public MessageVM[] Messages { get; set; }
    }
}