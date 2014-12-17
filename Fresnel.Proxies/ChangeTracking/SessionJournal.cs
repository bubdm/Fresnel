using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Proxies.ChangeTracking
{
    public class SessionJournal
    {
        public SessionJournal()
        {
            this.AllChanges = new List<BaseChange>();
            this.ObjectCreations = new List<ObjectCreation>();
            this.PropertyChanges = new List<PropertyChange>();
            this.CollectionAdditions = new List<CollectionAdd>();
            this.CollectionRemovals = new List<CollectionRemove>();
            this.MethodInvocations = new List<MethodInvocation>();
        }

        internal List<BaseChange> AllChanges { get; private set; }

        internal List<ObjectCreation> ObjectCreations { get; private set; }

        internal List<PropertyChange> PropertyChanges { get; private set; }

        internal List<CollectionAdd> CollectionAdditions { get; private set; }

        internal List<CollectionRemove> CollectionRemovals { get; private set; }

        internal List<MethodInvocation> MethodInvocations { get; private set; }

        internal void AddObjectCreation(ObjectObserver oObject)
        {
            var latestChange = new ObjectCreation()
            {
                Sequence = Environment.TickCount,
                ObjectID = oObject.ID,
            };

            this.AllChanges.Add(latestChange);
            this.ObjectCreations.Add(latestChange);
        }

        internal void AddPropertyChange(BasePropertyObserver oProperty)
        {
            var latestChange = new PropertyChange()
            {
                Sequence = Environment.TickCount,
                ObjectID = oProperty.OuterObject.ID,
                PropertyName = oProperty.Template.Name
            };

            this.AllChanges.Add(latestChange);
            this.PropertyChanges.Add(latestChange);
        }

        internal void AddCollectionAdd(CollectionObserver oCollection, ObjectObserver oAddedItem)
        {
            var latestChange = new CollectionAdd()
            {
                Sequence = Environment.TickCount,
                CollectionID = oCollection.ID,
                ElementID = oAddedItem.ID
            };

            this.AllChanges.Add(latestChange);
            this.CollectionAdditions.Add(latestChange);
        }

        internal void AddCollectionRemove(CollectionObserver oCollection, ObjectObserver oRemovedItem)
        {
            var latestChange = new CollectionRemove()
            {
                Sequence = Environment.TickCount,
                CollectionID = oCollection.ID,
                ElementID = oRemovedItem.ID
            };

            this.CollectionRemovals.Add(latestChange);
        }

        internal void AddMethodInvocations(ObjectObserver oObject, string methodName)
        {
            var latestChange = new MethodInvocation()
            {
                Sequence = Environment.TickCount,
                ObjectID = oObject.ID,
                MethodName = methodName,
            };

            this.AllChanges.Add(latestChange);
            this.MethodInvocations.Add(latestChange);
        }

        internal IEnumerable<BaseChange> GetChangesSince(long startSequence)
        {
            var startIndex = 0;
            var isStartIndexFound = false;
            while (!isStartIndexFound)
            {
                isStartIndexFound = this.AllChanges[startIndex].Sequence >= startSequence;
                if (isStartIndexFound)
                {
                    var rangeCount = this.AllChanges.Count - startIndex;
                    var results = this.AllChanges.GetRange(startIndex, rangeCount);
                    return results;
                }
                startIndex++;
            }

            return new BaseChange[] { };
        }

    }
}
