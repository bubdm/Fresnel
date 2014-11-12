using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Core.Configuration;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// </summary>
    public class SubProductA : Product
    {
        /// <summary>
        /// This property should only be available on SubProductA
        /// </summary>
        [Number(IsCurrency = true)]
        public double Discount { get; set; }

        /// <summary>
        /// This is from the derived class
        /// </summary>
        public new string HiddenProperty { get; set; }

    }
}
