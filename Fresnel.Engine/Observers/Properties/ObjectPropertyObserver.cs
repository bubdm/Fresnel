using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;

namespace Envivo.Fresnel.Engine.Observers
{

    public class ObjectPropertyObserver : BasePropertyObserver
    {

        public ObjectPropertyObserver(ObjectObserver oParent, PropertyTemplate tProperty)
            : base(oParent, tProperty)
        {
            this.ResetLazyLoadStatus();
        }

        /// <summary>
        /// Determines whether reading the Property value will trigger a lazy-load operation
        /// </summary>
        internal bool IsLazyLoadPending
        {
            get { return !this.IsLazyLoaded; }
            set { this.IsLazyLoaded = !value; }
        }

        /// <summary>
        /// Determines whether the Property value is immediately available
        /// </summary>
        public bool IsLazyLoaded { get; set; }

        /// <summary>
        /// Sets the lazy-loading flags to their original states
        /// </summary>
        internal void ResetLazyLoadStatus()
        {
            var tProp = this.TemplateAs<PropertyTemplate>();
            if (tProp.OuterClass.IsPersistable)
            {
                // If the Property isn't marked as 'lazy loaded', we need to make sure it is loaded immediately:
                this.IsLazyLoadPending = tProp.Attributes.Get<ObjectPropertyBaseAttribute>().IsLazyLoaded;
            }
            else
            {
                // The object only exists in memory, so we should be able to access it's contents immediately:
                this.IsLazyLoadPending = false;
            }
        }

    }
}
