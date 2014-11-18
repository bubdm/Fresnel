using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Introspection.Configuration;
using System.Diagnostics;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// A Product has a many-to-many relationship with Category.
    /// When a Category is added to a Product, a bi-directional relationship should be automatically setup.
    /// A Product is abstract, so it cannot be created. Only sub-classes can be created.
    /// </summary>
    public abstract class Product
    {
        public Guid ID { get; internal set; }

        public long Version { get; internal set; }

        public Product()
        {
            var categories = new Collection<Category>();

            categories.Adding += new NotifyCollectionChangesEventHandler<Category>(categories_Adding);
            categories.Added += new NotifyCollectionChangesEventHandler<Category>(categories_Added);
            categories.Removing += new NotifyCollectionChangesEventHandler<Category>(categories_Removing);
            categories.Removed += new NotifyCollectionChangesEventHandler<Category>(categories_Removed);

            this.Categories = categories;
        }

        void categories_Adding(object sender, ICollectionChangeEventArgs<Category> e)
        {
            e.IsCancelled = (this.Categories.Contains(e.Item));
        }

        void categories_Added(object sender, ICollectionChangeEventArgs<Category> e)
        {
            if (this.Categories.Contains(e.Item) == false)
            {
                e.Item.Products.Add(this);
            }
        }

        void categories_Removing(object sender, ICollectionChangeEventArgs<Category> e)
        {
            if (this.Categories.Contains(e.Item) == false)
            {
                e.IsCancelled = true;
            }
        }

        void categories_Removed(object sender, ICollectionChangeEventArgs<Category> e)
        {
            e.Item.Products.Remove(this);
        }

        /// <summary>
        /// The Categories that this Product belongs to
        /// </summary>
        [CollectionProperty(CanModifyContents = false, 
            Relationship = ManyRelationship.HasMany)]
        public IList<Category> Categories { get; private set; }

        /// <summary>
        /// The name of this Product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description for this Product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// This is from the base class
        /// </summary>
        [Property(IsVisible = false)]
        public string HiddenProperty { get; set; }

        /// <summary>
        /// This has a custom view
        /// </summary>
        public Money Money { get; set; }

        /// <summary>
        /// This should render itself as a button with a Green icon
        /// </summary>
        /// <returns></returns>
        public bool TestMethod1()
        {
            return true;
        }

        /// <summary>
        /// This should render itself as a button with a Grey icon, which only turns green when the argument is provided
        /// </summary>
        /// <returns></returns>
        public bool TestMethod2(Category categoryParameter)
        {
            return true;
        }
    }
}
