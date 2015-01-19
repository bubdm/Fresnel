using Envivo.Fresnel.UiCore.Model;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class InvokeMethodResponse : BaseCommandResponse
    {
        public ObjectVM ResultObject { get; set; }
    }
}