using Envivo.Fresnel.UiCore.TypeInfo;
using System;

namespace Envivo.Fresnel.UiCore.Model
{
    public class PropertyVM : BaseViewModel
    {
        public Guid ObjectID { get; set; }

        public int Index { get; set; }

        public string PropertyName { get; set; }

        public object Value { get; set; }

        public string ValueType { get; set; }

        public bool IsRequired { get; set; }

        public bool IsLoaded { get; set; }

        public bool IsObject { get; set; }

        public bool IsCollection { get; set; }

        public bool IsNonReference { get; set; }

        public ITypeInfo Info { get; set; }

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