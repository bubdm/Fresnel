using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.TestTypes
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
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also to 'Cut' this value using the clipboard commands.
        /// </summary>
        public bool? NullableBool { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also 'Cut' this value using the clipboard commands.
        /// </summary>
        public int? NullableInt { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also to 'Cut' this value using the clipboard commands.
        /// </summary>
        public double? NullableDouble { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also 'Cut' this value using the clipboard commands.
        /// </summary>
        public DateTime? NullableDateTime { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also 'Cut' this value using the clipboard commands.
        /// </summary>
        public DateTime? NullableDateTime2 { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// You can also 'Cut' this value using the clipboard commands.
        /// </summary>
        public string NullableString { get; set; }
    }
}