using T4TS;
namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class InteractionPoint : BaseViewModel
    {
        public string CommandUri { get; set; }

        public string CommandArg { get; set; }
    }
}