using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// A Category has a many-to-many relationship with Product.
    /// When a Product is added to a Category, a bi-directional relationship should be automatically setup.
    /// </summary>
    public class BiDirectionalExample : BaseEntity
    {

        public BiDirectionalExample()
        {
            this.Contents = new List<BiDirectionalExample>();
        }

        [Property(IsVisible = false)]
        public Guid? OwnerID { get; set; }
        
        /// <summary>
        /// Contains all of the other objects that relate back to this one
        /// </summary>
        [CollectionProperty(Relationship = ManyRelationship.OwnsMany)]
        public virtual ICollection<BiDirectionalExample> Contents { get; set; }

        /// <summary>
        /// This method should be used instead of Contents.Add() method
        /// </summary>
        /// <param name="item"></param>
        public void AddToContents(BiDirectionalExample item)
        {
            this.Contents.Add(item);

            item.Contents.Add(this);
        }
    }
}