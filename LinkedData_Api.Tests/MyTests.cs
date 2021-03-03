using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LinkedData_Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace LinkedData_Api.Tests
{
    public class MyTests
    {
        private readonly HttpClient _client;

        public MyTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient();
        }

        [Theory]
        [InlineData("dbpedia")]
        [InlineData("virtuoso")]
        public async Task TestGraphs_ShouldReturnOk(string endpoint)
        {
            var response = await _client.GetAsync(ApiRoutes.EndpointGraphs.Replace("{endpoint}", endpoint));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Theory]
        [InlineData("_dbpedia")]
        [InlineData("virtuoso1")]
        public async Task TestGraphs_ShouldReturnNotFound(string endpoint)
        {
            var response = await _client.GetAsync(ApiRoutes.EndpointGraphs.Replace("{endpoint}", endpoint));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        //TODO: add more tests....
    }
}