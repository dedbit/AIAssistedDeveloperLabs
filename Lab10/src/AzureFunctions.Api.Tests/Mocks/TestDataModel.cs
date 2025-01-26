using System.Collections.Generic;
using System.IO;
using AzureFunctions.Api.Model;
using Newtonsoft.Json;

namespace AzureFunctions.Api.Tests.Mocks
{
    public class TestDataModel
    {
        public List<Project> Projects = new List<Project>();

        public TestDataModel()
        {
            string projects = File.ReadAllText("Samples\\Projects.json");
            Projects = JsonConvert.DeserializeObject<List<Project>>(projects);
        }

    }
}
