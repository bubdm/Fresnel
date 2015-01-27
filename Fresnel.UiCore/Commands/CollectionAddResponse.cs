using Envivo.Fresnel.UiCore.Model;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CollectionAddResponse : BaseCommandResponse
    {
        public ObjectVM AddedItem { get; set; }
    }
}