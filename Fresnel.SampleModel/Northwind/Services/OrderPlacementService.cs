using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.SampleModel.Northwind.Services
{
    /// <summary>
    /// Command tasks for placing Orders
    /// </summary>
    public class OrderPlacementService : IDomainService
    {

        /// <summary>
        /// Creates an Order for the given Customer and Product
        /// </summary>
        /// <param name="customer">The Customer placing the order</param>
        /// <param name="product">The Product to be ordered</param>
        /// <param name="quantity">The number of items required</param>
        /// <returns></returns>
        public Order PlaceNewOrder(Customer customer, Product product, int quantity)
        {
            var order = new Order
            {
                DeliverTo = customer
            };

            var orderItem = new OrderItem
            {
                ParentOrder = order,
                Product = product,
                Quantity = quantity
            };

            order.OrderItems.Add(orderItem);

            return order;
        }

    }
}
