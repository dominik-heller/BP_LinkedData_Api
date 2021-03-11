using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LinkedData_Api.Tests.Tests
{
    public class ApiTests
    {
        protected readonly HttpClient TestClient;

        public ApiTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            TestClient = appFactory.CreateClient();
        }
    }
}