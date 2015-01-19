using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.UiCore.Types
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