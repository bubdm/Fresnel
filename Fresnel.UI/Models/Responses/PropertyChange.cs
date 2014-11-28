using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fresnel.UI.Models.Responses
{
    public class PropertyChange
    {
        /// <summary>
        /// The name of the Property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of the property (if not an object)
        /// </summary>
        public object ReferenceValue { get; set; }

        /// <summary>
        /// The value of the property (if an object)
        /// </summary>
        public object ObjectValue { get; set; }

    }
}
