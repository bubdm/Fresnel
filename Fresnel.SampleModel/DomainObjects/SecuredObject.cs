using System;
using System.Collections.Generic;
using System.Text;
using Envivo.Fresnel.SampleModel.BasicTypes;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Utils;
using Envivo.Fresnel.Core.Configuration;

namespace Envivo.Fresnel.SampleModel.Objects
{
    public class SecuredObject
    {

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var that = obj as SecuredObject;
            if (that == null)
                return false;

            return this.ID.Equals(that.ID);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

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
