using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AzureFunctions.Api.Managers;

namespace AzureFunctions.Api.Functions
{
    public class HttpTrigger
    {
        private readonly ConfigManager _configManager;

        public HttpTrigger(ConfigManager configManager)
        {
            _configManager = configManager;
        }

        [FunctionName("HttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTrigger called");

            string ServiceUsername = _configManager.GetConfigValue("ServiceUsername");

            log.LogInformation($"HttpTrigger completed successfully with Username {ServiceUsername}");
            return (ActionResult)new OkObjectResult($"Success");

        }
    }
}
