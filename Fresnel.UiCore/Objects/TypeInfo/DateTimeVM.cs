using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects.TypeInfo
{
    public class DateTimeVM : ITypeInfo
    {

        public bool IsTimeOnly { get; set; }

        public bool IsDateOnly { get; set; }

        public string CustomFormat { get; set; }

    }
}
