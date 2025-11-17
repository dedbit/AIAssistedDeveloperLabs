using EmployeeManagement.API.Data;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task GetCompensationAnalysisAsync_ReturnsCorrectAnalysis()
        {
            // Arrange
            var context = await GetDatabaseContext();

            // Seed data: 4 employees in 2 departments with varying salaries
            context.Employees.AddRange(
                new Employee { Id = 1, Department = "HR", Salary = 45000 },
                new Employee { Id = 2, Department = "HR", Salary = 55000 },
                new Employee { Id = 3, Department = "IT", Salary = 75000 },
                new Employee { Id = 4, Department = "IT", Salary = 85000 }
            );
            await context.SaveChangesAsync();

            var repository = new EmployeeRepository(context); // Assuming the repository contains GetCompensationAnalysisAsync
            var bucketSize = 10000; // Define bucket size
            var minEmployeeCount = 1; // Minimum employees per bucket

            // Act
            var result = await repository.GetCompensationAnalysisAsync(bucketSize, minEmployeeCount);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            var resultList = result.ToList(); // Convert to a list for easier validation

            // Validate the number of items in the result
            Assert.Equal(4, resultList.Count); // Expecting 4 salary buckets

            // Validate each item in the result
            Assert.Collection(resultList,
                item =>
                {
                    Assert.Equal("HR", item.Department);
                    Assert.Equal(40000, item.SalaryRangeMin);
                    Assert.Equal(50000, item.SalaryRangeMax);
                    Assert.Equal(1, item.EmployeeCount);
                    Assert.Equal(45000, item.AverageSalary);
                    Assert.Equal(50, item.PercentageOfDepartment);
                },
                item =>
                {
                    Assert.Equal("HR", item.Department);
                    Assert.Equal(50000, item.SalaryRangeMin);
                    Assert.Equal(60000, item.SalaryRangeMax);
                    Assert.Equal(1, item.EmployeeCount);
                    Assert.Equal(55000, item.AverageSalary);
                    Assert.Equal(50, item.PercentageOfDepartment);
                },
                item =>
                {
                    Assert.Equal("IT", item.Department);
                    Assert.Equal(70000, item.SalaryRangeMin);
                    Assert.Equal(80000, item.SalaryRangeMax);
                    Assert.Equal(1, item.EmployeeCount);
                    Assert.Equal(75000, item.AverageSalary);
                    Assert.Equal(50, item.PercentageOfDepartment);
                },
                item =>
                {
                    Assert.Equal("IT", item.Department);
                    Assert.Equal(80000, item.SalaryRangeMin);
                    Assert.Equal(90000, item.SalaryRangeMax);
                    Assert.Equal(1, item.EmployeeCount);
                    Assert.Equal(85000, item.AverageSalary);
                    Assert.Equal(50, item.PercentageOfDepartment);
                });
        }


    }
}