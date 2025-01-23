using EmployeeManagement.API.Data;
using EmployeeManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace EmployeeManagement.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EmployeeDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;
        }

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Employee Management API", 
                    Version = "v1",
                    Description = "An API for managing employee information"
                });
            });

            return services;
        }

        public static IServiceCollection AddCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazor",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            return services;
        }
    }
}