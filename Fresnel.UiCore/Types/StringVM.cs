using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.UiCore.Types
{
    public class StringVM : ITypeInfo
    {
        public string Name { get; set; }

        public InputControlTypes PreferredControl { get; set; }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public string EditMask { get; set; }
    }
}