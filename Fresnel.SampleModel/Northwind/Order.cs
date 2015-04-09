using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Order
    {
        private ICollection<OrderItem> _OrderItem = new List<OrderItem>();

        [Key]
        public Guid ID { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }

        public virtual ICollection<OrderItem> OrderItems
        {
            get { return _OrderItem; }
            set { _OrderItem = value; }
        }
    }
}