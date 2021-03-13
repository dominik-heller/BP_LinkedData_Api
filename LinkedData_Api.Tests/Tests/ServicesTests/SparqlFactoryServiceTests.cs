using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LinkedData_Api.Data;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Services;
using LinkedData_Api.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using Newtonsoft.Json;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Query.Algebra;
using VDS.RDF.Writing.Formatting;
using Xunit;
using Endpoint = LinkedData_Api.Model.Domain.Endpoint;

namespace LinkedData_Api.Tests
{
    public class SparqlFactoryServiceTests
    {
        private readonly SparqlFactoryService _sparqlFactoryService;

        private readonly Mock<INamespaceFactoryService> _namespaceFactoryServiceMock = new();
        private readonly Mock<IEndpointService> _endpointServiceMock = new();

        public SparqlFactoryServiceTests()
        {
            _sparqlFactoryService =
                new SparqlFactoryService(_namespaceFactoryServiceMock.Object, _endpointServiceMock.Object);
        }

        [Theory]
        [InlineData(100, 5, "Prague", "asc")]
        [InlineData(100, 5, "Prague", "desc")]
        public async Task GetFinalQueryTest_ShouldReturnQueryWithQueryStringParameters(int limit, int offset,
            string regex, string sort)
        {
            Endpoint e = await CreateEndpointMockObject();
            e.NamedGraphs = null;
            _endpointServiceMock.Setup(x => x.GetEndpointConfiguration(It.IsAny<string>())).Returns(e);
            string mockQuery = "Select distinct * Where { ?s ?p ?o }";
            string expectedQuery =
                $"Select distinct * FROM <{e.DefaultGraph}> WHERE {{ ?s ?p ?o FILTER regex(?s, \"{regex}\") }} ORDER BY DESC (?s) LIMIT {limit} OFFSET {offset}";
            if (sort.Equals("asc"))
            {
                expectedQuery =
                    $"Select distinct * FROM <{e.DefaultGraph}> WHERE {{ ?s ?p ?o FILTER regex(?s, \"{regex}\") }} ORDER BY (?s) LIMIT {limit} OFFSET {offset}";
            }

            Parameters p = CreateParametersMockObject();
            p.QueryStringParametersDto.Limit = limit;
            p.QueryStringParametersDto.Offset = offset;
            p.QueryStringParametersDto.Sort = sort;
            p.QueryStringParametersDto.Regex = regex;

            string query = _sparqlFactoryService.GetFinalQuery(mockQuery, p);

            Assert.Equal(expectedQuery, query);
        }
        
