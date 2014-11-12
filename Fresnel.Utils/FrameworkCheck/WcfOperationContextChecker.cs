


namespace Envivo.Fresnel.Utils
{
    /// <summary>
    /// 
    /// </summary>
    internal class WcfOperationContextChecker
    {
        /// <summary>
        /// Returns TRUE if the WCF ServiceModel.OperationContext is available
        /// </summary>
        /// <returns></returns>
        internal bool IsAvailable()
        {
            return System.ServiceModel.OperationContext.Current != null;
        }

    }

}
