using System;
using System.Collections.Generic;

namespace AzureFunctions.Api.BookModel
{
    public class Book
    {
        public Book()
        {
            BookAuthors = new HashSet<BookAuthor>();
            OrderLines = new HashSet<OrderLine>();
        }

        public int BookId { get; set; }
        public string Title { get; set; }
        public string Isbn13 { get; set; }
        public int? LanguageId { get; set; }
        public int? NumPages { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int? PublisherId { get; set; }

        public virtual BookLanguage Language { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
