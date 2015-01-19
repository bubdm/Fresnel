using Envivo.Fresnel.UiCore.Model;


namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetPropertyResponse : BaseCommandResponse
    {
        public ObjectVM ReturnValue { get; set; }
    }
}