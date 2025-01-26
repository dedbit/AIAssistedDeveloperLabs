using AzureFunctions.Api.Managers;
using AzureFunctions.Api.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using AzureFunctions.Api.Helpers;
using AzureFunctions.Api.Repositories;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace AzureFunctions.Api.Tests
{
    public class FunctionsTests
    {
        private readonly ConfigManager _configManager;
        MockLogger<FunctionsTests> _mockLogger;
        readonly FunctionHelper _functionHelper;
        private readonly ProjectRepository _projectRepository;
        private IServiceProvider provider;

        public FunctionsTests(ITestOutputHelper output, ConfigManager configManager, FunctionHelper functionHelper, ProjectRepository projectRepository, IServiceProvider provider)
        {
            _mockLogger = new MockLogger<FunctionsTests>(output);
            _configManager = configManager;
            _functionHelper = functionHelper;
            _projectRepository = projectRepository;
            this.provider = provider;
        }

        [Fact]
        public void HttpTriggerTest()
        {
            _mockLogger.LogInformation("HttpTriggerTest started");

            var query = new Dictionary<String, StringValues>();
            var header = new Dictionary<String, StringValues>();
            string body = "";

            var req = HttpMock.HttpRequestSetup(header, query, body);

            var httpTriggerFunc = new AzureFunctions.Api.Functions.HttpTrigger(_configManager);

            var result = httpTriggerFunc.Run(req, _mockLogger).Result;

            OkObjectResult res = (OkObjectResult)result;

            string outputStr = res.Value.ToString();
            Assert.False(string.IsNullOrEmpty(res.Value.ToString()));
            Assert.Contains("Success", outputStr);


        }

        [Fact]
        public void GetProjectTest()
        {
            _mockLogger.LogInformation("GetProjectTest started");

            var query = new Dictionary<String, StringValues>();
            var header = new Dictionary<String, StringValues>();
            string body = File.ReadAllText("Samples\\ProjectGetRequest.json");

            var req = HttpMock.HttpRequestSetup(header, query, body);

            var func = new AzureFunctions.Api.Functions.GetProject(_configManager, _functionHelper, _projectRepository);

            var result = func.Run(req, _mockLogger).Result;

            OkObjectResult res = (OkObjectResult)result;

            string outputStr = JsonConvert.SerializeObject(res.Value);
            Assert.False(string.IsNullOrEmpty(outputStr));
            Assert.Contains("Skagens perle", outputStr);

        }

    }
}
