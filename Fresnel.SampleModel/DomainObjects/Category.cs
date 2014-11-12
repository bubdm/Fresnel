using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;using Envivo.TrueView.Domain;
using Envivo.TrueView.Domain.Attributes;
using System.Diagnostics;

namespace Envivo.Sample.Model.Objects
{
    /// <summary>
    /// A Category has a many-to-many relationship with Product.
    /// When a Product is added to a Category, a bi-directional relationship should be automatically setup.
    /// </summary>
    public class Category : AggregateRootBase
    {
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

        public IAudit Audit { get; protected set; }

        private Money _Money;
        public Money Money
        {
            get { return _Money; }
            set { _Money = value; }
        }


        /// <summary>
        /// The Products that belong to this Category
        /// </summary>
        public IList<Product> Products { get; private set; }

        /// <summary>
        /// The name of this category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description for this category
        /// </summary>
        public string Description { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