        [Theory]
        [InlineData(100, 5, "Prague", "asc", "http://example.org/resource1")]
        [InlineData(100, 5, "Prague", "desc", "http://example.org/resource2")]
        public async Task GetFinalSelectQueryForClassTest_ShouldReturnFinalSelectQuery(int limit, int offset,
            string regex, string sort, string resourceAndPredicateUri)
        {
            Endpoint e = await CreateEndpointMockObject();
            e.NamedGraphs = null;
            _endpointServiceMock.Setup(x => x.GetEndpointConfiguration(It.IsAny<string>())).Returns(e);
            _namespaceFactoryServiceMock
                .Setup(x => x.GetAbsoluteUriFromQname(It.IsAny<string>(), out resourceAndPredicateUri)).Returns(true);
            string expectedQuery =
                $"SELECT ?s FROM <{e.DefaultGraph}> WHERE {{ ?s ?p <{resourceAndPredicateUri}> FILTER regex(?s, \"{regex}\") }} ORDER BY DESC (?s) LIMIT {limit} OFFSET {offset}";
            if (sort.Equals("asc"))
            {
                expectedQuery =
                    $"SELECT ?s FROM <{e.DefaultGraph}> WHERE {{ ?s ?p <{resourceAndPredicateUri}> FILTER regex(?s, \"{regex}\") }} ORDER BY (?s) LIMIT {limit} OFFSET {offset}";
            }

            Parameters p = CreateParametersMockObject();
            p.QueryStringParametersDto.Limit = limit;
            p.QueryStringParametersDto.Offset = offset;
            p.QueryStringParametersDto.Sort = sort;
            p.QueryStringParametersDto.Regex = regex;

            string query = _sparqlFactoryService.GetFinalSelectQueryForClass(p);
            Console.WriteLine();
            Assert.Equal(expectedQuery, query);
        }
        
        
        [Theory]
        [InlineData(100, 5, "Prague", "asc", "http://example.org/resource1")]
        [InlineData(100, 5, "Prague", "desc", "http://example.org/resource2")]
        public async Task GetFinalSelectQueryForResourceTest_ShouldReturnFinalSelectQuery(int limit, int offset,
            string regex, string sort, string resourceAndPredicateUri)
        {
            Endpoint e = await CreateEndpointMockObject();
            e.NamedGraphs = null;
            _endpointServiceMock.Setup(x => x.GetEndpointConfiguration(It.IsAny<string>())).Returns(e);
            _namespaceFactoryServiceMock
                .Setup(x => x.GetAbsoluteUriFromQname(It.IsAny<string>(), out resourceAndPredicateUri)).Returns(true);
            string expectedQuery =
                $"SELECT * FROM <{e.DefaultGraph}> WHERE {{ <{resourceAndPredicateUri}> ?p ?o FILTER regex(?p, \"{regex}\") }} ORDER BY DESC (?p) LIMIT {limit} OFFSET {offset}";
            if (sort.Equals("asc"))
            {
                expectedQuery =
                    $"SELECT * FROM <{e.DefaultGraph}> WHERE {{ <{resourceAndPredicateUri}> ?p ?o FILTER regex(?p, \"{regex}\") }} ORDER BY (?p) LIMIT {limit} OFFSET {offset}";
            }

            Parameters p = CreateParametersMockObject();
            p.QueryStringParametersDto.Limit = limit;
            p.QueryStringParametersDto.Offset = offset;
            p.QueryStringParametersDto.Sort = sort;
            p.QueryStringParametersDto.Regex = regex;

            string query = _sparqlFactoryService.GetFinalSelectQueryForResource(p);
            Assert.Equal(expectedQuery, query);
        }
        
        [Theory]
        [InlineData(100, 5, "Prague", "asc", "http://example.org/resource1")]
        [InlineData(100, 5, "Prague", "desc", "http://example.org/resource2")]
        public async Task GetFinalSelectQueryForPredicateTest_ShouldReturnFinalSelectQuery(int limit, int offset,
            string regex, string sort, string resourceAndPredicateUri)
        {
            Endpoint e = await CreateEndpointMockObject();
            e.NamedGraphs = null;
            _endpointServiceMock.Setup(x => x.GetEndpointConfiguration(It.IsAny<string>())).Returns(e);
            _namespaceFactoryServiceMock
                .Setup(x => x.GetAbsoluteUriFromQname(It.IsAny<string>(), out resourceAndPredicateUri)).Returns(true);
            string expectedQuery =
                $"SELECT * FROM <{e.DefaultGraph}> WHERE {{ <{resourceAndPredicateUri}> <{resourceAndPredicateUri}> ?o FILTER regex(?o, \"{regex}\") }} ORDER BY DESC (?o) LIMIT {limit} OFFSET {offset}";
            if (sort.Equals("asc"))
            {
                expectedQuery =
                    $"SELECT * FROM <{e.DefaultGraph}> WHERE {{ <{resourceAndPredicateUri}> <{resourceAndPredicateUri}> ?o FILTER regex(?o, \"{regex}\") }} ORDER BY (?o) LIMIT {limit} OFFSET {offset}";
            }

            Parameters p = CreateParametersMockObject();
            p.QueryStringParametersDto.Limit = limit;
            p.QueryStringParametersDto.Offset = offset;
            p.QueryStringParametersDto.Sort = sort;
            p.QueryStringParametersDto.Regex = regex;

            string query = _sparqlFactoryService.GetFinalSelectQueryForPredicate(p);
            Assert.Equal(expectedQuery, query);
        }
        

