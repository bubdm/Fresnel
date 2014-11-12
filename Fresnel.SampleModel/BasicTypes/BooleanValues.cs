using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;

namespace Envivo.Sample.Model.BasicTypes
{
    /// <summary>
    /// A set of Boolean properties
    /// </summary>
    public class BooleanValues
    {

        private bool _BoolValue = false;

        public Guid ID { get; internal set; }

        /// <summary>
        /// This is a normal Boolean
        /// </summary>
        public bool NormalBoolean { get; set; }

        /// <summary>
        /// This is a Boolean with custom titles
        /// </summary>
        [Boolean(TrueValue = "Clockwise", FalseValue = "Anti-clockwise")]
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
