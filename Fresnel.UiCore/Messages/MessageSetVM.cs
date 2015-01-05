using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Messages
{
    public class MessageSetVM
    {
        public MessageSetVM()
        {
            this.Infos = new List<MessageVM>();
            this.Warnings = new List<MessageVM>();
            this.Errors = new List<ErrorVM>();
        }

        internal MessageSetVM
            (
            IEnumerable<MessageVM> infos,
            IEnumerable<MessageVM> warnings,
            IEnumerable<ErrorVM> errors
            )
        {
            this.Infos = infos == null ? new List<MessageVM>() : infos.ToList();
            this.Warnings = warnings == null ? new List<MessageVM>() : warnings.ToList();
            this.Errors = errors == null ? new List<ErrorVM>() : errors.ToList();
        }

        public IList<MessageVM> Infos { get; set; }

        public IList<MessageVM> Warnings { get; set; }

        public IList<ErrorVM> Errors { get; set; }
    }
}
