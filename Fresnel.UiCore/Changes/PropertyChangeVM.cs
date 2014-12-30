﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Envivo.Fresnel.UiCore.Changes
{
    public class PropertyChangeVM
    {
        public Guid ObjectId { get; set; }

        public string PropertyName { get; set; }

        public object NonReferenceValue { get; set; }

        public Guid ReferenceValueId { get; set; }
    }
}