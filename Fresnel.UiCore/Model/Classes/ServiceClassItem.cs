using T4TS;
namespace Envivo.Fresnel.UiCore.Model.Classes
{
    [TypeScriptInterface]
    public class ServiceClassItem : ClassItem
    {
        public ObjectVM AssociatedService { get; set; }
    }
}