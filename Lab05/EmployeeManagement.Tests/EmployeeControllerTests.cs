using EmployeeManagement.API.Controllers;
using EmployeeManagement.API.Data;
using EmployeeManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class EmployeeControllerTests
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
        public async Task CreateEmployee_ValidData_ReturnsCreatedResponse()
        {
            // Arrange
            var context = await GetDatabaseContext();
            var repository = new EmployeeRepository(context);
            var controller = new EmployeesController(repository);
            var employee = new Employee
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Department = "HR",
                DateOfBirth = new DateTime(1992, 5, 15),
                Salary = 60000
            };

            // Act
            var result = await controller.CreateEmployee(employee);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdEmployee = Assert.IsType<Employee>(createdResult.Value);
            Assert.Equal(employee.Email, createdEmployee.Email);
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsAllEmployees()
        {
            // Arrange
            var context = await GetDatabaseContext();
            var repository = new EmployeeRepository(context);
            var controller = new EmployeesController(repository);
            await repository.AddAsync(new Employee
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Department = "IT",
                DateOfBirth = new DateTime(1990, 1, 1),
                Salary = 55000
            });

            // Act
            var result = await controller.GetEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var employees = Assert.IsAssignableFrom<IEnumerable<Employee>>(okResult.Value);
            Assert.Single(employees);
        }

        [Fact]
        public async Task GetById_ExistingEmployee_ReturnsEmployee()
        {
            // Arrange
            var context = await GetDatabaseContext();
            var repository = new EmployeeRepository(context);
            var controller = new EmployeesController(repository);
            var employee = await repository.AddAsync(new Employee
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Department = "IT",
                DateOfBirth = new DateTime(1990, 1, 1),
                Salary = 55000
            });

            // Act
            var result = await controller.GetEmployee(employee.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEmployee = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(employee.Id, returnedEmployee.Id);
        }

        [Fact]
        public async Task Delete_ExistingEmployee_ReturnsNoContent()
        {
            // Arrange
            var context = await GetDatabaseContext();
            var repository = new EmployeeRepository(context);
            var controller = new EmployeesController(repository);
            var employee = await repository.AddAsync(new Employee
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Department = "IT",
                DateOfBirth = new DateTime(1990, 1, 1),
                Salary = 55000
            });

            // Act
            var result = await controller.DeleteEmployee(employee.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExistentEmployee_ReturnsNotFound()
        {
            // Arrange
            var context = await GetDatabaseContext();
            var repository = new EmployeeRepository(context);
            var controller = new EmployeesController(repository);

            // Act
            var result = await controller.DeleteEmployee(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
