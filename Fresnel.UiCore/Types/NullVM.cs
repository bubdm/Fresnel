using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.UiCore.Types
{
    public class NullVM : ITypeInfo
    {
        public string Name { get; set; }

        public InputControlTypes PreferredControl { get; set; }
    }
}