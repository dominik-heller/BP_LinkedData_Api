using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VDS.RDF.Query;
using Xunit;

namespace LinkedData_Api.Tests.Extensions
{
    public class SparqlSetupExtensions
    {
        public sealed class IgnoreOnSparqlSetUpForUpdateTestFailed : TheoryAttribute
        {
            private static bool _first = true;
            private static bool _setUpSuccessful = false;

            private readonly string insertQuery =
                @"PREFIX ex: <http://example.org/> 
INSERT DATA {ex:uniqueDelResource1 ex:uniqueDelPredicate1 ex:curie1; ex:uniqueDelPredicate1 ex:curie2; ex:uniqueDelPredicate1 ex:curie3; ex:uniqueDelPredicate1 'value1'^^ex:exampleDatatype; ex:uniqueDelPredicate1 'value2'@en; ex:uniqueDelPredicate1 'value'} 
INSERT DATA {ex:uniqueDelResource2 ex:uniqueDelPredicate2 ex:curie1; ex:uniqueDelPredicate2 ex:curie2; ex:uniqueDelPredicate3 ex:curie3; ex:uniqueDelPredicate2 'value1'^^ex:exampleDatatype; ex:uniqueDelPredicate2 'value2'@en; ex:uniqueDelPredicate2 'value'} 
INSERT DATA {ex:uniquePutResource1 ex:uniquePutPredicate1 ex:curie1} 
INSERT DATA{ ex:uniquePutResource2 ex:uniquePutPredicate2 ex:curie2; ex:uniquePutPredicate3 ex:curie3}";

            public IgnoreOnSparqlSetUpForUpdateTestFailed()
            {
                if (_first)
                {
                    _setUpSuccessful = SetupSparqlEndpointData();
                    _first = false;
                }

                if (!_setUpSuccessful)
                {
                    Skip = "Sparql endpoint for update queries not reachable.";
                }
            }

            private bool SetupSparqlEndpointData()
            {
                try
                {
                    SparqlRemoteEndpoint sparqlRemoteEndpoint =
                        new SparqlRemoteEndpoint(new Uri("http://localhost:8890/sparql/"),
                            new Uri("http://localhost:8890/test"));
                    var response1 = sparqlRemoteEndpoint.QueryRaw("delete {?s ?p ?o} where {?s ?p ?o}");
                    var response2 = sparqlRemoteEndpoint.QueryRaw(insertQuery);
                    if (response1.StatusCode == HttpStatusCode.OK && response2.StatusCode == HttpStatusCode.OK)
                        return true;
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public sealed class IgnoreOnSparqlSettingUpForGetTestsNotSuccessful : TheoryAttribute
        {
            private static bool _first = true;
            private static bool _setUpSuccessful = false;

            private readonly List<Uri> _testSparqlEndpoints = new List<Uri>
            {
                new Uri("http://localhost:8890/sparql/"),
                new Uri("https://data.gov.cz/sparql"),
                new Uri("https://dbpedia.org/sparql")
            };

            public IgnoreOnSparqlSettingUpForGetTestsNotSuccessful()
            {
                if (_first)
                {
                    _setUpSuccessful = SetupSparqlEndpointData();
                    _first = false;
                }

                if (!_setUpSuccessful)
                {
                    Skip =
                        $"Some sparql endpoints are not reachable. " +
                        $"Therefore tests cannot be executed. " +
                        $"List of used endpoints: {String.Join(", ", _testSparqlEndpoints)}";
                }
            }

            private bool SetupSparqlEndpointData()
            {
                try
                {
                    FederatedSparqlRemoteEndpoint federatedSparqlRemoteEndpoint =
                        new FederatedSparqlRemoteEndpoint(_testSparqlEndpoints);
                    var response =
                        federatedSparqlRemoteEndpoint.QueryWithResultSet("select ?s where {?s ?p ?o} limit 1");
                    if (response.Count() == 3) return true;
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}