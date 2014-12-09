using System;
using System.Collections.Generic;
using System.Text;
using Envivo.Fresnel.SampleModel.BasicTypes;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;

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
        public virtual Guid ID { get; set; }

        /// <summary>
        /// This value can only be read
        /// </summary>
        [Permissions(AllowedOperations = Allow.Read, User = "Vij")]
        public virtual bool BooleanValue { get; set; }

        /// <summary>
        /// This value can only be set
        /// </summary>
        [Permissions(AllowedOperations = Allow.Write, User = "Vij")]
        public virtual DateTime DateValue { get; set; }

        /// <summary>
        /// This value can be read and set
        /// </summary>
        [Permissions(AllowedOperations = Allow.Read | Allow.Write, User = "Vij")]
        public virtual string TextValue { get; set; }

    }
}
