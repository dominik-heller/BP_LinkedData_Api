#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Services.Contracts;
using VDS.RDF;
using VDS.RDF.Query;

namespace LinkedData_Api.Services
{
    public class SparqlFactoryService : ISparqlFactoryService
    {
        private readonly IEndpointService _endpointConfigurationService;
        private INamespaceFactoryService _namespaceFactoryService;

        public SparqlFactoryService(INamespaceFactoryService namespaceFactoryService,
            IEndpointService endpointConfigurationService)
        {
            _namespaceFactoryService = namespaceFactoryService;
            _endpointConfigurationService = endpointConfigurationService;
        }

        public string? GetDefaultEntryClassQuery(string endpointName)
        {
            Endpoint? endpointDto = _endpointConfigurationService.GetEndpointConfiguration(endpointName);
            string? query = null;
            if (endpointDto != null)
            {
                query = endpointDto.EntryClass.FirstOrDefault(x => x.GraphName.Equals("default"))?.Command;
            }
            query += " LIMIT 100"; //TODO: toto samozřejmě jen dočasné, bude odstaněno při řešení paginace :)
            return query;
        }

        public string? GetGraphSpecificEntryClassQuery(string endpointName, string graphName)
        {
            Endpoint? endpointDto = _endpointConfigurationService.GetEndpointConfiguration(endpointName);
            string? query = null;
            if (endpointDto != null)
            {
                query = endpointDto.EntryClass.FirstOrDefault(x => x.GraphName.Equals(graphName))?.Command;
            }

            query += " LIMIT 100"; //TODO: toto samozřejmě jen dočasné, bude odstaněno při řešení paginace :)
            return query;
        }

        public async Task<IEnumerable<SparqlResult>?> ExecuteRemoteSelectSparqlQueryAsync(string endpointName, string? graphName, string query)
        {
            SparqlResultSet? sparqlResultSet = null;
            Endpoint? endpoint = _endpointConfigurationService.GetEndpointConfiguration(endpointName);
            if (endpoint != null && endpoint.SupportedMethods.Sparql10.Equals("yes"))
            {
                SparqlRemoteEndpoint sparqlEndpoint;
                if(graphName!=null && endpoint.NamedGraphs.Exists(x => x.GraphName.Equals(graphName))){
                   sparqlEndpoint = new SparqlRemoteEndpoint(new Uri(endpoint.EndpointUrl),
                        new Uri(endpoint.NamedGraphs.Find(x => x.GraphName.Equals(graphName))!.Uri));
                }
                else
                {
                    sparqlEndpoint = new SparqlRemoteEndpoint(new Uri(endpoint.EndpointUrl), endpoint.DefaultGraph);
                }
                try
                {
                    sparqlResultSet = await Task.Run(()=>sparqlEndpoint.QueryWithResultSet(query));
                }
                catch (RdfException)
                {
                    sparqlResultSet = null;
                }
            }

            return sparqlResultSet?.Results;
        }
    }
}