using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Commands
{
    public class GetCollectionItemsCommand
    {
        private ObserverCache _ObserverCache;

        public GetCollectionItemsCommand(ObserverCache observerCache)
        {
            _ObserverCache = observerCache;
        }

        public IEnumerable<ObjectObserver> Invoke(CollectionObserver oCollection)
        {
            var results = new List<ObjectObserver>();

            var items = oCollection.GetItems();

            foreach (var item in items)
            {
                if (item == null)
                    continue;

                if (item.GetType().IsNonReference())
                    continue;

                var oItem = (ObjectObserver)_ObserverCache.GetObserver(item);
                results.Add(oItem);
            }

            return results;

            //var currentContents = new Dictionary<Guid, ObjectObserver>();

            //var enumerator = this.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    if (enumerator.Current == null)
            //        continue;

            //    if (enumerator.Current.GetType().IsNonReference())
            //        continue;

            //    var oItem = (ObjectObserver)_ObserverCache.GetObserver(enumerator.Current);

            //    // Make sure we recognise any Domain Objects that were added by domain code:
            //    oItem.AssociateWith(this);

            //    currentContents[oItem.ID] = oItem;
            //}

            //// Make sure we recognise any Domain Objects that were removed by domain code:
            //if (_oPreviousContents.Count > 0)
            //{
            //    foreach (var oKnownItem in _oPreviousContents)
            //    {
            //        if (currentContents.DoesNotContain(oKnownItem.ID))
            //        {
            //            this.UpdatePersistenceForRemovedItem(oKnownItem);

            //            // We can only disassociate after the Persistence settings have been modified:
            //            oKnownItem.DisassociateFrom(this);
            //        }
            //    }

            //    _oPreviousContents.Clear();
            //}

            //if (currentContents.Count > 0)
            //{
            //    _oPreviousContents.AddRange(currentContents.Values);
            //}

            //// Return the current list contents (we're saving on variables):
            //return _oPreviousContents;

            //throw new NotImplementedException();
        }
    }
}