using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AzureFunctions.Api.BookModel;
using AzureFunctions.Api.Repositories;

namespace AzureFunctions.Api.Functions
{
    public class GetCustomers
    {
        private readonly BooksRepository _booksRepository;

        public GetCustomers(BooksRepository booksRepository)
        {
            _booksRepository = booksRepository;

            // Add a sample customer if none exists
            if (!_booksRepository.GetTopCustomers(1).Any())
            {
                var contextField = typeof(BooksRepository)
                    .GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var context = contextField?.GetValue(_booksRepository) as gravity_booksContext;
                if (context != null)
                {
                    context.Customers.Add(new Customer
                    {
                        FirstName = "Sample",
                        LastName = "Customer",
                        Email = "sample@example.com"
                    });
                    context.SaveChanges();
                }
            }

        }

        [FunctionName(nameof(GetCustomers))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("GetCustomer called");

                var customers = _booksRepository.GetTopCustomers(20).ToList();
                var customer = customers.FirstOrDefault();
                if (customer == null)
                {
                    return new NotFoundResult();
                }

                var customerOutput = customers.Select(c => c).ToArray();

                return new OkObjectResult(customerOutput);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Unexpected error in GetCustomer");
                return new ObjectResult("Unexpected error");
            }
        }

        
    }
}
