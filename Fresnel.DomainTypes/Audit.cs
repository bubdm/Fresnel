using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.DomainTypes
{
    /// <summary>
    /// Tracks simple create/update/delete statistics for a Domain Object
    /// </summary>
    [Serializable]
    public class Audit : IAudit
    {
        public virtual IDomainObject DomainObject { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedAt { get; set; }

        public virtual string UpdatedBy { get; set; }

        public virtual DateTime? UpdatedAt { get; set; }

        public virtual string DeletedBy { get; set; }

        public virtual DateTime? DeletedAt { get; set; }
    }
}