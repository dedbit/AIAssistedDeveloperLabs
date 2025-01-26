using System.Collections.Generic;

namespace AzureFunctions.Api.BookModel
{
    public partial class BookLanguage
    {
        public BookLanguage()
        {
            Books = new HashSet<Book>();
        }

        public int LanguageId { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
