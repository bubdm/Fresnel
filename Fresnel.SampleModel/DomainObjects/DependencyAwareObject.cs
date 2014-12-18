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
    /// This object has dependencies automatically injected into it
    /// </summary>
    public class DependencyAwareObject
    {
        public DependencyAwareObject(IFactory<PocoObject> pocoFactory, string name)
        {
            this.PocoObject = pocoFactory.Create();
            this.Name = name;
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
