using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class NullVM : ITypeInfo
    {
        public string Name { get; set; }

        public InputControlTypes PreferredControl { get; set; }
    }
}