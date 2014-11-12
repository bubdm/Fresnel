using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;
using System.Diagnostics;

namespace Envivo.Sample.Model.Objects
{
    //[Permissions(User = "Vij", AllowedOperations = Allow.Read)]
    //[Permissions(Role = "User", AllowedOperations = Allow.Read)]
    //[Permissions(Role = "Administrator", AllowedOperations = Allow.All)]
    public class MasterObject
    {
        public Guid ID { get; internal set; }

        public MasterObject()
        {
            var children = new Collection<DetailObject>();
            children.Adding +=new NotifyCollectionChangesEventHandler<DetailObject>(children_Adding);
            children.Added +=new NotifyCollectionChangesEventHandler<DetailObject>(children_Added);
            children.Removing +=new NotifyCollectionChangesEventHandler<DetailObject>(children_Removing);
            children.Removed +=new NotifyCollectionChangesEventHandler<DetailObject>(children_Removed);

            this.Children = children;
        }

        void children_Adding(object sender, ICollectionChangeEventArgs<DetailObject> e)
        {
            e.IsCancelled = (this.Children.Contains(e.Item));
        }

        void children_Added(object sender, ICollectionChangeEventArgs<DetailObject> e)
        {
            if (e.Item.Parent != this)
            {
                e.Item.Parent = this;
            }
        }

        void children_Removing(object sender, ICollectionChangeEventArgs<DetailObject> e)
        {
            if (this.Children.Contains(e.Item) == false)
            {
                e.IsCancelled = true;
            }
        }

        void children_Removed(object sender, ICollectionChangeEventArgs<DetailObject> e)
        {
            if (e.Item.Parent != null)
            {
                e.Item.Parent = null;
            }
        }

        public string Name { get; set; }

        public Collection<DetailObject> Children { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
