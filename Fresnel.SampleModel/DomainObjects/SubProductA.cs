using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;

namespace Envivo.Sample.Model.Objects
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
