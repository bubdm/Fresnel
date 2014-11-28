using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fresnel.UI.Models.Responses
{
    /// <summary>
    /// Any objects created as part of a Command
    /// </summary>
    public class ObjectCreation
    {
        public Guid ObjectId { get; set; }

        public PropertyChange[] PropertyChanges { get; set; }

    }
}
