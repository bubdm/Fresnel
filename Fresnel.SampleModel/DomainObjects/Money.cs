using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.SampleModel.Objects
{
    public class Money : BaseValueObject
    {

        public virtual string Name { get; set; }

        /// <summary>
        /// The description for this Product
        /// </summary>
        public virtual string Description { get; set; }

    }
}
