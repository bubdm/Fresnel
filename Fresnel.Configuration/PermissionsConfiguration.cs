using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Attributes for providing static Authorisation
    /// </summary>
    public class PermissionsConfiguration
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
        public Allow AllowedOperations { get; set; }
    }

    [Flags]
    public enum Allow
    {
        /// <summary>
        /// No permissions
        /// </summary>
        None = 0,

        /// <summary>
        /// Allow Create
        /// </summary>
        Create = 1,

        /// <summary>
        /// Allow Read
        /// </summary>
        Read = 2,

        /// <summary>
        /// Allow Write
        /// </summary>
        Write = 4,

        /// <summary>
        /// Allow Add to a List or Collection
        /// </summary>
        Add = 8,

        /// <summary>
        /// Allow Remove from a List or Collection
        /// </summary>
        Remove = 16,

        /// <summary>
        /// Allow Method Invoke
        /// </summary>
        Invoke = 32,

        /// <summary>
        /// Allow All operations
        /// </summary>
        All = 255
    }
}