namespace EmployeeManagement.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API V1");
                c.RoutePrefix = string.Empty; // Set Swagger UI as start page
            });

            return app;
        }
    }
}