using System.Collections.Generic;

namespace AzureFunctions.Api.BookModel
{
    public class Author
    {
        public Author()
        {
            BookAuthors = new HashSet<BookAuthor>();
        }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
