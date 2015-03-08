using Envivo.Fresnel.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

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
        [Range(0, 10000)]
        public virtual double Quantity { get; set; }

        /// <summary>
        ///
        /// </summary>
        public virtual DateTime DeliveryDate { get; set; }
    }
}