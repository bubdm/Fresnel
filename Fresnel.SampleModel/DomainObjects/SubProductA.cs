using Envivo.Fresnel.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// </summary>
    public class SubProductA : Product
    {
        /// <summary>
        /// This property should only be available on SubProductA
        /// </summary>
        [DataType(DataType.Currency)]
        public double Discount { get; set; }

        /// <summary>
        /// This is from the derived class
        /// </summary>
        public new string HiddenProperty { get; set; }
    }
}