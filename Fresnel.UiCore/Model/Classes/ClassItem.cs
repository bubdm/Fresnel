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

        public MethodVM[] FactoryMethods { get; set; }

        public MethodVM[] QueryMethods { get; set; }

        public MethodVM[] ServiceMethods { get; set; }
    }
}