        [Theory]
        [InlineData("http://example.org/resource1")]
        [InlineData("http://example.org/resource2")]
        public async Task GetFinalDeleteQueryForResourceTest_ShouldReturnFinalDeleteQueryString(string resourceUri)
        {
            Endpoint e = await CreateEndpointMockObject();
            e.NamedGraphs = null;
            _endpointServiceMock.Setup(x => x.GetEndpointConfiguration(It.IsAny<string>())).Returns(e);
            _namespaceFactoryServiceMock.Setup(x => x.GetAbsoluteUriFromQname(It.IsAny<string>(), out resourceUri))
                .Returns(true);

            string expectedQuery =
                $"WITH <{e.DefaultGraph}> DELETE {{?s ?p ?o}} WHERE {{?s ?p ?o. FILTER (?s = <{resourceUri}> || ?o = <{resourceUri}>)}}";
            Parameters p = CreateParametersMockObject();
            string query = _sparqlFactoryService.GetFinalDeleteQueryForResource(p);

            Assert.Equal(expectedQuery, query);
        }

        [Theory]
        [InlineData("http://example.org/resource1")]
        [InlineData("http://example.org/resource2")]
        public async Task GetFinalDeleteQueryForPredicateTest_ShouldReturnFinalDeleteQueryString(
            string resourceAndPredicateUri)
        {
            Endpoint e = await CreateEndpointMockObject();
            e.NamedGraphs = null;
            _endpointServiceMock.Setup(x => x.GetEndpointConfiguration(It.IsAny<string>())).Returns(e);
            _namespaceFactoryServiceMock
                .Setup(x => x.GetAbsoluteUriFromQname(It.IsAny<string>(), out resourceAndPredicateUri)).Returns(true);

            string expectedQuery =
                $"WITH <{e.DefaultGraph}> DELETE {{<{resourceAndPredicateUri}> <{resourceAndPredicateUri}> ?o}} WHERE {{<{resourceAndPredicateUri}> <{resourceAndPredicateUri}> ?o}}";
            Parameters p = CreateParametersMockObject();
            string query = _sparqlFactoryService.GetFinalDeleteQueryForPredicate(p);
            Assert.Equal(expectedQuery, query);
        }

        [Theory]
        [InlineData("http://example.org/resource1")]
        [InlineData("http://example.org/resource2")]
        public async Task GetFinalPostQueryForResourceTest_ShouldReturnFinalPostQueryString(string resourceAndPredicateUri)
        {
            Endpoint e = await CreateEndpointMockObject();
            e.NamedGraphs = null;
            _endpointServiceMock.Setup(x => x.GetEndpointConfiguration(It.IsAny<string>())).Returns(e);
            _namespaceFactoryServiceMock
                .Setup(x => x.GetAbsoluteUriFromQname(It.IsAny<string>(), out resourceAndPredicateUri)).Returns(true);

            string expectedQuery =
                $"WITH <http://dbpedia.org> INSERT DATA {{<{resourceAndPredicateUri}> <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> \"Example language text...\"@en; <{resourceAndPredicateUri}> \"2021-03-02T20:00:00-01:00\"^^<{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> \"String value\"}} ";
            Parameters p = CreateParametersMockObject();
            string query = _sparqlFactoryService.GetFinalPostQueryForResource(p, CreateNamedResourceVmMock());
            Assert.Equal(expectedQuery, query);
        }

        [Theory]
        [InlineData("http://example.org/resource1")]
        [InlineData("http://example.org/resource2")]
        public async Task GetFinalPostQueryForPredicateTest_ShouldReturnFinalPostQueryString(string resourceAndPredicateUri)
        {
            Endpoint e = await CreateEndpointMockObject();
            e.NamedGraphs = null;
            _endpointServiceMock.Setup(x => x.GetEndpointConfiguration(It.IsAny<string>())).Returns(e);
            _namespaceFactoryServiceMock
                .Setup(x => x.GetAbsoluteUriFromQname(It.IsAny<string>(), out resourceAndPredicateUri)).Returns(true);

            string expectedQuery =
                $"WITH <http://dbpedia.org> INSERT DATA {{<{resourceAndPredicateUri}> <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> \"Example language text...\"@en; <{resourceAndPredicateUri}> \"2021-03-02T20:00:00-01:00\"^^<{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> \"String value\"}}";
            Parameters p = CreateParametersMockObject();
            string query = _sparqlFactoryService.GetFinalPostQueryForPredicate(p, CreateNamedPredicateVmMock());
            Assert.Equal(expectedQuery, query);
        }


