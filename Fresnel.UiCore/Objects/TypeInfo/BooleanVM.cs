using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects.TypeInfo
{
    public class BooleanVM : ITypeInfo
    {
        public bool IsNullable { get; set; }

        public string TrueValue { get; set; }

        public string FalseValue { get; set; }
    }
}
