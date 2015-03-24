using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    /// <summary>
    /// Keeps track of changes for an Object's Title (ie ToString())
    /// </summary>
    public class ObjectTitleTracker : IDisposable
    {
        private BaseObjectObserver _oObject;
        private List<ObjectTitleChange> _Changes = new List<ObjectTitleChange>();

        internal ObjectTitleTracker(BaseObjectObserver oObject)
        {
            _oObject = oObject;
        }

        public override string ToString()
        {
            return string.Concat(this.GetType().Name, ":", _oObject.Template.Name);
        }

        internal void DetermineInitialState()
        {
            try
            {
                var value = _oObject.RealObject.ToStringOrNull();
                this.PreviousValue = this.LatestValue = value;
            }
            catch (Exception ex)
            {
                this.PreviousValue = this.LatestValue = null;
            }
        }

        internal IEnumerable<ObjectTitleChange> Changes
        {
            get { return _Changes; }
        }

        internal string PreviousValue { get; private set; }

        internal string LatestValue { get; private set; }

        internal bool HasChanges
        {
            get { return object.Equals(this.PreviousValue, this.LatestValue) == false; }
        }

        internal IAssertion DetectChanges()
        {
            var veryLatestValue = _oObject.RealObject.ToStringOrNull();

            var latestChange = new ObjectTitleChange
            {
                Sequence = SequentialIdGenerator.Next,
                ObjectObserver = _oObject,
                OriginalValue = veryLatestValue,
                NewValue = veryLatestValue
            };
            _Changes.Add(latestChange);

            this.PreviousValue = this.LatestValue;
            this.LatestValue = veryLatestValue;
            return Assertion.Pass();
        }

        internal void Reset()
        {
            this.Clear();

            var latestValue = _oObject.RealObject.ToStringOrNull();
            this.LatestValue = this.PreviousValue = latestValue;
        }

        internal void Clear()
        {
            if (_Changes.Count == 0)
                return;

            foreach (var item in _Changes)
            {
                item.Dispose();
            }
            _Changes.Clear();
        }

        public void Dispose()
        {
            this.Clear();
            this.PreviousValue = null;
            this.LatestValue = null;

            _oObject = null;
        }

        public IEnumerable<ObjectTitleChange> GetChangesSince(long startedAt)
        {
            var results = _Changes
                            .Where(c => c.Sequence > startedAt) // Skip/Take might be quicker
                            .ToArray();

            return results;
        }
    }
}