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
            var limit = parameterDto.QueryStringParametersDto.Limit;
            var offset = parameterDto.QueryStringParametersDto.Offset;
            var regex = parameterDto.QueryStringParametersDto.Regex;
            if (regex != null)
            {
                var i = query.LastIndexOf("}");
                query = $"{query.Substring(0, i)} FILTER regex(?s, \"{regex}\") {query.Substring(i)}";
            }
            //TODO: Potentially ddd sort  ...?sort(+foaf:name) = ORDER BY Desc(foaf:name) X  ...?sort(-foaf:name) = ORDER BY (foaf:name) ***možná ani orderby desc netřeba
            query +=$" LIMIT {limit} OFFSET {offset}";
            return query;
        }
        
    }
}