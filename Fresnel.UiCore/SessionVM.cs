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
            this.InfoMessages = new List<MessageVM>();
            this.WarningMessages = new List<MessageVM>();
            this.ErrorMessages = new List<ErrorVM>();
        }


        public string UserName { get; set; }

        public DateTime LogonTime { get; set; }

        public IEnumerable<MessageVM> InfoMessages { get; set; }

        public IEnumerable<MessageVM> WarningMessages { get; set; }

        public IEnumerable<ErrorVM> ErrorMessages { get; set; }

    }
}
