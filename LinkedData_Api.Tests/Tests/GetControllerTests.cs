using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LinkedData_Api.Controllers;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Tests.Extensions;
using Xunit;

namespace LinkedData_Api.Tests.Tests
{
    public class GetControllerTests : ApiTests
    {
        
        private const string LimitQuery = "?limit=5";

        #region GeneralEndpointsTests

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("dbpedia")]
        [InlineData("virtuoso")]
        public async Task TestConfigurationInfoEndpoint_ShouldReturnOk(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointConfiguration.Replace("{endpoint}", endpoint));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("_dbpedia")]
        [InlineData("@đ[dfa")]
        public async Task TestConfigurationInfoEndpoint_ShouldReturnNotFound(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointConfiguration.Replace("{endpoint}", endpoint));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("dbpedia")]
        [InlineData("virtuoso")]
        public async Task TestGraphsEndpoint_ShouldReturnOk(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointGraphs.Replace("{endpoint}", endpoint));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("_dbpedia")]
        [InlineData("@đ[dfa")]
        public async Task TestGraphsEndpoint_ShouldReturnNotFound(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointGraphs.Replace("{endpoint}", endpoint));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("rdf")]
        [InlineData("owl")]
        public async Task TestNamespaceEndpoint_ShouldReturnOk(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointNamespacePrefix.Replace("{prefix}", endpoint));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("_dbpedia")]
        [InlineData("@đ[dfa")]
        public async Task TestNamespaceEndpoint_ShouldReturnNotFound(string endpoint)
        {
            var response = await TestClient.GetAsync(ApiRoutes.EndpointNamespacePrefix.Replace("{prefix}", endpoint));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region ClassesEndpointsTests

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("dbpedia", "virtuoso", "ovm")]
        [InlineData("datagov", "virtuoso", "lexvo")]
        public async Task TestClassesEndpoint_ShouldReturnOkWithNotEmptyCurieVm(string endpointWithoutGraph,
            string endpointWithGraph, string graph)
        {
            var responses = new List<HttpResponseMessage>();
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphClasses.Replace("{endpoint}", endpointWithoutGraph) +
                                          LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphClasses.Replace("{endpoint}", endpointWithGraph)
                .Replace("{graph}", graph) + LimitQuery));
            responses.ForEach(x => x.EnsureSuccessStatusCode());
            var responseBodies = new List<CurieVm>();
            responses.ForEach(async x => responseBodies.Add(await x.Content.ReadAsAsync<CurieVm>()));
            responses.ForEach(x => Assert.Equal(HttpStatusCode.OK, x.StatusCode));
            responseBodies.ForEach(x => Assert.True(x != null && x.Curies.Count > 0));
            responses.ForEach(x =>
            {
                if (x.Content.Headers.ContentType != null)
                    Assert.Equal("application/json", x.Content.Headers.ContentType.MediaType);
            });
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("wrongEndpoint1", "virtuoso", "wrongGraph")]
        [InlineData("wrongEndpoint2", "datagov", "school")] //datagov does not have classesentry defined => false
        [InlineData("wrongEndpoint3", "datagov", null)]
        public async Task TestClassesEndpoint_ShouldReturnNotFound(string endpointWithoutGraph,
            string endpointWithGraph, string graph)
        {
            var responses = new List<HttpResponseMessage>();
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphClasses.Replace("{endpoint}", endpointWithoutGraph) +
                                          LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphClasses.Replace("{endpoint}", endpointWithGraph)
                .Replace("{graph}", graph) + LimitQuery));
            responses.ForEach(x => Assert.Equal(HttpStatusCode.NotFound, x.StatusCode));
        }

        #endregion

        #region ResourcesEndpointsTests

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("dbpedia", "virtuoso", "ovm")]
        public async Task TestResourcesEndpoint_ShouldReturnOkWithNotEmptyCurieVm(string endpointWithoutGraph,
            string endpointWithGraph, string graph)
        {
            var responses = new List<HttpResponseMessage>();
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphResources.Replace("{endpoint}", endpointWithoutGraph) +
                                          LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphResources
                .Replace("{endpoint}", endpointWithGraph)
                .Replace("{graph}", graph) + LimitQuery));
            responses.ForEach(x => x.EnsureSuccessStatusCode());
            var responseBodies = new List<CurieVm>();
            responses.ForEach(async x => responseBodies.Add(await x.Content.ReadAsAsync<CurieVm>()));
            responses.ForEach(x => Assert.Equal(HttpStatusCode.OK, x.StatusCode));
            responseBodies.ForEach(x => Assert.True(x != null && x.Curies.Count > 0));
            responses.ForEach(x =>
            {
                if (x.Content.Headers.ContentType != null)
                    Assert.Equal("application/json", x.Content.Headers.ContentType.MediaType);
            });
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("wrongEndpoint1", "virtuoso", "wrongGraph")]
        [InlineData("wrongEndpoint2", "datagov", null)]
        public async Task TestResourcesEndpoint_ShouldReturnNotFound(string endpointWithoutGraph,
            string endpointWithGraph, string graph)
        {
            var responses = new List<HttpResponseMessage>();
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphResources.Replace("{endpoint}", endpointWithoutGraph) +
                                          LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphResources
                .Replace("{endpoint}", endpointWithGraph)
                .Replace("{graph}", graph) + LimitQuery));
            responses.ForEach(x => Assert.Equal(HttpStatusCode.NotFound, x.StatusCode));
        }

        #endregion

        #region ConcreteClassEndpointTests

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("virtuoso", "virtuoso", "ovm", "gr:BusinessEntity")]
        public async Task TestConcreteClassEndpoint_ShouldReturnOkWithNotEmptyCurieVm(string endpointWithoutGraph,
            string endpointWithGraph, string graph, string classId)
        {
            var responses = new List<HttpResponseMessage>();
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphConcreteClass
                    .Replace("{endpoint}", endpointWithoutGraph).Replace("{class}", classId) + LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphConcreteClass
                .Replace("{endpoint}", endpointWithGraph)
                .Replace("{graph}", graph).Replace("{class}", classId) + LimitQuery));
            responses.ForEach(x => x.EnsureSuccessStatusCode());
            var responseBodies = new List<CurieVm>();
            responses.ForEach(async x => responseBodies.Add(await x.Content.ReadAsAsync<CurieVm>()));
            responses.ForEach(x => Assert.Equal(HttpStatusCode.OK, x.StatusCode));
            responseBodies.ForEach(x => Assert.True(x != null && x.Curies.Count > 0));
            responses.ForEach(x =>
            {
                if (x.Content.Headers.ContentType != null)
                    Assert.Equal("application/json", x.Content.Headers.ContentType.MediaType);
            });
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("wrongEndpoint1", "virtuoso", "wrongGraph", "gr:BusinessEntity")]
        [InlineData("wrongEndpoint1", "virtuoso", "ovm", "gr:wrongClass")]
        [InlineData("wrongEndpoint2", "datagov", null, null)]
        public async Task TestConcreteClassEndpoint_ShouldReturnNotFound(string endpointWithoutGraph,
            string endpointWithGraph, string graph, string classId)
        {
            var responses = new List<HttpResponseMessage>();
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphConcreteClass
                    .Replace("{endpoint}", endpointWithoutGraph).Replace("{class}", classId) + LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphConcreteClass.Replace("{graph}", graph)
                .Replace("{endpoint}", endpointWithGraph).Replace("{class}", classId) + LimitQuery));
            responses.ForEach(x => Assert.Equal(HttpStatusCode.NotFound, x.StatusCode));
        }

        #endregion

        #region ConcreteResourceEndpointTests

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("virtuoso", "virtuoso", "ovm", "gr:BusinessEntity", "eks:00226238")]
        public async Task TestConcreteResourceEndpoint_ShouldReturnOkWithNotEmptyResourceVm(string endpointWithoutGraph,
            string endpointWithGraph, string graph, string classId, string resource)
        {
            var responses = new List<HttpResponseMessage>();
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphClassesConcreteResource
                    .Replace("{endpoint}", endpointWithoutGraph).Replace("{class}", classId)
                    .Replace("{resource:regex(.+:.+)}", resource) + LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphClassesConcreteResource
                                                        .Replace("{endpoint}", endpointWithGraph)
                                                        .Replace("{graph}", graph).Replace("{class}", classId)
                                                        .Replace("{resource:regex(.+:.+)}", resource) +
                                                    LimitQuery));
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphResourcesConcreteResource
                                              .Replace("{endpoint}", endpointWithoutGraph)
                                              .Replace("{resource:regex(.+:.+)}", resource) +
                                          LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphResourcesConcreteResource
                                                        .Replace("{endpoint}", endpointWithGraph)
                                                        .Replace("{graph}", graph)
                                                        .Replace("{resource:regex(.+:.+)}", resource) +
                                                    LimitQuery));
            responses.ForEach(x => x.EnsureSuccessStatusCode());
            var responseBodies = new List<ResourceVm>();
            responses.ForEach(async x => responseBodies.Add(await x.Content.ReadAsAsync<ResourceVm>()));
            responses.ForEach(x => Assert.Equal(HttpStatusCode.OK, x.StatusCode));
            responseBodies.ForEach(x => Assert.True(x != null && x.Predicates.Count > 0));
            responses.ForEach(x =>
            {
                if (x.Content.Headers.ContentType != null)
                    Assert.Equal("application/json", x.Content.Headers.ContentType.MediaType);
            });
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("virtuoso", "virtuoso", "ovm", "gr:BusinessEntity", "eks:wrongResource")]
        [InlineData("datagov", "virtuoso", "lexvo", "gr:BusinessEntity", "eks:00226238")]
        public async Task TestConcreteResourceEndpoint_ShouldReturnNotFound(string endpointWithoutGraph,
            string endpointWithGraph, string graph, string classId, string resource)
        {
            var responses = new List<HttpResponseMessage>();
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphClassesConcreteResource
                    .Replace("{endpoint}", endpointWithoutGraph).Replace("{class}", classId)
                    .Replace("{resource:regex(.+:.+)}", resource) + LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphClassesConcreteResource
                                                        .Replace("{endpoint}", endpointWithGraph)
                                                        .Replace("{graph}", graph).Replace("{class}", classId)
                                                        .Replace("{resource:regex(.+:.+)}", resource) +
                                                    LimitQuery));
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphResourcesConcreteResource
                                              .Replace("{endpoint}", endpointWithoutGraph)
                                              .Replace("{resource:regex(.+:.+)}", resource) +
                                          LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphResourcesConcreteResource
                .Replace("{endpoint}", endpointWithGraph)
                .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resource) + LimitQuery));
            responses.ForEach(x => Assert.Equal(HttpStatusCode.NotFound, x.StatusCode));
        }

        #endregion

        #region ConretePredicateEndpointTests

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("virtuoso", "virtuoso", "ovm", "gr:BusinessEntity", "eks:00226238", "rdf:type")]
        public async Task TestPredicateEndpoint_ShouldReturnOkWithNotEmptyPredicateVm(string endpointWithoutGraph,
            string endpointWithGraph, string graph, string classId, string resource, string predicate)
        {
            var responses = new List<HttpResponseMessage>();
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphClassStartConcreteResourcePredicate
                                              .Replace("{endpoint}", endpointWithoutGraph).Replace("{class}", classId)
                                              .Replace("{resource:regex(.+:.+)}", resource)
                                              .Replace("{predicate:regex(.+:.+)}", predicate) +
                                          LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphClassStartConcreteResourcePredicate
                .Replace("{endpoint}", endpointWithGraph)
                .Replace("{graph}", graph).Replace("{class}", classId).Replace("{resource:regex(.+:.+)}", resource)
                .Replace("{predicate:regex(.+:.+)}", predicate) + LimitQuery));
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphResourceStartConcreteResourcePredicate
                    .Replace("{endpoint}", endpointWithoutGraph).Replace("{resource:regex(.+:.+)}", resource)
                    .Replace("{predicate:regex(.+:.+)}", predicate) + LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate
                .Replace("{endpoint}", endpointWithGraph)
                .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resource)
                .Replace("{predicate:regex(.+:.+)}", predicate) + LimitQuery));
            responses.ForEach(x => x.EnsureSuccessStatusCode());
            var responseBodies = new List<PredicateVm>();
            responses.ForEach(async x => responseBodies.Add(await x.Content.ReadAsAsync<PredicateVm>()));
            responses.ForEach(x => Assert.Equal(HttpStatusCode.OK, x.StatusCode));
            responseBodies.ForEach(x => Assert.True(x != null && x.Curies.Count > 0));
            responses.ForEach(x =>
            {
                if (x.Content.Headers.ContentType != null)
                    Assert.Equal("application/json", x.Content.Headers.ContentType.MediaType);
            });
        }

        [SparqlSetupExtensions.IgnoreOnSparqlSettingUpForGetTestsNotSuccessful]
        [InlineData("virtuoso", "virtuoso", "ovm", "gr:BusinessEntity", "eks:wrongResource", "ovm:wrongPredicate")]
        [InlineData("datagov", "virtuoso", "lexvo", "gr:BusinessEntity", "eks:00226238", "rdf:type")]
        public async Task TestPredicateEndpoint_ShouldReturnNotFound(string endpointWithoutGraph,
            string endpointWithGraph, string graph, string classId, string resource, string predicate)
        {
            var responses = new List<HttpResponseMessage>();
            responses.Add(await TestClient.GetAsync(ApiRoutes.DefaultGraphClassStartConcreteResourcePredicate
                                                        .Replace("{endpoint}", endpointWithoutGraph)
                                                        .Replace("{class}", classId)
                                                        .Replace("{resource:regex(.+:.+)}", resource)
                                                        .Replace("{predicate:regex(.+:.+)}", predicate) +
                                                    LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphClassStartConcreteResourcePredicate
                .Replace("{endpoint}", endpointWithGraph)
                .Replace("{graph}", graph).Replace("{class}", classId).Replace("{resource:regex(.+:.+)}", resource)
                .Replace("{predicate:regex(.+:.+)}", predicate) + LimitQuery));
            responses.Add(
                await TestClient.GetAsync(ApiRoutes.DefaultGraphResourceStartConcreteResourcePredicate
                    .Replace("{endpoint}", endpointWithoutGraph).Replace("{resource:regex(.+:.+)}", resource)
                    .Replace("{predicate:regex(.+:.+)}", predicate) + LimitQuery));
            responses.Add(await TestClient.GetAsync(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate
                .Replace("{endpoint}", endpointWithGraph)
                .Replace("{graph}", graph).Replace("{resource:regex(.+:.+)}", resource)
                .Replace("{predicate:regex(.+:.+)}", predicate) + LimitQuery));
            responses.ForEach(x => Assert.Equal(HttpStatusCode.NotFound, x.StatusCode));
        }

        #endregion
    }
}