using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Services;
using LinkedData_Api.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using VDS.RDF;
using Xunit;

namespace LinkedData_Api.Tests
{
    public class ParameterProcessorServiceTests
    {
        private readonly IParametersProcessorService _parametersProcessorService;

        public ParameterProcessorServiceTests()
        {
            _parametersProcessorService = new ParametersProcessorService();
        }


        //If class, resource, predicate are curies or not is checked by apiRouteDefinition regex
        [Theory]
        [InlineData("testendpoint", "testgraph", "ex:testclass", "ex:testresource", "exestpredicate", "testregex",
            "testsort", "50", "0")]
        [InlineData("testendpoint1", "testgraph1", "ex:testclass1", "ex:testresource1", "exestpredicate1", "testregex1",
            "testsort1", "133", "77")]
        public void ProccessParameters_ShouldReturnCompleteParameterObject(string endpoint, string graph, string @class,
            string resource, string predicate, string regex, string sort, string limit,
            string offset)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary
            {
                {"endpoint", endpoint},
                {"graph", graph},
                {"class", @class},
                {"resource", resource},
                {"predicate", predicate}
            };
            List<KeyValuePair<string, string>> queryStringDictionary = new List<KeyValuePair<string, string>>
            {
                new("regex", regex),
                new("sort", sort),
                new("limit", limit),
                new("offset", offset)
            };
            Parameters parameters = _parametersProcessorService.ProcessParameters(routeValueDictionary,
                QueryString.Create(queryStringDictionary));
            Assert.Equal(parameters.RouteParameters.Endpoint, endpoint);
            Assert.Equal(parameters.RouteParameters.Graph, graph);
            Assert.Equal(parameters.RouteParameters.Class, @class);
            Assert.Equal(parameters.RouteParameters.Resource, resource);
            Assert.Equal(parameters.RouteParameters.Predicate, predicate);
            Assert.Equal(parameters.QueryStringParametersDto.Limit, Int32.Parse(limit));
            Assert.Equal(parameters.QueryStringParametersDto.Offset, Int32.Parse(offset));
            Assert.Equal(parameters.QueryStringParametersDto.Regex, regex);
            Assert.Equal(parameters.QueryStringParametersDto.Sort, sort);
        }
    }
}