using Envivo.Fresnel.UiCore.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
