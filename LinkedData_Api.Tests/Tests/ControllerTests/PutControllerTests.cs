using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LinkedData_Api.Controllers;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Tests.Extensions;
using Xunit;

namespace LinkedData_Api.Tests.Tests.ControllerTests
{
    public class PutControllerTests : ApiTests
    {
        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "ex:uniquePutResource1")]
        public async Task TestPutResourceEndpoint_ShouldReturnCreated(string endpoint, string graph,
            string resourceCurie)
        {
            ResourceVm resourceVm = new ResourceVm()
            {
                Predicates = new Dictionary<string, PredicateContent>()
                {
                    {
                        "ex:newProperty",
                        new PredicateContent()
                        {
                            Curies = new List<string>() {"ex:newCurie"},
                        }
                    }
                }
            };
            //Resource exists
            var responseOnGetNotYetModifiedResource = await TestClient.GetAsync(ApiRoutes
                .NamedGraphResourcesConcreteResource
                .Replace("{endpoint}", endpoint)
                .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie));
            Assert.Equal(HttpStatusCode.OK, responseOnGetNotYetModifiedResource.StatusCode);

            //Resource modified
            var response =
                await TestClient.PutAsJsonAsync(ApiRoutes.NamedGraphResourcesConcreteResource
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie), resourceVm);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            //Modified resource confirmation
            var recreatedResource = await TestClient.GetAsync(ApiRoutes.NamedGraphResourcesConcreteResource
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)).Result.Content
                .ReadAsAsync<ResourceVm>();
            Assert.True(recreatedResource.Predicates.ContainsKey("ex:newProperty"));
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "ex:uniquePutResource1", "notacurieProperty")]
        [InlineData("virtuoso", "test", "ex:uniquePutResource1", "")]
        public async Task TestPutResourceEndpoint_ShouldRBadRequest(string endpoint, string graph,
            string resourceCurie, string predicateCurie)
        {
            ResourceVm resourceVm = new ResourceVm()
            {
                Predicates = new Dictionary<string, PredicateContent>()
                {
                    {
                        predicateCurie,
                        new PredicateContent()
                        {
                            Curies = new List<string>() {"ex:newCurie"},
                        }
                    }
                }
            };
            var response =
                await TestClient.PutAsJsonAsync(ApiRoutes.NamedGraphResourcesConcreteResource
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie), resourceVm);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "ex:uniquePutResource2", "ex:uniquePutPredicate2")]
        public async Task TestPutPredicateEndpoint_ShouldReturnCreated(string endpoint, string graph,
            string resourceCurie, string predicateCurie)
        {
            PredicateVm predicateVm = new PredicateVm()
            {
                Curies = new List<string>()
                {
                    "ex:newCurie"
                }
            };

            //Predicate exists
            var responseOnGetNotYetModifiedPredicate = await TestClient.GetAsync(ApiRoutes
                .NamedGraphResourceStartConcreteResourcePredicate
                .Replace("{endpoint}", endpoint)
                .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)
                .Replace("{predicate:regex(.+:.+)}", predicateCurie));
            Assert.Equal(HttpStatusCode.OK, responseOnGetNotYetModifiedPredicate.StatusCode);

            //Same resource different predicate
            var responseOnGetSameResourceDifferenctPredicate = await TestClient.GetAsync(ApiRoutes
                .NamedGraphResourceStartConcreteResourcePredicate
                .Replace("{endpoint}", endpoint)
                .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)
                .Replace("{predicate:regex(.+:.+)}", "ex:uniquePutPredicate3"));
            Assert.Equal(HttpStatusCode.OK, responseOnGetSameResourceDifferenctPredicate.StatusCode);

            //Predicate modified
            var response =
                await TestClient.PutAsJsonAsync(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)
                    .Replace("{predicate:regex(.+:.+)}", predicateCurie), predicateVm);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            //Modified predicate confirmation
            var recreatedResource = await TestClient.GetAsync(ApiRoutes
                    .NamedGraphResourcesConcreteResource
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)).Result.Content
                .ReadAsAsync<ResourceVm>();

            //Predicate was recreated to desired value
            Assert.Contains("ex:newCurie",
                recreatedResource.Predicates.First(keyValuePair => keyValuePair.Key.Equals(predicateCurie)).Value.Curies
                    .First());

            //Same resource different predicate still exists
            Assert.True(recreatedResource.Predicates.ContainsKey("ex:uniquePutPredicate3"));
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "ex:uniquePutResource2", "ex:uniquePutPredicate2", "notacurie", "ok")]
        [InlineData("virtuoso", "test", "ex:uniquePutResource2", "ex:uniquePutPredicate2", "ex:curie", "")]
        public async Task TestPutPredicateEndpoint_ShouldReturnBadRequest(string endpoint, string graph,
            string resourceCurie, string predicateCurie, string innerCurie, string value)
        {
            PredicateVm predicateVm = new PredicateVm()
            {
                Curies = new List<string>()
                {
                    innerCurie
                },
                Literals = new List<Literal>()
                {
                    new() {Value = value}
                }
            };

            var response =
                await TestClient.PutAsJsonAsync(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate
                    .Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)
                    .Replace("{predicate:regex(.+:.+)}", predicateCurie), predicateVm);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}