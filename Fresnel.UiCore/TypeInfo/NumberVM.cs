using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class NumberVM : ITypeInfo
    {
        public string Name { get; set; }

        public InputControlTypes PreferredControl { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public int DecimalPlaces { get; set; }

        public string CurrencySymbol { get; set; }

    }
}
