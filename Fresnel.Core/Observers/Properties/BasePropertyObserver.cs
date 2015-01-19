using Envivo.Fresnel.Introspection.Templates;
using Newtonsoft.Json;
using System;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// An Observer for a Property belonging to a Object
    /// </summary>
    public abstract class BasePropertyObserver : BaseMemberObserver
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="parentObject">The ObjectObserver that owns this Property</param>
        /// <param name="propertyTemplate">The PropertyTemplate that reflects the Property</param>
        internal BasePropertyObserver(ObjectObserver oParent, PropertyTemplate tSourceProperty)
            : base(oParent, tSourceProperty)
        {
        }

        [JsonIgnore]
        public new PropertyTemplate Template
        {
            get { return (PropertyTemplate)base.Template; }
        }

        public DateTime LastUpdatedAtUtc { get; internal set; }

        public object PreviousValue { get; set; }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}