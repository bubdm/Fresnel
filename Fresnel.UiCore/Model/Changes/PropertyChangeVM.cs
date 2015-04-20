using Envivo.Fresnel.UiCore.Model;
using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.Changes
{
    [TypeScriptInterface]
    public class PropertyChangeVM
    {
        public Guid ObjectID { get; set; }

        public string PropertyName { get; set; }

        public ValueStateVM State { get; set; }

    }
}