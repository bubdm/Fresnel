using Envivo.Fresnel.DomainTypes;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    ///
    /// </summary>
    public class Money : BaseValueObject
    {
        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description for this Product
        /// </summary>
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Money);
        }

        public bool Equals(Money money)
        {
            if (money == null)
                return false;

            return string.Equals(this.Name, money.Name) &&
                   string.Equals(this.Description, money.Description);
        }

        public override string ToString()
        {
            return this.Name ?? typeof(Money).Name;
        }
    }
}