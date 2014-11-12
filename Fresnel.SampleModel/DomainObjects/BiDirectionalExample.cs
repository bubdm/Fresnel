using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.TrueView.Domain.Attributes;
using System.Diagnostics;
using Envivo.DomainTypes;

namespace Envivo.Sample.Model.Objects
{
    /// <summary>
    /// A Category has a many-to-many relationship with Product.
    /// When a Product is added to a Category, a bi-directional relationship should be automatically setup.
    /// </summary>
    public class BiDirectionalExample : EntityBase
    {
        private Collection<BiDirectionalExample> _Contents = new Collection<BiDirectionalExample>();

        public BiDirectionalExample()
        {
        }

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
