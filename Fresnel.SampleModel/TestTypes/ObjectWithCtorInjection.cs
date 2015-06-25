using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.TestTypes.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.TestTypes
{
    /// <summary>
    /// This object has dependencies automatically injected into it
    /// </summary>
    public class ObjectWithCtorInjection
    {
        private IList<AbstractEntity> _OwnedItems = new List<AbstractEntity>();
        private IList<ConcreteEntityC> _AssociatedItems = new List<ConcreteEntityC>();

        public ObjectWithCtorInjection(IFactory<Product> productFactory)
        {
            this.Product = productFactory.Create();
            this.Name = "This name is provided by default";
        }

        public ObjectWithCtorInjection(IFactory<Product> productFactory, string name)
        {
            this.Product = productFactory.Create();
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
        [FilterQuerySpecification(SpecificationType = typeof(ProductFilterQuerySpecification))]
        public virtual Product Product { get; set; }

        /// <summary>
        /// New items can be created within this property. Existing items cannot be added to this list.
        /// </summary>
        [Relationship(Type = RelationshipType.Owns)]
        public IList<AbstractEntity> OwnedItems
        {
            get { return _OwnedItems; }
            set { _OwnedItems = value; }
        }

        /// <summary>
        /// Existing items can be added to this property.  New items cannot be added to this list.
        /// </summary>
        [Relationship(Type = RelationshipType.Has)]
        public IList<ConcreteEntityC> AssociatedItems
        {
            get { return _AssociatedItems; }
            set { _AssociatedItems = value; }
        }

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