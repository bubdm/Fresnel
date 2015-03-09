using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Attributes for providing static Authorisation
    /// </summary>
    public class AuthorisationConfiguration
    {
        /// <summary>
        /// The Role these Permissions apply to
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// The User these Permissions apply to
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The operations allowed for the named Role/User
        /// </summary>
        public AllowTypes AllowedOperations { get; set; }
    }

    [Flags]
    public enum AllowTypes
    {
        /// <summary>
        /// No permissions
        /// </summary>
        None,

        /// <summary>
        /// Allow Create
        /// </summary>
        Create,

        /// <summary>
        /// Allow Read
        /// </summary>
        Read,

        /// <summary>
        /// Allow Write
        /// </summary>
        Write,

        /// <summary>
        /// Allow Add to a List or Collection
        /// </summary>
        Add,

        /// <summary>
        /// Allow Remove from a List or Collection
        /// </summary>
        Remove,

        /// <summary>
        /// Allow the item to be cleared
        /// </summary>
        Clear,

        /// <summary>
        /// Allow Method Invoke
        /// </summary>
        Invoke,

        /// <summary>
        /// Allow All operations
        /// </summary>
        All
    }
}