namespace Envivo.Fresnel.Configuration
{
    public enum RelationshipType
    {
        /// <summary>
        /// Aka Aggregation. This object is related to the property's content(s). Deleting this object will not affect these contents.
        /// </summary>
        Has,

        /// <summary>
        /// Aka Composition. This object owns the properties content(s) . Deleting this object should delete the contents too.
        /// </summary>
        Owns,

        /// <summary>
        /// This object is owned by the property's content. This is only relevant for objects, not collections.
        /// </summary>
        OwnedBy
    }
}