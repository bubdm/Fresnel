using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;

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
        public virtual string Name { get; set; }

        /// <summary>
        /// The description for this Product
        /// </summary>
        public virtual string Description { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Money);
        }

        public virtual bool Equals(Money money)
        {
            if (money == null)
                return false;

            return string.Equals(this.Name, money.Name) &&
                   string.Equals(this.Description, money.Description);
        }


    }
}
