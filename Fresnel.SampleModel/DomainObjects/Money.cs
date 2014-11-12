using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;

namespace Envivo.Sample.Model.Objects
{
    public class Money : ValueObjectBase
    {

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

    }
}
