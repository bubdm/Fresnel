using Envivo.Fresnel.UiCore.Model;

using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore.Changes
{
    public class Modifications
    {
        public IEnumerable<ObjectVM> NewObjects { get; set; }

        public IEnumerable<PropertyChangeVM> PropertyChanges { get; set; }

        public IEnumerable<CollectionElementVM> CollectionAdditions { get; set; }

        public IEnumerable<CollectionElementVM> CollectionRemovals { get; set; }
    }
}