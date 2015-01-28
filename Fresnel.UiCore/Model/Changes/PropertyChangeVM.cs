using Envivo.Fresnel.UiCore.Model;
using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.Changes
{
    [TypeScriptInterface]
    public class PropertyChangeVM
    {
        public Guid ObjectId { get; set; }

        public string PropertyName { get; set; }

        public object NonReferenceValue { get; set; }

        public Guid? ReferenceValueId { get; set; }

        public ValueStateVM State { get; set; }

    }
}