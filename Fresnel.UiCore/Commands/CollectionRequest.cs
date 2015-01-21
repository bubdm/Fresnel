using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CollectionRequest
    {
        public Guid CollectionID { get; set; }

        public Guid ElementID { get; set; }

        public string ElementTypeName { get; set; }
    }
}