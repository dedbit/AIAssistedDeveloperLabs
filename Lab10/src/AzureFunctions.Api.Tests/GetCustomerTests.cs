using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctions.Api.BookModel;
using AzureFunctions.Api.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace AzureFunctions.Api.Tests
{
    public class GetCustomerTests
    {
        private readonly Mock<BooksRepository> _booksRepositoryMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly gravity_booksContext _contextMock;
        private ITestOutputHelper _output;

        public GetCustomerTests(ITestOutputHelper output)
        {
            _output = output;
            _loggerMock = new Mock<ILogger>();
            _contextMock = new Mock<gravity_booksContext>().Object;
            _booksRepositoryMock = new Mock<BooksRepository>(_contextMock);
        }

        [Fact]
        public async Task Run_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var httpRequestMock = new Mock<HttpRequest>();
            var logMock = _loggerMock.Object;
            var getCustomerFunction = new GetCustomers(_booksRepositoryMock.Object);

            // Act
            var result = await getCustomerFunction.Run(httpRequestMock.Object, logMock);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Run_CallsGetTopBooks()
        {
            // Arrange
            var httpRequestMock = new Mock<HttpRequest>();
            var logMock = _loggerMock.Object;
            var getCustomerFunction = new GetCustomers(_booksRepositoryMock.Object);

            // Act
            await getCustomerFunction.Run(httpRequestMock.Object, logMock);

            // Assert
            _booksRepositoryMock.Verify(repo => repo.GetTopCustomers(20), Times.Once);

        }

        [Fact]
        public async Task Run_ReturnsOkObjectResult_WithCustomerOutput()
        {
            // Arrange
            var customers = new List<BookModel.Customer>
            {
                new BookModel.Customer { FirstName = "John" },
                new BookModel.Customer { FirstName = "Jane" }
            };

            _booksRepositoryMock.Setup(repo => repo.GetTopCustomers(20)).Returns(customers);

            var httpRequestMock = new Mock<HttpRequest>();
            var logMock = new Mock<ILogger>();

            var function = new GetCustomers(_booksRepositoryMock.Object);

            // Act
            var result = await function.Run(httpRequestMock.Object, logMock.Object);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var customerOutput = (string[])okResult.Value;

            Assert.Equal(customers.Select(c => c.FirstName).ToArray(), customerOutput);
        }

        [Fact]
        public async Task Run_ReturnsNotFoundResult_WhenNoCustomerFound()
        {
            // Arrange
            var customers = new List<BookModel.Customer>();
            IEnumerable<BookModel.Customer> customerEnumerable = customers;

            _booksRepositoryMock.Setup(booksRepository => booksRepository.GetTopCustomers(20)).Returns(customerEnumerable);

            var httpRequestMock = new Mock<HttpRequest>();
            var logMock = new Mock<ILogger>();

            var function = new GetCustomers(_booksRepositoryMock.Object);

            // Act
            var result = await function.Run(httpRequestMock.Object, logMock.Object);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Run_ReturnsObjectResult_WithErrorMessage_WhenExceptionOccurs()
        {
            // Arrange
            _booksRepositoryMock.Setup(repo => repo.GetTopCustomers(20)).Throws(new Exception("Test exception"));

            var httpRequestMock = new Mock<HttpRequest>();
            var logMock = new Mock<ILogger>();

            var function = new GetCustomers(_booksRepositoryMock.Object);

            // Act
            var result = await function.Run(httpRequestMock.Object, logMock.Object);

            // Assert
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;
            Assert.Equal("Unexpected error", objectResult.Value);
        }

    }
}
