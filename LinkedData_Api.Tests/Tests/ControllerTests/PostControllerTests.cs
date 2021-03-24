using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LinkedData_Api.Controllers;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Tests.Extensions;
using Newtonsoft.Json;
using Xunit;

namespace LinkedData_Api.Tests.Tests.ControllerTests
{
    public class PostControllerTests : ApiTests
    {

        private const string OkMockEndpointsConfigDirectory =
            @"..\..\..\MockData\OkMockEndpointsConfig";

        private const string FaultyMockEndpointsConfigDirectory =
            @"..\..\..\MockData\FaultyMockEndpointsConfig";

        #region GeneralInfoTests

        [Fact]
        public async Task TestConfigurationInfoEndpoint_ShouldReturnCreated()
        {
            string[] fileEntries = Directory.GetFiles(OkMockEndpointsConfigDirectory);
            List<HttpResponseMessage> responseMessages = new List<HttpResponseMessage>();
            foreach (string fileName in fileEntries)
            {
                var response = await TestClient.PostAsJsonAsync(
                    ApiRoutes.Endpoints,
                    JsonConvert.DeserializeObject(await File.ReadAllTextAsync(fileName)));
                responseMessages.Add(response);
            }

            foreach (var response in responseMessages)
            {
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }

        [Fact]
        public async Task TestConfigurationInfoEndpoint_ShouldReturnBadRequest()
        {
            string[] fileEntries = Directory.GetFiles(FaultyMockEndpointsConfigDirectory);
            List<HttpResponseMessage> responseMessages = new List<HttpResponseMessage>();
            foreach (string fileName in fileEntries)
            {
                var response = await TestClient.PostAsJsonAsync(
                    ApiRoutes.Endpoints,
                    JsonConvert.DeserializeObject(await File.ReadAllTextAsync(fileName)));
                responseMessages.Add(response);
            }

            foreach (var response in responseMessages)
            {
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        #endregion

        #region ResourceTests

        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "ex:uniqueResource")]
        public async Task TestPostResourceEndpoint_ShouldReturnCreated(string endpoint, string graph, string resourceCurie)
        {
            NamedResourceVm resourceVm = new NamedResourceVm()
            {
                ResourceCurie = resourceCurie,
                Predicates = new Dictionary<string, PredicateContent>()
                {
                    {
                        "ex:examplePredicate",
                        new PredicateContent()
                        {
                            Curies = new List<string>() {"ex:curie1", "ex:curie2", "ex:curie3"},
                            Literals = new List<Literal>()
                            {
                                new() {Value = "Example language text...", Language = "en"},
                                new() {Value = "2021-03-02T20:00:00-01:00", Datatype = "xsd:dateTime"},
                                new() {Value = "String value"}
                            }
                        }
                    }
                }
            };
            var response = await TestClient.PostAsJsonAsync(
                ApiRoutes.NamedGraphResources.Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph), resourceVm);
            ResourceVm createdResource = await TestClient.GetAsync(ApiRoutes.NamedGraphResourcesConcreteResource
                .Replace("{endpoint}", endpoint)
                .Replace("{graph}", graph)
                .Replace("{resource:regex(.+:.+)}", resourceCurie)).Result.Content.ReadAsAsync<ResourceVm>();
            Assert.Contains("ex:examplePredicate", createdResource.Predicates.Keys);
            Assert.True(createdResource.Predicates.Values.First().Curies.Count()==3);
            Assert.True(createdResource.Predicates.Values.First().Literals.Count()==3);
            //Possibly additional checks on returned object which should same as NamedResourceObjectAbove
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "", "", "notacurie", "")]
        [InlineData("virtuoso", "test", "ex:Exa:mpleResource", "ex:Exa:mplePredicate", "notacurie", "")]
        public async Task TestPostResourceEndpoint_ShouldReturnBadRequest(string endpoint, string graph,
            string resourceCurie, string predicateCurie, string innerCurie, string value)
        {
            NamedResourceVm resourceVm = new NamedResourceVm()
            {
                ResourceCurie = resourceCurie,
                Predicates = new Dictionary<string, PredicateContent>()
                {
                    {
                        predicateCurie,
                        new PredicateContent()
                        {
                            Curies = new List<string>() {innerCurie},
                            Literals = new List<Literal>()
                            {
                                new() {Value = value},
                            }
                        }
                    }
                }
            };
            var response = await TestClient.PostAsJsonAsync(
                ApiRoutes.NamedGraphResources.Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph), resourceVm);
            var validationErrorVm = await response.Content.ReadAsAsync<ValidationErrorVm>();
            Assert.Equal(4, validationErrorVm.ValidationErrors.Count);
            //Possibly additional checks about ValidationErrorVm
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region PredicateTests

        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "ex:Resource", "ex:uniquePredicate")]
        public async Task TestPostPredicateEndpoint_ShouldReturnCreated(string endpoint, string graph,
            string resourceCurie, string predicateCurie)
        {
            NamedPredicateVm resourceVm = new NamedPredicateVm()
            {
                PredicateCurie = predicateCurie,
                Curies = new List<string>()
                {
                    "ex:curie1", "ex:curie2", "ex:curie3"
                },
                Literals = new List<Literal>()
                {
                    new() {Value = "value1", Datatype = "ex:exampleDatatype"},
                    new() {Value = "value2", Language = "en"},
                    new() {Value = "value3"}
                }
            };

            
            var response = await TestClient.PostAsJsonAsync(
                ApiRoutes.NamedGraphResourcesConcreteResource.Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie), resourceVm);
            PredicateVm createdPredicate = await TestClient.GetAsync(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate
                .Replace("{endpoint}", endpoint)
                .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie)
                .Replace("{predicate:regex(.+:.+)}", predicateCurie)).Result.Content.ReadAsAsync<PredicateVm>();
            Assert.True(createdPredicate.Curies.Count == 3);
            Assert.True(createdPredicate.Literals.Count == 3);
            //Possibly additional checks about ValidationErrorVm
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        

        [SparqlSetupExtensions.IgnoreOnSparqlSetUpForUpdateTestFailed]
        [InlineData("virtuoso", "test", "ex:Resource", null, "notacurie", null)]
        [InlineData("virtuoso", "test", "ex:Resource", "ex:Exa:mplePredicate", "", "")]
        public async Task TestPostPredicateEndpoint_ShouldReturnBadRequest(string endpoint, string graph,
            string resourceCurie, string predicateCurie, string innerCurie, string value)
        {
            NamedPredicateVm resourceVm = new NamedPredicateVm()
            {
                PredicateCurie = predicateCurie,
                Curies = new List<string>() {innerCurie},
                Literals = new List<Literal>()
                {
                    new() {Value = value}
                }
            };
            var response = await TestClient.PostAsJsonAsync(
                ApiRoutes.NamedGraphResourcesConcreteResource.Replace("{endpoint}", endpoint)
                    .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resourceCurie), resourceVm);
            var validationErrorVm = await response.Content.ReadAsAsync<ValidationErrorVm>();
            Assert.Equal(3, validationErrorVm.ValidationErrors.Count);
            //Possibly additional checks about ValidationErrorVm
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion
    }
}
