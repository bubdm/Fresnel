



namespace Envivo.Fresnel.Engine.Observers
{

    public static class ObserverExtensions
    {
        /// <summary>
        /// Returns TRUE if the Observer has no inner value
        /// </summary>
        /// <param name="observer"></param>
        
        internal static bool IsNullOrEmpty(this BaseObserver observer)
        {
            return (observer == null) || (observer.RealObject == null);
        }

        /// <summary>
        /// Returns TRUE if the Observer genuinely has an inner value
        /// </summary>
        /// <param name="observer"></param>
        
        internal static bool HasValue(this BaseObserver observer)
        {
            return (observer != null) && (observer.RealObject !=null);
        }
    }

}
