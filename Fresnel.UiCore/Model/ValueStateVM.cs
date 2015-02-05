using Envivo.Fresnel.UiCore.Model.TypeInfo;
using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class ValueStateVM 
    {

        public object Value { get; set; }

        public Guid? ReferenceValueID { get; set; }

        public string FriendlyValue { get; set; }

        public string ValueType { get; set; }

        /// <summary>
        /// State for navigation interaction
        /// </summary>
        public InteractionPoint Get { get; set; }

        /// <summary>
        /// Interaction state for setting the property to an object
        /// </summary>
        public InteractionPoint Set { get; set; }

        /// <summary>
        /// Interaction state for creating new objects within the property
        /// </summary>
        public InteractionPoint Create { get; set; }

        /// <summary>
        /// Interaction state for clearing the property
        /// </summary>
        public InteractionPoint Clear { get; set; }

        /// <summary>
        /// Interaction state for adding to a collection property
        /// </summary>
        public InteractionPoint Add { get; set; }
    }
}