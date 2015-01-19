using Envivo.Fresnel.UiCore.Objects;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetPropertyResponse : BaseCommandResponse
    {
        public ObjectVM ReturnValue { get; set; }
    }
}