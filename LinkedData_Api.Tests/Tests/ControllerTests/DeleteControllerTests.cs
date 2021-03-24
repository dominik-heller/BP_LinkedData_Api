using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LinkedData_Api.Controllers;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Tests.Extensions;
using Xunit;

namespace LinkedData_Api.Tests.Tests.ControllerTests
{
    public class DeleteControllerTests : ApiTests
    {
        [Theory]
        [InlineData("datagov")]
        public async Task TestConfigurationInfoEndpoint_ShouldReturnOk(string endpoint)
        {
            var response =
                await TestClient.DeleteAsync(ApiRoutes.EndpointConfiguration.Replace("{endpoint}", endpoint));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "ex:uniqueDelResource1")]
        public async Task TestDeleteResourceEndpoint_ShouldReturnNoContent(string endpoint, string graph,
            string resourceCurie)
        {
            //Resource exists
            var responseOnGetNotYesDeletedResource = await TestClient.GetAsync(ApiRoutes
                .NamedGraphResourcesConcreteResource
                .Replace("{endpoint}", endpoint)
                .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie));
            Assert.Equal(HttpStatusCode.OK, responseOnGetNotYesDeletedResource.StatusCode);

            //Resource deleted
            var response =
                await TestClient.DeleteAsync(ApiRoutes.NamedGraphResourcesConcreteResource
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            //Resource not found
            var responseOnGetDeletedResource = await TestClient.GetAsync(ApiRoutes.NamedGraphResourcesConcreteResource
                .Replace("{endpoint}", endpoint)
                .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie));
            Assert.Equal(HttpStatusCode.NotFound, responseOnGetDeletedResource.StatusCode);
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "invalidnamespace:predicate")]
        public async Task TestDeleteResourceEndpoint_ShouldReturnNotFound(string endpoint, string graph,
            string resourceCurie)
        {
            var response =
                await TestClient.DeleteAsync(ApiRoutes.NamedGraphResourcesConcreteResource
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie));
            var errorVm = await response.Content.ReadAsAsync<CustomErrorVm>();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(!string.IsNullOrEmpty(errorVm.CustomErrorMessage));
        }


        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "ex:uniqueDelResource2", "ex:uniqueDelPredicate2")]
        public async Task TestDeletePredicateEndpoint_ShouldReturnNoContent(string endpoint, string graph,
            string resourceCurie, string predicateCurie)
        {
            //Predicate exists
            var responseOnGetNotYesDeletedPredicate = await TestClient.GetAsync(ApiRoutes
                .NamedGraphResourceStartConcreteResourcePredicate
                .Replace("{endpoint}", endpoint)
                .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)
                .Replace("{predicate:regex(.+:.+)}", predicateCurie));
            Assert.Equal(HttpStatusCode.OK, responseOnGetNotYesDeletedPredicate.StatusCode);

            //Delete predicate
            var response =
                await TestClient.DeleteAsync(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)
                    .Replace("{predicate:regex(.+:.+)}", predicateCurie));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            //Get resource which used to have the predicate => check if it does not anymore
            var resourceVm = await TestClient.GetAsync(ApiRoutes.NamedGraphResourcesConcreteResource
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)).Result.Content
                .ReadAsAsync<ResourceVm>();
            Assert.True(!resourceVm.Predicates.ContainsKey(predicateCurie));
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "ex:uniqueDelResource2", "invalidnamespace:predicate")]
        public async Task TestDeletePredicateEndpoint_ShouldReturnNotFound(string endpoint, string graph,
            string resourceCurie, string predicateCurie)
        {
            var response =
                await TestClient.DeleteAsync(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)
                    .Replace("{predicate:regex(.+:.+)}", predicateCurie));
            var errorVm = await response.Content.ReadAsAsync<CustomErrorVm>();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(!string.IsNullOrEmpty(errorVm.CustomErrorMessage));
        }
    }
}