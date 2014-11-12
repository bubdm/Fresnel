
using System.Collections.Generic;
using System.Text;
using System;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{
    /// <summary>
    /// Used for recording basic audit information for a persisted Domain Object
    /// </summary>
    public interface IAudit
    {
        IDomainObject DomainObject { get; set; }

        string CreatedBy { get; set; }

        DateTime? CreatedAt { get; set; }

        string UpdatedBy { get; set; }

        DateTime? UpdatedAt { get; set; }

        string DeletedBy { get; set; }

        DateTime? DeletedAt { get; set; }
    }
}
