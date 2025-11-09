using EmployeeManagement.API.Data;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
        [Fact]
        public async Task GetDepartmentStats_NoFilters_ReturnsAggregates()
        {
            // Arrange
            var context = await GetDatabaseContext();
            var repository = new EmployeeRepository(context);
            await repository.AddAsync(new Employee { FirstName = "Alice", LastName = "A", Email = "alice@example.com", Department = "IT", DateOfBirth = new DateTime(1990,1,1), Salary = 80000 });
            await repository.AddAsync(new Employee { FirstName = "Bob", LastName = "B", Email = "bob@example.com", Department = "IT", DateOfBirth = new DateTime(1991,1,1), Salary = 60000 });
            await repository.AddAsync(new Employee { FirstName = "Carol", LastName = "C", Email = "carol@example.com", Department = "HR", DateOfBirth = new DateTime(1992,1,1), Salary = 50000 });

            // Act
            var stats = (await repository.GetDepartmentStatsAsync()).ToList();

            // Assert
            Assert.Equal(2, stats.Count);
            var it = stats.First(s => s.Department == "IT");
            Assert.Equal(2, it.EmployeeCount);
            Assert.Equal(140000, it.TotalSalary);
            Assert.Equal(80000, it.MaxSalary);
            Assert.Equal("Alice", it.TopEarnerFirstName);
        }

        [Fact]
        public async Task GetDepartmentStats_WithFilters_FiltersProperly()
        {
            // Arrange
            var context = await GetDatabaseContext();
            var repository = new EmployeeRepository(context);
            await repository.AddAsync(new Employee { FirstName = "Zara", LastName = "Z", Email = "zara@example.com", Department = "Sales", DateOfBirth = new DateTime(1990,1,1), Salary = 90000 });
            await repository.AddAsync(new Employee { FirstName = "Yanni", LastName = "Y", Email = "yanni@example.com", Department = "Sales", DateOfBirth = new DateTime(1991,1,1), Salary = 45000 });
            await repository.AddAsync(new Employee { FirstName = "Xena", LastName = "X", Email = "xena@example.com", Department = "Marketing", DateOfBirth = new DateTime(1992,1,1), Salary = 70000 });

            // Act - minSalary filter should exclude Yanni; search term 'sale' should match Sales dept only
            var stats = (await repository.GetDepartmentStatsAsync(minSalary: 50000, search: "sale")).ToList();

            // Assert
            Assert.Single(stats);
            var sales = stats[0];
            Assert.Equal("Sales", sales.Department);
            Assert.Equal(1, sales.EmployeeCount);
            Assert.Equal(90000, sales.TotalSalary);
            Assert.Equal("Zara", sales.TopEarnerFirstName);
        }
    }
}