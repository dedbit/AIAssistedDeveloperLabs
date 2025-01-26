using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Api.Functions
{
    public class GetCustomers
    {
        private readonly BooksRepository _booksRepository;

        public GetCustomers(BooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
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

                var customerOutput = customers.Select(c => c.FirstName).ToArray();

                return new OkObjectResult(customerOutput);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Unexpected error in GetCustomer");
                //throw;
                return new ObjectResult("Unexpected error");

            }

        }
    }
}
