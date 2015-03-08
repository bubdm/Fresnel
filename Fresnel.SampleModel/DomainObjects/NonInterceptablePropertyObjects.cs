using System;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    ///
    /// </summary>
    public class NonInterceptablePropertyObjects
    {
        private DetailObject _DetailObject = new DetailObject();

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// DetailObject MUST have non-virtual members i.e. DynamicProxy must not be able to intercept it
        /// </summary>
        public virtual DetailObject DetailObject
        {
            get { return _DetailObject; }
            set { _DetailObject = value; }
        }
    }
}