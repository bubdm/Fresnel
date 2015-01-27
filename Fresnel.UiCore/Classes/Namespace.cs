using T4TS;
namespace Envivo.Fresnel.UiCore.Classes
{
    [TypeScriptInterface]
    public class Namespace : BaseViewModel
    {
        public string FullName { get; set; }

        public ClassItem[] Classes { get; set; }
    }
}