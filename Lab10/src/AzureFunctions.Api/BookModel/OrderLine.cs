namespace AzureFunctions.Api.BookModel
{
    public class OrderLine
    {
        public int LineId { get; set; }
        public int? OrderId { get; set; }
        public int? BookId { get; set; }
        public decimal? Price { get; set; }

        public virtual Book Book { get; set; }
        public virtual CustOrder Order { get; set; }
    }
}
