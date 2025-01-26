using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctions.Api.Managers;
using AzureFunctions.Api.Model;
using AzureFunctions.Api.Repositories;

namespace AzureFunctions.Api.Tests.Mocks
{
    public class ProjectRepositoryFake : ProjectRepository
    {
        private readonly TestDataModel _testDataModel;

        public ProjectRepositoryFake(ConfigManager configManager, TestDataModel testDataModel)
        {
            _testDataModel = testDataModel;
        }

        public override async Task<Project> GetProject(string projectId)
        {
            List<Project> projects = _testDataModel.Projects.Where(p => p.RowKey == projectId).ToList();
            if (projects.Count == 1)
            {
                return projects.First();
            }

            return null;
        }

        public override async Task<Project> CreateProject(Project proj)
        {
            _testDataModel.Projects.Add(proj);
            return proj;

        }

        public override async Task DeleteProject(string rowKey)
        {
            var project = await GetProject(rowKey);
            _testDataModel.Projects.Remove(project);

        }
    }
}
