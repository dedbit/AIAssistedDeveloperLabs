using System.Collections.Generic;

namespace AzureFunctions.Api.BookModel
{
    public class Publisher
    {
        public Publisher()
        {
            Books = new HashSet<Book>();
        }

        public int PublisherId { get; set; }
        public string PublisherName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
