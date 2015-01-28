using T4TS;
namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public abstract class BaseViewModel
    {
        public bool IsVisible { get; set; }

        public bool IsEnabled { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Error { get; set; }

        public string Tooltip { get; set; }
    }
}