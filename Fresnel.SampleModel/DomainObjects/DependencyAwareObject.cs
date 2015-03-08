using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// This object has dependencies automatically injected into it
    /// </summary>
    public class DependencyAwareObject
    {
        public DependencyAwareObject(IFactory<PocoObject> pocoFactory)
        {
            this.PocoObject = pocoFactory.Create();
            this.Name = "This name is provided by default";
        }

        public DependencyAwareObject(IFactory<PocoObject> pocoFactory, string name)
        {
            this.PocoObject = pocoFactory.Create();
            this.Name = name;
        }

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

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