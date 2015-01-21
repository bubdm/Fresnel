using Envivo.Fresnel.UiCore.Model;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CollectionAddResponse : BaseCommandResponse
    {
        public ObjectVM AddedItem { get; set; }
    }
}