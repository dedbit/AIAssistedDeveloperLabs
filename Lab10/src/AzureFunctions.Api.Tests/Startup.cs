using AzureFunctions.Api.Managers;
using Microsoft.Extensions.DependencyInjection;
using AzureFunctions.Api.Helpers;
using AzureFunctions.Api.Repositories;
using AzureFunctions.Api.Tests.Mocks;

namespace AzureFunctions.Api.Tests
{
    class Startup
    {
        public delegate void LogRetryMessage(string messsage);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.AddSingleton<FunctionHelper>();
            services.AddSingleton<ProjectRepository, ProjectRepositoryFake>();
            services.AddSingleton<TestDataModel>();

            ConfigManager _configManager = new ConfigManager();
            services.AddSingleton(_configManager);


            services.BuildServiceProvider();
            
        }

        
    }
}
