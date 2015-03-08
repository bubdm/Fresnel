using Envivo.Fresnel.Configuration;
using System;
using System.Security.Permissions;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    ///
    /// </summary>
    public class SecuredObject
    {
        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// This value can only be read
        /// </summary>
        //[PermissionsConfiguration(AllowedOperations = Allow.Read, User = "Vij")]
        public bool BooleanValue { get; set; }

        /// <summary>
        /// This value can only be set
        /// </summary>
        //[PermissionsConfiguration(AllowedOperations = Allow.Write, User = "Vij")]
        public DateTime DateValue { get; set; }

        /// <summary>
        /// This value can be read and set
        /// </summary>
        //[PermissionsConfiguration(AllowedOperations = Allow.Read | Allow.Write, User = "Vij")]
        public string TextValue { get; set; }
    }
}