#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
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
        private readonly INamespaceFactoryService _namespaceFactoryService;

        public SparqlFactoryService(INamespaceFactoryService namespaceFactoryService)
        {
            _namespaceFactoryService = namespaceFactoryService;
        }


        public string? GetFinalQuery(string? query, QueryStringParametersDto queryStringParameters)
        {
            if (query != null) query = ApplyQueryStringParametersToSparqlQuery(query, queryStringParameters, "?s");
            return query;
        }

        public string? GetFinalQueryForClass(ParameterDto parameters)
        {
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Class, out var absoluteUri))
            {
                SparqlParameterizedString sparqlParameterizedString = new();
                sparqlParameterizedString.CommandText = "SELECT ?s WHERE {?s ?p @var}";
                sparqlParameterizedString.SetUri("var", new Uri(absoluteUri));
                string query = ApplyQueryStringParametersToSparqlQuery(sparqlParameterizedString.ToString(),
                    parameters.QueryStringParametersDto, "?s");
                return query;
            }

            return null;
        }

        public string? GetFinalQueryForResource(ParameterDto parameters)
        {
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Resource,
                out var absoluteUri))
            {
                SparqlParameterizedString sparqlParameterizedString = new();
                sparqlParameterizedString.CommandText = "SELECT * WHERE {@var ?p ?o}";
                sparqlParameterizedString.SetUri("var", new Uri(absoluteUri));
                string query = ApplyQueryStringParametersToSparqlQuery(sparqlParameterizedString.ToString(),
                    parameters.QueryStringParametersDto, "?p");
                return query;
            }

            return null;
        }

        public string? GetFinalQueryForPredicate(ParameterDto parameters)
        {
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Resource,
                out var resourceAbsoluteUri))
            {
                if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Predicate,
                    out var predicateAbsoluteUri))
                {
                    SparqlParameterizedString sparqlParameterizedString = new();
                    sparqlParameterizedString.CommandText = "SELECT * WHERE {@sub @pred ?o}";
                    sparqlParameterizedString.SetUri("sub", new Uri(resourceAbsoluteUri));
                    sparqlParameterizedString.SetUri("pred", new Uri(predicateAbsoluteUri));
                    string query = ApplyQueryStringParametersToSparqlQuery(sparqlParameterizedString.ToString(),
                        parameters.QueryStringParametersDto, "?o");
                    return query;
                }
            }

            return null;
        }

        /*
        public string? GetFinalQueryForClassResource(string? classQuery, ParameterDto parameters)
        {
            if (classQuery == null) return null;
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Resource,
                out var absoluteUri))
            {
                string query = ConstructClassResourceQuery(classQuery);
                SparqlParameterizedString sparqlParameterizedString = new();
                sparqlParameterizedString.CommandText = query;
                sparqlParameterizedString.SetUri("var", new Uri(absoluteUri));
                query = ApplyQueryStringParametersToSparqlQuery(sparqlParameterizedString.ToString(),
                    parameters.QueryStringParametersDto, "?p");
                return query;
            }

            return null;
        }

        private string ConstructClassResourceQuery(string classQuery)
        {
            var x = Regex.Split(classQuery, "where", RegexOptions.IgnoreCase);
            var y = x[1].Substring(x[1].IndexOf('?')).Split((char[]) null!, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim()).First();
            x[0] = x[0].Replace(y, "*");
            var query = x[1].Replace(y, "@var");
            var i = query.IndexOfAny(new char[] {';', '}'});
            query = query.Remove(i, 1);
            query = query.Insert(i, "; ?p ?o }");
            query = $"{x[0]} WHERE {query}";
            return query;
        }
*/
        private string ApplyQueryStringParametersToSparqlQuery(string query,
            QueryStringParametersDto queryStringParameters, string sortAndRegexParameter)
        {
            var limit = queryStringParameters.Limit;
            var offset = queryStringParameters.Offset;
            var regex = queryStringParameters.Regex;
            var sort = queryStringParameters.Sort;
            if (regex != null)
            {
                var i = query.LastIndexOf("}", StringComparison.Ordinal);
                query =
                    $"{query.Substring(0, i)} FILTER regex({sortAndRegexParameter}, \"{regex}\") {query.Substring(i)}";
            }

            if (sort != null)
            {
                if (sort.Equals("asc")) query += $" ORDER BY ({sortAndRegexParameter})";
                if (sort.Equals("desc")) query += $" ORDER BY DESC ({sortAndRegexParameter})";
            }

            //TODO: Potentially ddd sort ...?sort(+foaf:name) = ORDER BY Desc(foaf:name) X ...?sort(-foaf:name) = ORDER BY (foaf:name) ***možná ani orderby desc netřeba
            query += $" LIMIT {limit} OFFSET {offset}";

            return query;
        }
    }
}