        [Theory]
        [InlineData("http://example.org/resource1")]
        [InlineData("http://example.org/resource2")]
        public async Task GetFinalPutQueryForResourceTest_ShouldReturnFinalPutQueryString(string resourceAndPredicateUri)
        {
            Endpoint e = await CreateEndpointMockObject();
            e.NamedGraphs = null;
            _endpointServiceMock.Setup(x => x.GetEndpointConfiguration(It.IsAny<string>())).Returns(e);
            _namespaceFactoryServiceMock
                .Setup(x => x.GetAbsoluteUriFromQname(It.IsAny<string>(), out resourceAndPredicateUri)).Returns(true);
            string expectedQuery =
                $"WITH <http://dbpedia.org> DELETE {{<{resourceAndPredicateUri}> ?p ?o}} WHERE {{<{resourceAndPredicateUri}> ?p ?o}} INSERT DATA {{<{resourceAndPredicateUri}> <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> \"Example language text...\"@en; <{resourceAndPredicateUri}> \"2021-03-02T20:00:00-01:00\"^^<{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> \"String value\"}} ";
            Parameters p = CreateParametersMockObject();
            string query = _sparqlFactoryService.GetFinalPutQueryForResource(p, CreateNamedResourceVmMock());
            Assert.Equal(expectedQuery, query);
        }


        [Theory]
        [InlineData("http://example.org/resource1")]
        [InlineData("http://example.org/resource2")]
        public async Task GetFinalPutQueryForPredicateTest_ShouldReturnFinalPutQueryString(string resourceAndPredicateUri)
        {
            Endpoint e = await CreateEndpointMockObject();
            e.NamedGraphs = null;
            _endpointServiceMock.Setup(x => x.GetEndpointConfiguration(It.IsAny<string>())).Returns(e);
            _namespaceFactoryServiceMock
                .Setup(x => x.GetAbsoluteUriFromQname(It.IsAny<string>(), out resourceAndPredicateUri)).Returns(true);
            string expectedQuery =
                $"WITH <http://dbpedia.org> DELETE {{<{resourceAndPredicateUri}> <{resourceAndPredicateUri}> ?o}} WHERE {{<{resourceAndPredicateUri}> <{resourceAndPredicateUri}> ?o}} INSERT DATA {{<{resourceAndPredicateUri}> <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> <{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> \"Example language text...\"@en; <{resourceAndPredicateUri}> \"2021-03-02T20:00:00-01:00\"^^<{resourceAndPredicateUri}>; <{resourceAndPredicateUri}> \"String value\"}}";
            Parameters p = CreateParametersMockObject();
            string query = _sparqlFactoryService.GetFinalPutQueryForPredicate(p, CreateNamedPredicateVmMock());
            Assert.Equal(expectedQuery, query);
        }


        private NamedPredicateVm CreateNamedPredicateVmMock()
        {
            return new NamedPredicateVm()
            {
                PredicateCurie = "ex:examplePredicate",
                Curies = new List<string>()
                {
                    "ex:curie1", "ex:curie2", "ex:curie3"
                },
                Literals = new List<Literal>()
                {
                    new() {Value = "Example language text...", Language = "en"},
                    new() {Value = "2021-03-02T20:00:00-01:00", Datatype = "xsd:dateTime"},
                    new() {Value = "String value"}
                }
            };
        }

        private NamedResourceVm CreateNamedResourceVmMock()
        {
            return new NamedResourceVm()
            {
                ResourceCurie = "ex:exampleResource",
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
        }

        private async Task<Endpoint> CreateEndpointMockObject()
        {
            Endpoint e =
                JsonConvert.DeserializeObject<Endpoint>(
                    await File.ReadAllTextAsync(@"..\..\..\MockData\DefaultEndpoint\dbPediaSparqlEndpoint.json"));
            return e;
        }

        private Parameters CreateParametersMockObject()
        {
            RouteParameters routeParameters = new RouteParameters()
            {
                Endpoint = "ednpoint", Graph = "graph", Class = "class", Predicate = "predicate", Resource = "resource"
            };
            QueryStringParameters queryStringParameters = new QueryStringParameters()
            {
                Limit = 10, Offset = 0, Regex = "city", Sort = "asc"
            };
            Parameters parameters = new Parameters()
            {
                RouteParameters = routeParameters,
                QueryStringParametersDto = queryStringParameters
            };
            return parameters;
        }
    }
}