﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.Proxies.ChangeTracking
{
    public class CollectionRemove : BaseChange
    {
        public Guid CollectionID { get; set; }

        public Guid ElementID { get; set; }
    }
}