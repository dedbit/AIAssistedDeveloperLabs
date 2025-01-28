using System;
using System.Threading.Tasks;
using AzureFunctions.Api.Clients;
using AzureFunctions.Api.Managers;
using AzureFunctions.Api.Model;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureFunctions.Api.Repositories
{
    public class ProjectRepository
    {
        private readonly ConfigManager _configManager;
        private readonly TableStorageClient _projectTableClient;
        private readonly string projectPartitionKey = "project";

        public ProjectRepository()
        {

        }

        public ProjectRepository(ConfigManager configManager)
        {
            _configManager = configManager;
            string storageConnectionString = _configManager.GetConfigValue("AzureWebJobsStorage");
            _projectTableClient = new TableStorageClient(storageConnectionString, "Projects");

            SeedData().Wait();
        }

        public ProjectRepository(ConfigManager configManager, TableStorageClient tableStorageClient)
        {
            _configManager = configManager;
            _projectTableClient = tableStorageClient ?? throw new ArgumentNullException(nameof(tableStorageClient));

            SeedData().Wait();
        }

        private async Task SeedData()
        {
            if (await GetProject("A01") == null)
            {
                await CreateProject(new Project() { PartitionKey = projectPartitionKey, RowKey = "A01", Title = "Sample title", Name = "Sampleproject"});
            }

        }

        public virtual async Task<Project> GetProject(string projectId)
        {
            Project proj1 = await _projectTableClient.Retrieve<Project>(projectPartitionKey, projectId);
            return proj1;
        }

        public virtual async Task<Project> CreateProject(Project proj)
        {
            TableResult insertResult = await _projectTableClient.InsertOrMerge(proj);
            Project project = await GetProject(proj.RowKey);
            return project;
        }

        public virtual async Task DeleteProject(string rowKey)
        {
            Project project = await GetProject(rowKey);
            
            if (project == null)
            {
                return;
            }

            var deleteResult = await _projectTableClient.Delete(project);


        }

    }
}
