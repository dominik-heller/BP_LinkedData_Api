using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LinkedData_Api.Tests.Tests.ControllerTests
{
    public class ApiTests
    {
        protected readonly HttpClient TestClient;

        protected ApiTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            TestClient = appFactory.CreateClient();
        }
    }
}