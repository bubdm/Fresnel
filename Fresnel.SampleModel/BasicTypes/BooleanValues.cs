using Envivo.Fresnel.Configuration;
using System;

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
        public virtual Guid ID { get; set; }

        /// <summary>
        /// This is a normal Boolean
        /// </summary>
        public virtual bool NormalBoolean { get; set; }

        /// <summary>
        /// This is a Boolean with custom titles
        /// </summary>
        [Boolean(TrueValue = "Clockwise", FalseValue = "Anti-clockwise")]
        public virtual bool Orientation { get; set; }

        /// <summary>
        /// This is a read-only Boolean
        /// </summary>
        public virtual bool ReadOnlyBool
        {
            get { return this.NormalBoolean; }
        }
    }
}