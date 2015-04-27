using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using System.ComponentModel.DataAnnotations;

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
        public bool IsLazyLoadPending
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

                var lazyLoad = tProp.Attributes.Get<LazyLoadAttribute>();
                this.IsLazyLoadPending = lazyLoad.IsEnabled;
            }
            else
            {
                // The object only exists in memory, so we should be able to access it's contents immediately:
                this.IsLazyLoadPending = false;
            }
        }
    }
}