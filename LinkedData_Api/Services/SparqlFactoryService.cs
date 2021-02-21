using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using LinkedData_Api.DataModel.EndpointConfigurationDto;
using LinkedData_Api.DataModel.ParameterDto;
using VDS.RDF.Query;

namespace LinkedData_Api.Services
{
    public class SparqlFactoryService : ISparqlFactoryService
    {
        private IEndpointConfigurationService _endpointConfigurationService;
        private INamespaceFactoryService _namespaceFactoryService;
        public SparqlFactoryService(INamespaceFactoryService namespaceFactoryService, IEndpointConfigurationService endpointConfigurationService)
        {
            _namespaceFactoryService = namespaceFactoryService;
            _endpointConfigurationService = endpointConfigurationService;
        }
        public string ConstructEntryClassQuery(string endpointName)
        {
            EndpointDto endpointDto = _endpointConfigurationService.GetEndpointConfiguration(endpointName);
            string query = null;
            if (endpointDto != null)
            {
                query = endpointDto.EntryClass.FirstOrDefault(x => x.GraphName.Equals("default"))?.Command;
            }
            return query;
        }
        
        public IEnumerable<SparqlResult> ExecuteSelectSparqlQuery(string endpointName, string query)
        {
            query += " LIMIT 100"; //TODO: toto samozřejmě jen dočasné, bude odstaněno při řešení paginace :)
            SparqlRemoteEndpoint sparqlEndpoint = _endpointConfigurationService.GetSparqlSelectRemoteSparqlEndpoint(endpointName);
            SparqlResultSet sparqlResultSet = sparqlEndpoint.QueryWithResultSet(query);
            return sparqlResultSet.Results;
        }
        
    }
}