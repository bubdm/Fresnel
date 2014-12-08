using Envivo.Fresnel.UiCore.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects
{
    public class PropertyVM : BaseViewModel
    {
        public Guid ObjectID { get; set; }

        public bool IsLoaded { get; set; }

        public object NonRefValue { get; set; }

        public EditorType EditorType { get; set; }

        public bool IsExpandable { get; set; }
    }
}
