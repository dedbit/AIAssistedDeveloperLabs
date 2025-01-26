using AzureFunctions.Api;
using AzureFunctions.Api.BookModel;
using AzureFunctions.Api.Helpers;
using AzureFunctions.Api.Managers;
using AzureFunctions.Api.Repositories;
//using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureFunctions.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigManager _configManager = new ConfigManager();

            var services = builder.Services;
            
            services.AddSingleton(_configManager);
            services.AddSingleton<FunctionHelper>();
            services.AddSingleton<ProjectRepository>();
            services.BuildServiceProvider();

            string connectionString = _configManager.GetConfigValue("ConnectionString");

            services.AddDbContext<gravity_booksContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<BooksRepository>();


        }
    }

}
