using System.Collections.Generic;

namespace AzureFunctions.Api.BookModel
{
    public class OrderStatus
    {
        public OrderStatus()
        {
            OrderHistories = new HashSet<OrderHistory>();
        }

        public int StatusId { get; set; }
        public string StatusValue { get; set; }

        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
    }
}
