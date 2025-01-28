using AzureFunctions.Api.BookModel;
using System;
using System.Collections.Generic;
using System.Linq;

public class BooksRepository
{
    private readonly gravity_booksContext _context;

    public BooksRepository(gravity_booksContext context)
    {
        _context = context;
    }

    public virtual IEnumerable<Customer> GetTopCustomers(int count)
    {
        return _context.Customers.Take(count);

    }

    public List<object> ComplexQuery()
    {
        if (_context.Books == null || _context.BookAuthors == null || _context.Authors == null || _context.Publishers == null || _context.BookLanguages == null)
        {
            throw new ArgumentNullException("One or more required tables are null.");
        }

        IQueryable<object> query = from book in _context.Books
                                   join bookAuthor in _context.BookAuthors on book.BookId equals bookAuthor.BookId
                                   join author in _context.Authors on bookAuthor.AuthorId equals author.AuthorId
                                   join publisher in _context.Publishers on book.PublisherId equals publisher.PublisherId
                                   join language in _context.BookLanguages on book.LanguageId equals language.LanguageId
                                   where publisher.PublisherName == "Specific Publisher"
                                   select new
                                   {
                                       BookTitle = book.Title,
                                       AuthorName = author.AuthorName,
                                       PublisherName = publisher.PublisherName,
                                       LanguageName = language.LanguageName
                                   };

        List<object> result = query.ToList();
        return result;
    }



}
