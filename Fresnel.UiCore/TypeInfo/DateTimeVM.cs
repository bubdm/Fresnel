using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class DateTimeVM : ITypeInfo
    {
        public string Name { get; set; }

        public InputControlTypes PreferredControl { get; set; }

        public string CustomFormat { get; set; }
    }
}