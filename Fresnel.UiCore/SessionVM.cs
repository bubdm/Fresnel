using Envivo.Fresnel.UiCore.Messages;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore
{
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
