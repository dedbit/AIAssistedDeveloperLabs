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
using Moq.EntityFrameworkCore;

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
            var customerOutput = (BookModel.Customer[])okResult.Value;

            Assert.Equal(2, customerOutput.Length);
            Assert.Equal("John", customerOutput[0].FirstName);
            Assert.Equal("Jane", customerOutput[1].FirstName);
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


        [Fact]
        public void ComplexQuery2_ReturnsExpectedResult()
        {
            // Arrange
            List<Book> books = new List<Book>
            {
                new Book { BookId = 1, Title = "Book1", PublisherId = 1, LanguageId = 1 },
                new Book { BookId = 2, Title = "Book2", PublisherId = 2, LanguageId = 2 }
            };

            List<BookAuthor> bookAuthors = new List<BookAuthor>
            {
                new BookAuthor { BookId = 1, AuthorId = 1 },
                new BookAuthor { BookId = 2, AuthorId = 2 }
            };

            List<Author> authors = new List<Author>
            {
                new Author { AuthorId = 1, AuthorName = "Author1" },
                new Author { AuthorId = 2, AuthorName = "Author2" }
            };

            List<Publisher> publishers = new List<Publisher>
            {
                new Publisher { PublisherId = 1, PublisherName = "Specific Publisher" },
                new Publisher { PublisherId = 2, PublisherName = "Other Publisher" }
            };

            List<BookLanguage> languages = new List<BookLanguage>
            {
                new BookLanguage { LanguageId = 1, LanguageName = "English" },
                new BookLanguage { LanguageId = 2, LanguageName = "French" }
            };

            Mock<gravity_booksContext> contextMock = new Mock<gravity_booksContext>();
            contextMock.Setup(c => c.Books).ReturnsDbSet(books);
            contextMock.Setup(c => c.BookAuthors).ReturnsDbSet(bookAuthors);
            contextMock.Setup(c => c.Authors).ReturnsDbSet(authors);
            contextMock.Setup(c => c.Publishers).ReturnsDbSet(publishers);
            contextMock.Setup(c => c.BookLanguages).ReturnsDbSet(languages);

            BooksRepository repository = new BooksRepository(contextMock.Object);

            // Act
            repository.ComplexQuery();

            // Assert
            // Add assertions to verify the expected result
        }
    }
}
