using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.Proxies.ChangeTracking
{
    [DebuggerDisplay("ObjectID: {Collection.ID} / ElementID: {Element.ID}")]
    public class CollectionRemove : BaseChange
    {
        public CollectionObserver Collection { get; set; }

        public ObjectObserver Element { get; set; }
    }
}
