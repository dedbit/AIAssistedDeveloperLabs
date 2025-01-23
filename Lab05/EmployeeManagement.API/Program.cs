using EmployeeManagement.API.Extensions;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddCorsServices();

var app = builder.Build();

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazor");
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { } // Required for testing