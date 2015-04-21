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

        public DependencyMethodVM[] FactoryCommands { get; set; }

        public DependencyMethodVM[] QueryCommands { get; set; }

        public DependencyMethodVM[] ServiceCommands { get; set; }
    }
}