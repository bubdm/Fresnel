using System;

using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Templates;

namespace Envivo.Fresnel.Engine.Observers
{

    /// <summary>
    /// The base class for all Observer classes
    /// </summary>
    public abstract class BaseObserver : IDisposable
    {
        private DateTimeOffset _CreatedAt;

        internal BaseObserver(object obj, Type objectType, ITemplate template)
        {
            if (obj is BaseObserver)
            {
                throw new ArgumentOutOfRangeException("Object cannot be an Observer");
            }

            this.ID = Guid.NewGuid();
            this.Template = template;
            this.RealObject = obj;
            _CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// A unique identifier for this Observer
        /// </summary>
        public Guid ID { get; internal set; }

        /// <summary>
        /// Returns a string that uniquely identifies this Observer
        /// </summary>
        internal virtual string DebugID
        {
            get { return string.Concat("[", this.ID.ToString().ToUpper(), " ", this.Template.FullName, "]"); }
        }

        /// <summary>
        /// The object (or value) that is being observed
        /// </summary>
        public object RealObject { get; private set; }

        internal virtual void SetRealObject(object obj)
        {
            if (obj is BaseObserver)
            {
                throw new ArgumentOutOfRangeException("Object cannot be an Observer");
            }

            if (object.ReferenceEquals(this.RealObject, obj) == false)
            {
                this.RealObject = obj;
            }
        }

        /// <summary>
        /// The underlying Template used to create this Observer
        /// </summary>
        protected ITemplate Template { get; private set; }

        /// <summary>
        /// Returns the underlying Template, casted to the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T TemplateAs<T>()
            where T : class, ITemplate
        {
            return this.Template as T;
        }

        public BaseObjectObserver InnerObserver { get; set; }

        public T InnerAs<T>()
            where T : BaseObjectObserver
        {
            return this.InnerObserver as T;
        }

        ///// <summary>
        ///// Returns the InnerObserver converted to an ObjectObserver
        ///// </summary>
        //public ObjectObserver InnerObject
        //{
        //    get { return this.InnerObserver as ObjectObserver; }
        //}

        ///// <summary>
        ///// Returns the InnerObserver converted to a NonReferenceObserver
        ///// </summary>
        //public NonReferenceObserver InnerNonReference
        //{
        //    get { return this.InnerObserver as NonReferenceObserver; }
        //}

        ///// <summary>
        ///// Returns the InnerObserver converted to a ListObserver
        ///// </summary>
        //internal CollectionObserver InnerCollection
        //{
        //    get { return this.InnerObserver as CollectionObserver; }
        //}

        /// <summary>
        /// Returns TRUE if this Observer was created before the given time point (i.e. DateTime.UtcNow)
        /// </summary>
        internal bool WasCreatedBefore(DateTimeOffset timePoint)
        {
            return _CreatedAt < timePoint;
        }

        internal virtual void FinaliseConstruction()
        {
        }

        public virtual void Dispose()
        {
            this.RealObject = null;
            this.InnerObserver = null;
            this.Template = null;
        }

    }
}
