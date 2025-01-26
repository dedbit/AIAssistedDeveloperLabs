using System.Threading.Tasks;
using AzureFunctions.Api.Clients;
using AzureFunctions.Api.Managers;
using AzureFunctions.Api.Model;
using AzureFunctions.Api.Repositories;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Xunit;

namespace AzureFunctions.Api.Tests.Repositories
{
    public class ProjectRepositoryTests
    {
        private readonly Mock<ConfigManager> _mockConfigManager;
        private readonly Mock<TableStorageClient> _mockTableStorageClient;
        private readonly ProjectRepository _projectRepository;

        public ProjectRepositoryTests()
        {
            _mockConfigManager = new Mock<ConfigManager>();
            _mockTableStorageClient = new Mock<TableStorageClient>("UseDevelopmentStorage=true", "projects");
            _projectRepository = new ProjectRepository(_mockConfigManager.Object, _mockTableStorageClient.Object);
        }

        [Fact]
        public async Task GetProject_ReturnsProject_WhenProjectExists()
        {
            // Arrange
            var projectId = "A01";
            var project = new Project { PartitionKey = "project", RowKey = projectId, Title = "Sample title", Name = "Sampleproject" };
            _mockTableStorageClient.Setup(client => client.Retrieve<Project>("project", projectId)).ReturnsAsync(project);

            // Act
            var result = await _projectRepository.GetProject(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectId, result.RowKey);
        }

        [Fact]
        public async Task GetProject_ReturnsNull_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = "A02";
            _mockTableStorageClient.Setup(client => client.Retrieve<Project>("project", projectId)).ReturnsAsync((Project)null);

            // Act
            var result = await _projectRepository.GetProject(projectId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateProject_InsertsProject()
        {
            // Arrange
            var project = new Project { PartitionKey = "project", RowKey = "A02", Title = "New title", Name = "Newproject" };
            _mockTableStorageClient.Setup(client => client.InsertOrMerge(project)).ReturnsAsync(new TableResult { Result = project });
            _mockTableStorageClient.Setup(client => client.Retrieve<Project>("project", project.RowKey)).ReturnsAsync(project);

            // Act
            var result = await _projectRepository.CreateProject(project);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.RowKey, result.RowKey);
        }

        [Fact]
        public async Task DeleteProject_DeletesProject_WhenProjectExists()
        {
            // Arrange
            var projectId = "A01";
            var project = new Project { PartitionKey = "project", RowKey = projectId, Title = "Sample title", Name = "Sampleproject" };
            _mockTableStorageClient.Setup(client => client.Retrieve<Project>("project", projectId)).ReturnsAsync(project);
            _mockTableStorageClient.Setup(client => client.Delete(project)).ReturnsAsync(new TableResult { Result = project });

            // Act
            await _projectRepository.DeleteProject(projectId);

            // Assert
            _mockTableStorageClient.Verify(client => client.Delete(project), Times.Once);
        }

        [Fact]
        public async Task DeleteProject_DoesNothing_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = "A02";
            _mockTableStorageClient.Setup(client => client.Retrieve<Project>("project", projectId)).ReturnsAsync((Project)null);

            // Act
            await _projectRepository.DeleteProject(projectId);

            // Assert
            _mockTableStorageClient.Verify(client => client.Delete(It.IsAny<Project>()), Times.Never);
        }
    }
}
