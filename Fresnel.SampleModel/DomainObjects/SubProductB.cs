using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;

namespace Envivo.Sample.Model.Objects
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
