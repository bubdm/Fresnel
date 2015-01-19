using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.UiCore.Types
{
    public class DateTimeVM : ITypeInfo
    {
        public string Name { get; set; }

        public InputControlTypes PreferredControl { get; set; }

        public string CustomFormat { get; set; }
    }
}