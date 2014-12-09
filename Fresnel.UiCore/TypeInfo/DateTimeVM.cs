using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class DateTimeVM : ITypeInfo
    {
        public string Name
        {
            get { return "datetime"; }
        }

        public bool IsTimeOnly { get; set; }

        public bool IsDateOnly { get; set; }

        public string CustomFormat { get; set; }

    }
}
