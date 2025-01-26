namespace AzureFunctions.Api.BookModel
{
    public class CustomerAddress
    {
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public int? StatusId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
