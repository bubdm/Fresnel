using Envivo.Fresnel.UiCore.TypeInfo;
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

        public string PropertyName { get; set; }

        public object Value { get; set; }

        public bool IsRequired { get; set; }

        public bool CanRead { get; set; }

        public bool CanWrite { get; set; }

        public bool IsLoaded { get; set; }

        public bool IsExpandable { get; set; }

        public ITypeInfo Info { get; set; }

    }
}
