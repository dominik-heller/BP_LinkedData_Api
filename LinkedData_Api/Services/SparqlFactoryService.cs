#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Model.ParameterDto;
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


        public string? GetFinalQuery(string? query, ParameterDto parameterDto)
        {
            if (query != null)
            {
                query = ApplyQueryStringParametersToSparqlQuery(query, parameterDto.QueryStringParametersDto);
                return query;
            }

            return query;
        }

        public string? GetFinalQuery(string? query, QueryStringParametersDto queryStringParameters)
        {
            if (query != null) query = ApplyQueryStringParametersToSparqlQuery(query, queryStringParameters);
            return query;
        }

        public string? GetFinalQueryForClass(ParameterDto parameters)
        {
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Class, out var absoluteUri))
            {
                SparqlParameterizedString sparqlParameterizedString = new();
                sparqlParameterizedString.CommandText = "SELECT ?s WHERE {?s ?p @var}";
                sparqlParameterizedString.SetUri("var", new Uri(absoluteUri));
                string query = ApplyQueryStringParametersToSparqlQuery(sparqlParameterizedString.ToString(),parameters.QueryStringParametersDto);
                return query;
            }
            return null;
        }


        private string ApplyQueryStringParametersToSparqlQuery(string query,
            QueryStringParametersDto queryStringParameters)
        {
            var limit = queryStringParameters.Limit;
            var offset = queryStringParameters.Offset;
            var regex = queryStringParameters.Regex;
            if (regex != null)
            {
                var i = query.LastIndexOf("}", StringComparison.Ordinal);
                query = $"{query.Substring(0, i)} FILTER regex(?s, \"{regex}\") {query.Substring(i)}";
            }

            //TODO: Potentially ddd sort  ...?sort(+foaf:name) = ORDER BY Desc(foaf:name) X  ...?sort(-foaf:name) = ORDER BY (foaf:name) ***možná ani orderby desc netřeba
            query += $" LIMIT {limit} OFFSET {offset}";

            return query;
        }
    }
}