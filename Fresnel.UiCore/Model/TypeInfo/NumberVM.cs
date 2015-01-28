using Envivo.Fresnel.Configuration;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    [TypeScriptInterface]
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