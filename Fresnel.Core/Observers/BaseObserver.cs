using System;

using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Templates;
using Newtonsoft.Json;

namespace Envivo.Fresnel.Core.Observers
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
        
        public override string ToString()
        {
            return string.Concat(this.GetType().Name, ":", this.Template.Name);
        }

        /// <summary>
        /// The object (or value) that is being observed
        /// </summary>
        [JsonIgnore]
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
        [JsonIgnore]
        public ITemplate Template { get; private set; }

        [JsonIgnore]
        public BaseObjectObserver InnerObserver { get; set; }

        public T InnerAs<T>()
            where T : BaseObjectObserver
        {
            return this.InnerObserver as T;
        }


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
