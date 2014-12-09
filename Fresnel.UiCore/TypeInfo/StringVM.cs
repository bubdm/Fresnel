using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class StringVM : ITypeInfo
    {
        public string Name
        {
            get { return "string"; }
        }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public bool IsMultiLine { get; set; }

        public bool IsPassword { get; set; }

        public string EditMask { get; set; }

    }
}
