using Envivo.Fresnel.UiCore.Objects;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CreateCommandResponse : BaseCommandResponse
    {
        public ObjectVM NewObject { get; set; }
    }
}