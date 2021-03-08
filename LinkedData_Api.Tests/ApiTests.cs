using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LinkedData_Api.Controllers;
using LinkedData_Api.Data;
using LinkedData_Api.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace LinkedData_Api.Tests
{
    public class ApiTests
    {
        protected readonly HttpClient TestClient;

        public ApiTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            TestClient = appFactory.CreateClient();
            SetupMockDataAcces(appFactory);
        }

        private void SetupMockDataAcces(WebApplicationFactory<Startup> appFactory)
        {
            var dataAccess = (IDataAccess) appFactory.Services.GetService(typeof(IDataAccess));
            dataAccess?.LoadConfigurationFiles(@"..\..\..\MockData\EndpointConfigurations");
            dataAccess?.LoadNamespacesFile(@"..\..\..\MockData\Namespaces\namespaces.json");
        }
    }
}