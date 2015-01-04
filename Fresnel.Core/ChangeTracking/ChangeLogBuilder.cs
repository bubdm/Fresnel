using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    public class ChangeLogBuilder
    {
        private ObserverCache _ObserverCache;

        public ChangeLogBuilder
            (
            ObserverCache observerCache
            )
        {
            _ObserverCache = observerCache;
        }

        public ChangeLog BuildFrom(IEnumerable<ObjectObserver> objectObservers, long startedAt)
        {
            var result = new ChangeLog();

            foreach (var oObject in objectObservers)
            {
                var creation = oObject.ChangeTracker.GetObjectCreation();
                if (creation.Sequence >= startedAt)
                {
                    result.NewObjects.Add(creation);
                }

                var propertyChanges = oObject.ChangeTracker.GetPropertyChangesSince(startedAt);
                foreach (var change in propertyChanges)
                {
                    result.PropertyChanges.Add(change);
                }

                var oCollection = oObject as CollectionObserver;
                if (oCollection != null)
                {
                    var additions = oCollection.ChangeTracker.GetCollectionAdditionsSince(startedAt);
                    foreach (var change in additions)
                    {
                        result.CollectionAdditions.Add(change);
                    }

                    var removals = oCollection.ChangeTracker.GetCollectionRemovalsSince(startedAt);
                    foreach (var change in additions)
                    {
                        result.CollectionAdditions.Add(change);
                    }
                }
            }

            return result;
        }

    }
}
