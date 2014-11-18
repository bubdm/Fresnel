using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.SampleModel.BasicTypes;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Introspection.Configuration;

namespace Envivo.Fresnel.SampleModel.Objects
{
    public class NonInterceptablePropertyObjects
    {
        private DetailObject _DetailObject = new DetailObject();

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var that = obj as NonInterceptablePropertyObjects;
            if (that == null)
                return false;

            return this.ID.Equals(that.ID);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public virtual Guid ID { get; set; }

        public virtual long Version { get; set; }

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
