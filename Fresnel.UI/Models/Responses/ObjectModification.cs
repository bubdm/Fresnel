using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fresnel.UI.Models.Responses
{
    /// <summary>
    /// The modifications made to an object as part of a Command
    /// </summary>
    public class ObjectModification
    {
        public Guid ObjectId { get; set; }

        public PropertyChange[] PropertyChanges { get; set; }

    }
}
