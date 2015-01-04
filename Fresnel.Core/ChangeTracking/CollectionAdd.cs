using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    [DebuggerDisplay("ObjectID: {Collection.ID} / ElementID: {Element.ID}")]
    public class CollectionAdd : BaseChange
    {
        public CollectionObserver Collection { get; set; }

        public ObjectObserver Element { get; set; }
        
        public override void Dispose()
        {
            this.Collection = null;
            this.Element = null;
        }

    }
}
