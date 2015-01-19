using System;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// A set of primitive Nullable properties.
    /// You can clear these values (as long as they have public Setters)
    /// </summary>
    public class NullableValues
    {
        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public virtual Guid ID { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also to 'Cut' this value using the clipboard commands.
        /// </summary>
        public virtual bool? NullableBool { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also 'Cut' this value using the clipboard commands.
        /// </summary>
        public virtual int? NullableInt { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also to 'Cut' this value using the clipboard commands.
        /// </summary>
        public virtual double? NullableDouble { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also 'Cut' this value using the clipboard commands.
        /// </summary>
        public virtual DateTime? NullableDateTime { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also 'Cut' this value using the clipboard commands.
        /// </summary>
        public virtual DateTime? NullableDateTime2 { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also 'Cut' this value using the clipboard commands.
        /// </summary>
        public virtual string NullableString { get; set; }
    }
}