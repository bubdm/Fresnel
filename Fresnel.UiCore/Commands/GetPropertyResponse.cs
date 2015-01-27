using Envivo.Fresnel.UiCore.Model;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class GetPropertyResponse : BaseCommandResponse
    {
        public ObjectVM ReturnValue { get; set; }
    }
}