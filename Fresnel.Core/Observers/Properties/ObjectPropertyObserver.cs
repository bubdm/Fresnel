using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;

namespace Envivo.Fresnel.Core.Observers
{

    public class ObjectPropertyObserver : BasePropertyObserver
    {

        public ObjectPropertyObserver(ObjectObserver oParent, PropertyTemplate tProperty)
            : base(oParent, tProperty)
        {
            
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
        internal void ResetLazyLoadStatus(bool isOuterClassPersistable)
        {
            if (isOuterClassPersistable)
            {
                // If the Property isn't marked as 'lazy loaded', we need to make sure it is loaded immediately:
                var tProp = this.Template;
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
