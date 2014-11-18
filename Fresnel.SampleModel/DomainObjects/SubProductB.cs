using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// This class has a custom Presenter associated with it
    /// </summary>
    public class SubProductB : Product
    {

        /// <summary>
        /// This property should only be available on SubProductB
        /// </summary>
        [Number(MinValue = 0, MaxValue = 10000)]
        public double Quantity { get; set; }

        public DateTime DeliveryDate { get; set; }

    }
}
