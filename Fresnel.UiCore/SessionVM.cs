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
            this.Messages = new MessageSetVM(new MessageVM[0],
                                             new MessageVM[0],
                                             new ErrorVM[0]);
        }

        public string UserName { get; set; }

        public DateTime LogonTime { get; set; }

        public MessageSetVM Messages { get; set; }

    }
}
