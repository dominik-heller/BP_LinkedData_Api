/*using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LinkedData_Api.Controllers;
using Xunit;

namespace LinkedData_Api.Tests
{
    public class PostControllerTests : ApiTests
    {
         #region GeneralEndpointsTests

        [Theory]
        [InlineData("test")]
        public async Task TestConfigurationInfoEndpoint_ShouldReturnCreated(string endpoint)
        {
            var response = await TestClient.PostAsync(ApiRoutes.EndpointConfiguration.Replace("{endpoint}", endpoint), new MultipartContent());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("_dbpedia")]
        [InlineData("@đ[dfa")]
        public async Task TestConfigurationInfoEndpoint_ShouldReturnNotFound(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointConfiguration.Replace("{endpoint}", endpoint));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("dbpedia")]
        [InlineData("virtuoso")]
        public async Task TestGraphsEndpoint_ShouldReturnOk(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointGraphs.Replace("{endpoint}", endpoint));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("_dbpedia")]
        [InlineData("@đ[dfa")]
        public async Task TestGraphsEndpoint_ShouldReturnNotFound(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointGraphs.Replace("{endpoint}", endpoint));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("rdf")]
        [InlineData("owl")]
        public async Task TestNamespaceEndpoint_ShouldReturnOk(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointNamespacePrefix.Replace("{prefix}", endpoint));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("_dbpedia")]
        [InlineData("@đ[dfa")]
        public async Task TestNamespaceEndpoint_ShouldReturnNotFound(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointNamespacePrefix.Replace("{prefix}", endpoint));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion
    }
}*/

