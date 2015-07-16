using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// The base class for all Object Observers.
    /// </summary>
    public abstract class BaseObjectObserver : BaseObserver
    {
        private Lazy<List<CollectionObserver>> _OuterCollections;
        private Lazy<List<ObjectPropertyObserver>> _OuterProperties;
        private Lazy<List<ParameterObserver>> _OuterParameters;

        public BaseObjectObserver
        (
            object obj,
            Type objectType,
            BaseClassTemplate sourceTemplate
        )
            : base(objectType, sourceTemplate)
        {
            if (obj is BaseObserver)
            {
                throw new ArgumentOutOfRangeException("Object cannot be an Observer");
            }

            this.RealObject = obj;

            _OuterCollections = new Lazy<List<CollectionObserver>>(
                                    () => new List<CollectionObserver>(),
                                    System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _OuterProperties = new Lazy<List<ObjectPropertyObserver>>(
                                    () => new List<ObjectPropertyObserver>(),
                                    System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

            _OuterParameters = new Lazy<List<ParameterObserver>>(
                                    () => new List<ParameterObserver>(),
                                    System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        [JsonIgnore]
        public new BaseClassTemplate Template
        {
            get { return (BaseClassTemplate)base.Template; }
        }

        /// <summary>
        /// The object (or value) that is being observed
        /// </summary>
        [JsonIgnore]
        public object RealObject { get; private set; }

        public virtual void SetRealObject(object obj)
        {
            if (obj is BaseObserver)
            {
                throw new ArgumentOutOfRangeException("Object cannot be an Observer");
            }

            if (!object.ReferenceEquals(this.RealObject, obj))
            {
                this.RealObject = obj;
            }
        }

        /// <summary>
        /// A set of all Lists that have a reference to this Observer
        /// </summary>
        /// <remarks>In any object graph, it is possible for a single object to be referenced from more than one object.
        /// This property allows a reference to each of those objects.</remarks>
        [JsonIgnore]
        public IEnumerable<CollectionObserver> OuterCollections
        {
            get { return _OuterCollections.Value; }
        }

        /// <summary>
        /// A set of all Properties that have a reference to this Observer
        /// </summary>
        /// <remarks>In any object graph, it is possible for a single object to be referenced from more than one object (via a Property).
        /// This property allows a reference to each of those Properties.</remarks>
        [JsonIgnore]
        public IEnumerable<ObjectPropertyObserver> OuterProperties
        {
            get { return _OuterProperties.Value; }
        }

        /// <summary>
        /// A set of all Parameters that have a reference to this Observer
        /// </summary>
        /// <value>A generic Dictionary of ParameterObserver</value>
        /// <remarks>In any object graph, it is possible for a single object to be referenced from more than one object (via a Parameter).
        /// This property allows a reference to each of those Parameters.</remarks>
        [JsonIgnore]
        public IEnumerable<ParameterObserver> OuterParameters
        {
            get { return _OuterParameters.Value; }
        }

        /// <summary>
        /// Associates this Observer with the given owner
        /// </summary>
        /// <param name="oOuterCollection"></param>
        public void AssociateWith(CollectionObserver oOuterCollection)
        {
            var outerCollections = _OuterCollections.Value;
            if (outerCollections.DoesNotContain(oOuterCollection))
            {
                outerCollections.Add(oOuterCollection);
            }
        }

        /// <summary>
        /// Disassociates this Observer from the given owner
        /// </summary>
        /// <param name="oOuterCollection"></param>
        public void DisassociateFrom(CollectionObserver oOuterCollection)
        {
            var outerCollections = _OuterCollections.Value;
            if (_OuterCollections.Value.Contains(oOuterCollection))
            {
                _OuterCollections.Value.Remove(oOuterCollection);
            }
        }

        /// <summary>
        /// Associates this Observer with the given owner
        /// </summary>
        /// <param name="oOuterProperty"></param>
        public void AssociateWith(ObjectPropertyObserver oOuterProperty)
        {
            var outerProperties = _OuterProperties.Value;
            if (outerProperties.DoesNotContain(oOuterProperty))
            {
                _OuterProperties.Value.Add(oOuterProperty);
            }
        }

        /// <summary>
        /// Disassociates this Observer from the given owner
        /// </summary>
        /// <param name="oOuterProperty"></param>
        public void DisassociateFrom(ObjectPropertyObserver oOuterProperty)
        {
            var outerProperties = _OuterProperties.Value;
            if (outerProperties.Contains(oOuterProperty))
            {
                outerProperties.Remove(oOuterProperty);
            }
        }

        /// <summary>
        /// Detaches this Object from any owners
        /// </summary>

        public void MakeOrphan()
        {
            // NB: We're using a While loop to circumvent a "Collection was modified" exception:

            // Remove this object from any Collections:
            var outerCollections = this.OuterCollections.ToList();
            foreach (var oCollection in outerCollections)
            {
                this.DisassociateFrom(oCollection);
            }

            // Remove this object from any Properties:
            var outerProperties = this.OuterProperties.ToList();
            foreach (var oProperty in outerProperties)
            {
                this.DisassociateFrom(oProperty);
            }

            //this.ChangeTracker.ResetDirtyFlags();
        }

        /// <summary>
        /// Ensures that this Observer is never cleaned up
        /// </summary>
        public bool IsPinned { get; set; }

        public override void Dispose()
        {
            if (this.IsPinned)
                return;

            if (_OuterCollections != null)
            {
                _OuterCollections.Value.ClearSafely();
            }

            if (_OuterProperties != null)
            {
                _OuterProperties.Value.ClearSafely();
            }

            if (_OuterParameters != null)
            {
                _OuterParameters.Value.ClearSafely();
            }

            base.Dispose();
        }
    }
}