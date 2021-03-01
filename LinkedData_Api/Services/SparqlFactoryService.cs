#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Model.ParameterDto;
using LinkedData_Api.Model.ViewModels;
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

        #region SelectQueries

        public string? GetFinalQuery(string? query, QueryStringParametersDto queryStringParameters)
        {
            if (query != null) query = ApplyQueryStringParametersToSparqlQuery(query, queryStringParameters, "?s");
            return query;
        }

        public string? GetFinalSelectQueryForClass(ParametersDto parameters)
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

        public string? GetFinalSelectQueryForResource(ParametersDto parameters)
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

        public string? GetFinalSelectQueryForPredicate(ParametersDto parameters)
        {
            SparqlParameterizedString sparqlParameterizedString = new();
            if (parameters.RouteParameters.Resource.Equals("*"))
                sparqlParameterizedString.CommandText = "SELECT * WHERE {[] @pred ?o}";
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Resource,
                out var resourceAbsoluteUri))
            {
                sparqlParameterizedString.CommandText = "SELECT * WHERE {@sub @pred ?o}";
                sparqlParameterizedString.SetUri("sub", new Uri(resourceAbsoluteUri));
            }

            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Predicate,
                out var predicateAbsoluteUri))
            {
                sparqlParameterizedString.SetUri("pred", new Uri(predicateAbsoluteUri));
                string query = ApplyQueryStringParametersToSparqlQuery(sparqlParameterizedString.ToString(),
                    parameters.QueryStringParametersDto, "?o");
                return query;
            }

            return null;
        }

        #endregion


        #region PutSparqlQueries

        public string? GetFinalPutQueryForResource(ParametersDto parameters, ResourceVm resourceVm)
        {
            string resourceCurie = parameters.RouteParameters.Resource;
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(resourceCurie, out string resourceAbsoluteUri))
            {
                SparqlParameterizedString sparqlParameterizedDeleteQuery = new();
                sparqlParameterizedDeleteQuery.CommandText = "DELETE {@sub ?p ?o} WHERE {@sub ?p ?o} ";
                sparqlParameterizedDeleteQuery.SetUri("sub", new Uri(resourceAbsoluteUri));
                string deleteQuery = sparqlParameterizedDeleteQuery.ToString();
                SparqlParameterizedString sparqlParameterizedInsertQuery = new();
                sparqlParameterizedInsertQuery.CommandText = "INSERT DATA {@sub @pred @obj";
                string insertQuery = "";
                sparqlParameterizedInsertQuery.SetUri("sub", new Uri(resourceAbsoluteUri));
                SparqlParameterizedString sparqlParameterizedInsertSubQuery = new();
                sparqlParameterizedInsertSubQuery.CommandText = "; @pred @obj";
                bool first = true;
                foreach (var propertyName in resourceVm.Properties.Keys)
                {
                    if (_namespaceFactoryService.GetAbsoluteUriFromQname(propertyName, out string predicateAbsoluteUri))
                    {
                        sparqlParameterizedInsertQuery.SetUri("pred", new Uri(predicateAbsoluteUri));
                        sparqlParameterizedInsertSubQuery.SetUri("pred", new Uri(predicateAbsoluteUri));
                        PropertyContent x = resourceVm.Properties[propertyName];
                        foreach (var objCurie in x.Curies)
                        {
                            if (_namespaceFactoryService.GetAbsoluteUriFromQname(objCurie,
                                out string objectAbsoluteUri))
                            {
                                if (first)
                                {
                                    sparqlParameterizedInsertQuery.SetUri("obj", new Uri(objectAbsoluteUri));
                                    insertQuery = sparqlParameterizedInsertQuery.ToString();
                                    first = false;
                                    continue;
                                }

                                sparqlParameterizedInsertSubQuery.SetUri("obj", new Uri(objectAbsoluteUri));
                                insertQuery += sparqlParameterizedInsertSubQuery.ToString();
                                Console.WriteLine(insertQuery);
                            }
                            else
                            {
                                return null;
                            }
                        }
                        //TODO: Insert literals: foreach...
                    }
                    else
                    {
                        return null;
                    }
                }

                string finalQuery = deleteQuery + insertQuery + "}";
                Console.WriteLine(finalQuery);
                return finalQuery;
            }

            return null;
        }

        #endregion


        /*
         *{
  "properties": {
    "propertyName": {
      "curies": [
        "curie1",
        "curie2",
        "curie3"
      ],
      "literals": [
        {
          "value": "value1",
          "datatype": "datatype1",
          "language": "lang1"
        },
        {
          "value": "value1",
          "datatype": "datatype1",
          "language": "lang1"
        }
      ]
    }
  }
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
    }
}