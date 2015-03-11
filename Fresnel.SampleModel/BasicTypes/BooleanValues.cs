using Envivo.Fresnel.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// A set of Boolean properties
    /// </summary>
    public class BooleanValues
    {
        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// This is a normal Boolean
        /// </summary>
        public bool NormalBoolean { get; set; }

        /// <summary>
        /// This is a Boolean with custom titles
        /// </summary>
        [DisplayFormat(DataFormatString="Clockwise|Anti-Clockwise")]
        public bool Orientation { get; set; }

        /// <summary>
        /// This is a read-only Boolean
        /// </summary>
        public bool ReadOnlyBool
        {
            get { return this.NormalBoolean; }
        }
    }
}