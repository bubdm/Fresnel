using Envivo.Fresnel.Introspection.Templates;
using Newtonsoft.Json;
using System;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// The base class for all Observer classes
    /// </summary>
    public abstract class BaseObserver : IDisposable
    {
        private DateTimeOffset _CreatedAt;

        internal BaseObserver(Type objectType, ITemplate template)
        {
            this.ID = Guid.NewGuid();
            this.Template = template;
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

        internal virtual void FinaliseConstruction()
        {
        }

        public virtual void Dispose()
        {
            this.InnerObserver = null;
            this.Template = null;
        }
    }
}