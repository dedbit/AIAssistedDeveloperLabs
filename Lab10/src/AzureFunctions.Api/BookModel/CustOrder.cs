using System;
using System.Collections.Generic;

namespace AzureFunctions.Api.BookModel
{
    public class CustOrder
    {
        public CustOrder()
        {
            OrderHistories = new HashSet<OrderHistory>();
            OrderLines = new HashSet<OrderLine>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? CustomerId { get; set; }
        public int? ShippingMethodId { get; set; }
        public int? DestAddressId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Address DestAddress { get; set; }
        public virtual ShippingMethod ShippingMethod { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
