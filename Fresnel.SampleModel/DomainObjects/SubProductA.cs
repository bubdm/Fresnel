using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// </summary>
    public class SubProductA : Product
    {
        /// <summary>
        /// This property should only be available on SubProductA
        /// </summary>
        [NumberConfiguration(IsCurrency = true)]
        public virtual double Discount { get; set; }

        /// <summary>
        /// This is from the derived class
        /// </summary>
        public virtual new string HiddenProperty { get; set; }
    }
}