using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class BooleanVM : ITypeInfo
    {
        public string Name { get; set; }

        public InputControlTypes PreferredControl { get; set; }

        public bool IsNullable { get; set; }

        public string TrueValue { get; set; }

        public string FalseValue { get; set; }

    }
}
