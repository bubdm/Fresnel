using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.SampleModel.BasicTypes;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.SampleModel.Objects
{
    public class NonInterceptablePropertyObjects
    {
        private DetailObject _DetailObject = new DetailObject();
        
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
