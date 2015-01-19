using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.UiCore.Types
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