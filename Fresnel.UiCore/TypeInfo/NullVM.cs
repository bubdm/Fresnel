using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class NullVM : ITypeInfo
    {
        public string Name
        {
            get { return "null"; }
        }

    }
}
