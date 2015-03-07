using Envivo.Fresnel.Configuration;
using System;

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
        [NumberConfiguration(MinValue = 0, MaxValue = 10000)]
        public virtual double Quantity { get; set; }

        /// <summary>
        ///
        /// </summary>
        public virtual DateTime DeliveryDate { get; set; }
    }
}