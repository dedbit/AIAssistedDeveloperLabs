using AzureFunctions.Api.BookModel;
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

    public void ComplexQuery()
    {
        // Todo: Setup this query for explaining
        // CurrentData.AppConfidential_FromDb = await (from o in _context.AppConfidential
        //                          where o.CustomerId == _configuration.CustomerId
        //                          && o.SystemId == _configuration.System
        //                          && (!o.Deleted.HasValue) // Dem som ikke er fjernet (dem der er softdeleted, skal potentielt stadig vedligeholdes pga obslisten
        //                          select o).ToDictionaryAsync(x => CurrentData.GetKey_AppConfidential_Current(x.Type, x.Identifier, x.InstitutionId, x.SectionId, x.UserId), StringComparer.OrdinalIgnoreCase);

    }



}
