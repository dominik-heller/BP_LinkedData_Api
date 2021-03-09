using System.Collections.Generic;
using LinkedData_Api.Model.ViewModels;
using Swashbuckle.AspNetCore.Filters;

namespace LinkedData_Api.Swagger.ExampleSchemas
{
    public class EndpointExample : IExamplesProvider<EndpointVm>
    {
        public EndpointVm GetExamples()
        {
            return new EndpointVm()
            {
                EndpointName = "endpointName",
                EndpointUrl = "http://localhost/sparql",
                DefaultGraph = "http://localhost/dgraph",
                NamedGraphs = new List<NamedGraph>()
                {
                    new NamedGraph() {GraphName = "ngraph1", Uri = "http://localhost/ngraph1"},
                    new NamedGraph() {GraphName = "ngraph2", Uri = "http://localhost/ngraph2"}
                },
                Namespaces = new List<Namespace>()
                {
                    new Namespace() {Prefix = "examplePrefix1", Uri = "http://www.example1.com/"},
                    new Namespace() {Prefix = "examplePrefix2", Uri = "http://www.example2.com/"}
                },
                EntryResource = new List<EntryResource>()
                {
                    new EntryResource() {GraphName = "default", Command = "SELECT DISTINCT ?s WHERE{?s <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> <http://www.w3.org/2002/07/owl#Thing>}"},
                    new EntryResource() {GraphName = "ngraph1", Command = "SELECT ?s WHERE{?s ?p ?o}"}
                },
                EntryClass = new List<EntryClass>()
                {
                    new EntryClass() {GraphName = "default", Command = "SELECT DISTINCT ?s WHERE{?s <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> <http://www.w3.org/2002/07/owl#Class>}"},
                    new EntryClass() {GraphName = "ngraph2", Command = "SELECT ?s WHERE{?s <http://www.w3.org/2000/01/rdf-schema#subClassOf> <http://www.w3.org/2004/02/skos/core#Collection>}"}
                },
                SupportedMethods = new SupportedMethods() {Sparql10 = "yes", Sparql11 = "no"}
            };
        }
    }
}