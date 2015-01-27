using Envivo.Fresnel.UiCore.Model;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class InvokeMethodResponse : BaseCommandResponse
    {
        public ObjectVM ResultObject { get; set; }
    }
}