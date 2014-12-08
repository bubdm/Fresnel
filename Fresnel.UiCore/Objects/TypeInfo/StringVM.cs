using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects.TypeInfo
{
    public class StringVM : ITypeInfo
    {
        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public bool IsMultiLine { get; set; }

        public bool IsPassword { get; set; }

        public string EditMask { get; set; }

    }
}
