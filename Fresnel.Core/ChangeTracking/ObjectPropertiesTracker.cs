using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    public class ObjectPropertiesTracker : IDisposable
    {
        private ObjectObserver _oObject;
        private Dictionary<BasePropertyObserver, PropertyChangeTracker> _PropertyTrackerMap;

        public ObjectPropertiesTracker(ObjectObserver oObject)
        {
            _oObject = oObject;
        }

        public void DetermineInitialState()
        {
            _PropertyTrackerMap = new Dictionary<BasePropertyObserver, PropertyChangeTracker>();

            if (_oObject != null)
            {
                foreach (var oProp in _oObject.Properties.Values)
                {
                    var tracker = new PropertyChangeTracker(oProp);
                    _PropertyTrackerMap[oProp] = tracker;
                    tracker.DetermineInitialState();
                }
            }
        }

        public void DetectChanges(BasePropertyObserver oProperty)
        {
            var tracker = _PropertyTrackerMap[oProperty];
            tracker.DetectChanges();
        }

        public IAssertion DetectChanges()
        {
            if (_PropertyTrackerMap.Count > 0)
            {
                foreach (var tracker in _PropertyTrackerMap.Values)
                {
                    tracker.DetectChanges();
                }
            }

            if (_PropertyTrackerMap.Values.Any(t => t.HasChanges))
            {
                return Assertion.Pass();
            }

            return Assertion.Fail("No changes detected");
        }

        public void Reset()
        {
            if (_PropertyTrackerMap.Count == 0)
                return;

            foreach (var tracker in _PropertyTrackerMap.Values)
            {
                tracker.Reset();
            }
        }

        public void Dispose()
        {
            if (_PropertyTrackerMap.Count > 0)
            {
                foreach (var tracker in _PropertyTrackerMap.Values)
                {
                    tracker.Dispose();
                }
                _PropertyTrackerMap.Clear();
            }

            _PropertyTrackerMap = null;
            _oObject = null;
        }

        public IEnumerable<PropertyChange> GetChangesSince(long startedAt)
        {
            var results = _PropertyTrackerMap
                            .Values
                            .SelectMany(t => t.Changes)
                            .Where(c => c.Sequence > startedAt) // Skip/Take might be quicker
                            .ToArray();

            return results;
        }
    }
}