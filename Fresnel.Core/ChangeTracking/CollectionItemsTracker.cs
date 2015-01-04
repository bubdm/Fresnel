using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    /// <summary>
    /// Tracks changes made to a Collection
    /// </summary>
    internal class CollectionItemsTracker : IDisposable
    {
        private CollectionObserver _oCollection = null;
        private List<CollectionAdd> _Additions = new List<CollectionAdd>();
        private List<CollectionRemove> _Removals = new List<CollectionRemove>();

        internal CollectionItemsTracker(CollectionObserver oCollection)
        {
            _oCollection = oCollection;

            this.PreviousItems = new List<object>();
            this.LatestItems = new List<object>();
        }

        internal IEnumerable<CollectionAdd> Additions
        {
            get { return _Additions; }
        }

        internal IEnumerable<CollectionRemove> Removals
        {
            get { return _Removals; }
        }

        internal IEnumerable<object> PreviousItems { get; private set; }

        internal IEnumerable<object> LatestItems { get; private set; }

        internal void DetermineInitialState()
        {
            //var items = _oCollection.GetContents();
            //this.PreviousItems = this.LatestItems = items.Cast<object>();
        }

        internal IAssertion DetectChanges()
        {
            var veryLatestItems = _oCollection.GetContents().Cast<object>();

            var addedItems = veryLatestItems.Except(this.LatestItems).ToArray();
            var removedItems = this.LatestItems.Except(veryLatestItems).ToArray();

            if (!addedItems.Any() && !removedItems.Any())
            {
                return Assertion.Fail("The collection has not changed");
            }

            var addedChanges = addedItems.Select(a => new CollectionAdd()
                {
                    Sequence = SequentialIdGenerator.Next,
                    Collection = _oCollection,
                    Element = a,
                });
            _Additions.AddRange(addedChanges);

            var removedChanges = removedItems.Select(a => new CollectionRemove()
            {
                Sequence = SequentialIdGenerator.Next,
                Collection = _oCollection,
                Element = a,
            });
            _Removals.AddRange(removedChanges);

            return Assertion.Pass();
        }

        public void Reset()
        {
            _Additions.Clear();
            _Removals.Clear();

            var items = _oCollection.GetContents();
            this.PreviousItems = this.LatestItems = items.Cast<object>();
        }

        public void Dispose()
        {
            _Additions.ClearSafely();
            _Additions = null;

            _Removals.ClearSafely();
            _Removals = null;

            _oCollection = null;
        }

    }
}
