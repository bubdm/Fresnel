using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a class Method
    /// </summary>
    public class MethodConfiguration : MemberConfiguration
    {
        /// <summary>
        /// Determines whether access to this member can be executed on a separate thread,
        /// Set this attibute to FALSE if the member accesses a resource that is not thread safe.
        /// </summary>
        /// <value></value>
        public bool IsAsynchronous { get; set; }

        /// <summary>
        /// Determines if the method can be executed if unsaved objects are still being used
        /// </summary>
        public bool AllowWithUnsavedObjects { get; set; }
    }
}