using Envivo.Fresnel.Introspection.Templates;

namespace Envivo.Fresnel.Core.Observers
{
    public class NonReferencePropertyObserver : BasePropertyObserver
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="oParent">The ObjectObserver that owns this Property</param>
        /// <param name="tProperty">The PropertyTemplate that reflects the Property</param>
        public NonReferencePropertyObserver(ObjectObserver oParent, PropertyTemplate tProperty)
            : base(oParent, tProperty)
        {
        }
    }
}