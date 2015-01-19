using System;

namespace Envivo.Fresnel.UiCore.Changes
{
    public class PropertyChangeVM
    {
        public Guid ObjectId { get; set; }

        public string PropertyName { get; set; }

        public object NonReferenceValue { get; set; }

        public Guid? ReferenceValueId { get; set; }

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