using T4TS;
namespace Envivo.Fresnel.UiCore.Model.Classes
{
    [TypeScriptInterface]
    public class ClassItem : BaseViewModel
    {
        public string Type { get; set; }

        public string FullTypeName { get; set; }

        public InteractionPoint Create { get; set; }

        public InteractionPoint Search { get; set; }

        public InteractionPoint[] RepositoryCommands { get; set; }

        public InteractionPoint[] StaticMethodCommands { get; set; }

        public InteractionPoint[] FactoryCommands { get; set; }

        public InteractionPoint[] ServiceCommands { get; set; }
    }
}