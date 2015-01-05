﻿using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    [DebuggerDisplay("ObjectID: {Collection.ID} / ElementID: {Element.ID}")]
    public class CollectionRemove : BaseChange
    {
        public CollectionObserver Collection { get; set; }

        public object Element { get; set; }

        public override void Dispose()
        {
            this.Collection = null;
            this.Element = null;
        }

    }
}