using AzureFunctions.Api;
using AzureFunctions.Api.BookModel;
using AzureFunctions.Api.Clients;
using AzureFunctions.Api.Helpers;
using AzureFunctions.Api.Managers;
using AzureFunctions.Api.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory; // Add this using directive
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureFunctions.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var _configManager = new ConfigManager();
            var services = builder.Services;

            services.AddSingleton(_configManager);
            services.AddSingleton<FunctionHelper>();

            // Register ProjectRepository
            services.AddSingleton<ProjectRepository>(provider =>
            {
                var configManager = provider.GetRequiredService<ConfigManager>();
                return new ProjectRepository(configManager);
            });

            services.BuildServiceProvider();

            // Use an in-memory database instead of SQL
            services.AddDbContext<gravity_booksContext>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));


            services.AddScoped<BooksRepository>();
        }
    }
}