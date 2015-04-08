using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Objects;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.TestTypes
{
    /// <summary>
    /// This object has dependencies automatically injected into it
    /// </summary>
    public class ObjectWithCtorInjection
    {
        public ObjectWithCtorInjection(IFactory<PocoObject> pocoFactory)
        {
            this.PocoObject = pocoFactory.Create();
            this.Name = "This name is provided by default";
        }

        public ObjectWithCtorInjection(IFactory<PocoObject> pocoFactory, string name)
        {
            this.PocoObject = pocoFactory.Create();
            this.Name = name;
        }

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This property should have an item created by the injected factory
        /// </summary>
        [FilterQuerySpecification(SpecificationType = typeof(PocoFilterQuerySpecification))]
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