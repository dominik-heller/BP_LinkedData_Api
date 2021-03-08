using System.Collections.Generic;
using LinkedData_Api.Model.Domain;
using Swashbuckle.AspNetCore.Filters;

namespace LinkedData_Api.Swagger.ExampleSchemas
{
    public class EndpointExample : IExamplesProvider<Endpoint>
    {
        public Endpoint GetExamples()
        {
            return new Endpoint()
            {
                EndpointName = "endpointName",
                EndpointUrl = "http:localhost/sparql",
                DefaultGraph = "http:localhost/dgraph",
                NamedGraphs = new List<NamedGraph>()
                {
                    new NamedGraph() {GraphName = "ngraph1", Uri = "http:localhost/dgraph1"},
                    new NamedGraph() {GraphName = "ngraph2", Uri = "http:localhost/dgraph2"}
                },
                Namespaces = new List<Namespace>()
                {
                    new Namespace() {Prefix = "examplePrefix1", Uri = "http://www.example1.com/"},
                    new Namespace() {Prefix = "examplePrefix2", Uri = "http://www.example2.com/"}
                },
                EntryResource = new List<EntryResource>()
                {
                    new EntryResource() {GraphName = "default", Command = "sparqlSelectResourceCommand1"},
                    new EntryResource() {GraphName = "ngraph1", Command = "sparqlSelectResourceCommand2"}
                },
                EntryClass = new List<EntryClass>()
                {
                    new EntryClass() {GraphName = "default", Command = "sparqlSelectClassCommand1"},
                    new EntryClass() {GraphName = "ngraph2", Command = "sparqlSelectClassCommand2"}
                },
                SupportedMethods = new SupportedMethods() {Sparql10 = "yes", Sparql11 = "no"}
            };
        }
    }
}