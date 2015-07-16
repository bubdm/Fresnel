namespace Envivo.Fresnel.Core.Observers
{
    public static class ObserverExtensions
    {
        /// <summary>
        /// Returns TRUE if the Observer has no inner value
        /// </summary>
        /// <param name="observer"></param>
        public static bool IsNullOrEmpty(this BaseObjectObserver observer)
        {
            return (observer == null) || (observer.RealObject == null);
        }

        /// <summary>
        /// Returns TRUE if the Observer genuinely has an inner value
        /// </summary>
        /// <param name="observer"></param>
        public static bool HasValue(this BaseObjectObserver observer)
        {
            return (observer != null) && (observer.RealObject != null);
        }
    }
}