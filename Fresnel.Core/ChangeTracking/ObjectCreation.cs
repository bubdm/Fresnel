using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    public class ObjectCreation : BaseChange
    {

        public ObjectObserver Object { get; set; }

        public override void Dispose()
        {
            this.Object = null;
        }

    }
}
