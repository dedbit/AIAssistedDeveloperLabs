using EmployeeManagement.API.Data;
using EmployeeManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class EmployeeRepositoryTests
    {
        private async Task<EmployeeDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new EmployeeDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task AddEmployee_ShouldReturnEmployeeWithId()
        {
            // Arrange
            var context = await GetDatabaseContext();
            var repository = new EmployeeRepository(context);
            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Department = "IT",
                DateOfBirth = new DateTime(1990, 1, 1),
                Salary = 50000
            };

            // Act
            var result = await repository.AddAsync(employee);

            // Assert
            Assert.NotEqual(0, result.Id);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task GetAllEmployees_ShouldReturnAllEmployees()
        {
            // Arrange
            var context = await GetDatabaseContext();
            var repository = new EmployeeRepository(context);
            await repository.AddAsync(new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Department = "IT",
                DateOfBirth = new DateTime(1990, 1, 1),
                Salary = 50000
            });

            // Act
            var employees = await repository.GetAllAsync();

            // Assert
            Assert.Single(employees);
        }


        
    }
}