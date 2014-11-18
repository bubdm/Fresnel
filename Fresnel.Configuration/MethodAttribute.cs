using System;


namespace Envivo.Fresnel.Configuration
{

    /// <summary>
    /// Attributes for a class Method
    /// </summary>
    
    [Serializable()]
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodAttribute : MemberAttribute
    {
        /// <summary>
        /// Determines if the method must run on the remote server (if available)
        /// </summary>
        public bool IsRunOnServer { get; set; }

        /// <summary>
        /// Determines if the method can be executed if unsaved objects are still being used
        /// </summary>
        public bool AllowWithUnsavedObjects { get; set; }
    }

}
