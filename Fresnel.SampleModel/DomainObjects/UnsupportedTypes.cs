using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// Exposes known classes that are not fully supported
    /// </summary>
    public class UnsupportedTypes
    {
        private Dictionary<string, PocoObject> _Map = new Dictionary<string, PocoObject>();

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public virtual Guid ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public virtual Dictionary<string, PocoObject> Map
        {
            get { return _Map; }
            protected set { _Map = value; }
        }
    }
}