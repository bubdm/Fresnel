//.################################################
//# Copyright © 2006-2011 Evolving Software Ltd   #
//# Author: Vijay Patel                           #
//################################################'

using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.ChangeTracking
{
    /// <summary>
    /// Keeps track of changes made to a property
    /// </summary>
    public class PropertyChangeTracker : IDisposable
    {
        private BasePropertyObserver _oProperty;
        private List<PropertyChange> _Changes = new List<PropertyChange>();

        internal PropertyChangeTracker(BasePropertyObserver oProperty)
        {
            _oProperty = oProperty;
        }

        public override string ToString()
        {
            return string.Concat(this.GetType().Name, ":", _oProperty.Template.Name);
        }

        internal void DetermineInitialState()
        {
            try
            {
                var value = _oProperty.Template.GetProperty(_oProperty.OuterObject.RealObject);
                this.PreviousValue = this.LatestValue = value;
            }
            catch (Exception ex)
            {
                this.PreviousValue = this.LatestValue = null;
            }
        }

        internal IEnumerable<PropertyChange> Changes
        {
            get { return _Changes; }
        }

        internal object PreviousValue { get; private set; }

        internal object LatestValue { get; private set; }

        internal bool HasChanges
        {
            get { return object.Equals(this.PreviousValue, this.LatestValue) == false; }
        }

        internal IAssertion DetectChanges()
        {
            // Don't access Properties that haven't been lazy loaded yet:
            var oObjectProperty = _oProperty as ObjectPropertyObserver;

            if (oObjectProperty != null &&
                oObjectProperty.OuterObject.Template.IsPersistable &&
                oObjectProperty.IsLazyLoadPending)
            {
                return Assertion.Fail("Property hasn't been lazy-loaded");
            }

            object veryLatestValue = null;
            try
            {
                veryLatestValue = _oProperty.Template.GetProperty(_oProperty.OuterObject.RealObject);
            }
            catch (Exception ex)
            {
                // The property getter might throw an exception:
                return Assertion.Fail(ex);
            }

            if (object.Equals(this.LatestValue, veryLatestValue))
            {
                return Assertion.Fail("No changes detected");
            }

            var latestChange = new PropertyChange
            {
                Sequence = SequentialIdGenerator.Next,
                Property = _oProperty,
                OriginalValue = this.LatestValue,
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

            var latestValue = _oProperty.Template.GetProperty(_oProperty.OuterObject.RealObject);
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

            _oProperty = null;
        }
    }
}