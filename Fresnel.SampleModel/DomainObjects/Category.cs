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
    /// A Category has a many-to-many relationship with Product.
    /// When a Product is added to a Category, a bi-directional relationship should be automatically setup.
    /// </summary>
    public class Category : BaseAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public Category()
        {
            var products = new Collection<Product>();
            products.Adding += new NotifyCollectionChangesEventHandler<Product>(Products_Adding);
            products.Added += new NotifyCollectionChangesEventHandler<Product>(products_Added);
            products.Removing += new NotifyCollectionChangesEventHandler<Product>(products_Removing);
            products.Removed += new NotifyCollectionChangesEventHandler<Product>(products_Removed);

            this.Products = products;
        }

        void Products_Adding(object sender, ICollectionChangeEventArgs<Product> e)
        {
            e.IsCancelled = (this.Products.Contains(e.Item));
        }

        void products_Removing(object sender, ICollectionChangeEventArgs<Product> e)
        {
            if (this.Products.Contains(e.Item) == false)
            {
                e.IsCancelled = true;
            }
        }

        void products_Added(object sender, ICollectionChangeEventArgs<Product> e)
        {
            if (e.Item.Categories.Contains(this) == false)
            {
                e.Item.Categories.Add(this);
            }
        }

        void products_Removed(object sender, ICollectionChangeEventArgs<Product> e)
        {
            e.Item.Categories.Remove(this);
        }

        // Not sure why this property is declared here, as it hides the one in the base class:
        //        public IAudit Audit { get; protected set; }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual Money Money { get; set; }


        /// <summary>
        /// The Products that belong to this Category
        /// </summary>
        public virtual IList<Product> Products { get; private set; }

        /// <summary>
        /// The name of this category
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// The description for this category
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
