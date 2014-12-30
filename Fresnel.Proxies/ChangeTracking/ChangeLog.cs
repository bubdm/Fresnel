using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Proxies.ChangeTracking
{
    public class ChangeLog
    {
        public ChangeLog()
        {
            this.AllChanges = new List<BaseChange>();
            this.ObjectCreations = new List<ObjectCreation>();
            this.PropertyChanges = new List<PropertyChange>();
            this.CollectionAdditions = new List<CollectionAdd>();
            this.CollectionRemovals = new List<CollectionRemove>();
            this.MethodInvocations = new List<MethodInvocation>();
        }

        public IList<BaseChange> AllChanges { get; private set; }

        public IList<ObjectCreation> ObjectCreations { get; private set; }

        public IList<PropertyChange> PropertyChanges { get; private set; }

        public IList<CollectionAdd> CollectionAdditions { get; private set; }

        public IList<CollectionRemove> CollectionRemovals { get; private set; }

        public IList<MethodInvocation> MethodInvocations { get; private set; }

        internal void AddObjectCreation(ObjectObserver oObject)
        {
            var latestChange = new ObjectCreation()
            {
                Sequence = Environment.TickCount,
                Object = oObject
            };

            this.AllChanges.Add(latestChange);
            this.ObjectCreations.Add(latestChange);
        }

        internal void AddPropertyChange(BasePropertyObserver oProperty)
        {
            var latestChange = new PropertyChange()
            {
                Sequence = Environment.TickCount,
                Property = oProperty
            };

            this.AllChanges.Add(latestChange);
            this.PropertyChanges.Add(latestChange);
        }

        internal void AddCollectionAdd(CollectionObserver oCollection, ObjectObserver oAddedItem)
        {
            var latestChange = new CollectionAdd()
            {
                Sequence = Environment.TickCount,
                Collection = oCollection,
                Element = oAddedItem
            };

            this.AllChanges.Add(latestChange);
            this.CollectionAdditions.Add(latestChange);
        }

        internal void AddCollectionRemove(CollectionObserver oCollection, ObjectObserver oRemovedItem)
        {
            var latestChange = new CollectionRemove()
            {
                Sequence = Environment.TickCount,
                Collection = oCollection,
                Element = oRemovedItem
            };

            this.CollectionRemovals.Add(latestChange);
        }

        internal void AddMethodInvocations(ObjectObserver oObject, MethodObserver oMethod)
        {
            var latestChange = new MethodInvocation()
            {
                Sequence = Environment.TickCount,
                Method = oMethod
            };

            this.AllChanges.Add(latestChange);
            this.MethodInvocations.Add(latestChange);
        }

        //internal IEnumerable<BaseChange> GetChangesSince(long startSequence)
        //{
        //    var startIndex = 0;
        //    var isStartIndexFound = false;
        //    while (!isStartIndexFound)
        //    {
        //        isStartIndexFound = this.AllChanges[startIndex].Sequence >= startSequence;
        //        if (isStartIndexFound)
        //        {
        //            var rangeCount = this.AllChanges.Count - startIndex;
        //            var results = this.AllChanges.GetRange(startIndex, rangeCount);
        //            return results;
        //        }
        //        startIndex++;
        //    }

        //    return new BaseChange[] { };
        //}

    }
}
