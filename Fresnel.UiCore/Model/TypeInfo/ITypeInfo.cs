using Envivo.Fresnel.Configuration;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public interface ITypeInfo
    {
        UiControlType PreferredControl { get; set; }
    }
}