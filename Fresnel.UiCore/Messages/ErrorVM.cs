using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Messages
{
    public class ErrorVM : MessageVM
    {
        public ErrorVM()
        {
        }

        public ErrorVM(Exception ex)
        {
            this.Text = ex.Message;
            this.Detail = ex.StackTrace;
        }

        public string Detail { get; set; }

        public bool IsAcknowledged { get; set; }

        public DateTime AcknowledgedAt { get; set; }

    }
}
