using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;
using System.Diagnostics;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public class DependencyAwareObject
    {
        private IFactory<PocoObject> _PocoFactory;

        public DependencyAwareObject(IFactory<PocoObject> pocoFactory)
        {
            _PocoFactory = pocoFactory;

            this.PocoObject = _PocoFactory.Create();
        }
    
        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public virtual Guid ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// This property should have an item created by the injected factory
        /// </summary>
        public virtual PocoObject PocoObject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

    }
}
