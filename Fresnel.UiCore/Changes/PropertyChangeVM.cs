using Envivo.Fresnel.UiCore.Model;
using System;

namespace Envivo.Fresnel.UiCore.Changes
{
    public class PropertyChangeVM
    {
        public Guid ObjectId { get; set; }

        public string PropertyName { get; set; }

        public object NonReferenceValue { get; set; }

        public Guid? ReferenceValueId { get; set; }

        public PropertyStateVM State { get; set; }

    }
}