using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using System.Collections.Generic;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// A Category has a many-to-many relationship with Product.
    /// When a Product is added to a Category, a bi-directional relationship should be automatically setup.
    /// </summary>
    public class BiDirectionalExample : BaseEntity
    {
        private Collection<BiDirectionalExample> _Contents = new Collection<BiDirectionalExample>();

        /// <summary>
        ///
        /// </summary>
        public BiDirectionalExample()
        {
        }

        /// <summary>
        ///
        /// </summary>
        [CollectionProperty(Relationship = ManyRelationship.OwnsMany)]
        public virtual ICollection<BiDirectionalExample> Contents
        {
            get { return _Contents; }
        }

        /// <summary>
        /// This method will take precedence over the Contents Collection's Add() method
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddToContents(BiDirectionalExample item)
        {
            _Contents.Add(item);

            item.Contents.Add(this);
        }
    }
}