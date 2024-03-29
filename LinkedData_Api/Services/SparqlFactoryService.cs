﻿#nullable enable
using System;
using System.Linq;
using System.Text.RegularExpressions;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Services.Contracts;
using VDS.RDF.Query;

namespace LinkedData_Api.Services
{
    public class SparqlFactoryService : ISparqlFactoryService
    {
        private readonly INamespaceFactoryService _namespaceFactoryService;
        private readonly IEndpointService _endpointService;

        public SparqlFactoryService(INamespaceFactoryService namespaceFactoryService, IEndpointService endpointService)
        {
            _namespaceFactoryService = namespaceFactoryService;
            _endpointService = endpointService;
        }

        #region SelectQueries

        public string? GetFinalQuery(string? query, Parameters parameters)
        {
            if (query == null) return query;
            query = ApplyQueryStringParametersToSparqlQuery(query, parameters.QueryStringParametersDto,
                FindSubjectVariableInQuery(query));
            query = ImplementFromGraphClauseToSelectQuery(query, parameters);
            return query;
        }

        public string? GetFinalSelectQueryForClass(Parameters parameters)
        {
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Class, out var absoluteUri))
            {
                string command = "SELECT ?s WHERE { ?s ?p @var }";
                SparqlParameterizedString sparqlParameterizedString = new();
                sparqlParameterizedString.CommandText = command;
                sparqlParameterizedString.SetUri("var", new Uri(absoluteUri));
                string? query = ApplyQueryStringParametersToSparqlQuery(sparqlParameterizedString.ToString(),
                    parameters.QueryStringParametersDto, "?s");
                query = ImplementFromGraphClauseToSelectQuery(query, parameters);
                if (string.IsNullOrEmpty(query)) return null;
                return query;
            }

            return null;
        }

        public string? GetFinalSelectQueryForResource(Parameters parameters)
        {
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Resource,
                out var absoluteUri))
            {
                string command = "SELECT * WHERE { @var ?p ?o }";
                SparqlParameterizedString sparqlParameterizedString = new();
                sparqlParameterizedString.CommandText = command;
                sparqlParameterizedString.SetUri("var", new Uri(absoluteUri));
                string? query = ApplyQueryStringParametersToSparqlQuery(sparqlParameterizedString.ToString(),
                    parameters.QueryStringParametersDto, "?p");
                query = ImplementFromGraphClauseToSelectQuery(query, parameters);
                if (string.IsNullOrEmpty(query)) return null;
                return query;
            }

            return null;
        }

        public string? GetFinalSelectQueryForPredicate(Parameters parameters)
        {
            SparqlParameterizedString sparqlParameterizedString = new();
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Resource,
                out var resourceAbsoluteUri))
            {
                sparqlParameterizedString.CommandText = "SELECT * WHERE { @sub @pred ?o }";
                sparqlParameterizedString.SetUri("sub", new Uri(resourceAbsoluteUri));
            }

            if (_namespaceFactoryService.GetAbsoluteUriFromQname(parameters.RouteParameters.Predicate,
                out var predicateAbsoluteUri))
            {
                sparqlParameterizedString.SetUri("pred", new Uri(predicateAbsoluteUri));
                string? query = ApplyQueryStringParametersToSparqlQuery(sparqlParameterizedString.ToString(),
                    parameters.QueryStringParametersDto, "?o");
                query = ImplementFromGraphClauseToSelectQuery(query, parameters);
                if (string.IsNullOrEmpty(query)) return null;
                return query;
            }

            return null;
        }

        #endregion

        #region PutSparqlQueries

        public string? GetFinalPutQueryForResource(Parameters parameters, ResourceVm resourceVm)
        {
            string resourceCurie = parameters.RouteParameters.Resource;
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(resourceCurie, out string resourceAbsoluteUri))
            {
                SparqlParameterizedString sparqlParameterizedDeleteQuery = new();
                sparqlParameterizedDeleteQuery.CommandText = "DELETE {@sub ?p ?o} WHERE {@sub ?p ?o} ";
                sparqlParameterizedDeleteQuery.SetUri("sub", new Uri(resourceAbsoluteUri));
                string? deleteQuery = sparqlParameterizedDeleteQuery.ToString();
                string? insertQuery = ConstructInsertResourceQueryString(resourceVm, resourceAbsoluteUri);
                if (string.IsNullOrEmpty(insertQuery)) return null;
                deleteQuery = ImplementFromGraphClauseToInsertDeleteQuery(deleteQuery, parameters);
                if (string.IsNullOrEmpty(deleteQuery)) return null;
                string finalQuery = deleteQuery + insertQuery;
                return finalQuery;
            }

            return null;
        }


        public string? GetFinalPutQueryForPredicate(Parameters parameters, PredicateVm predicateVm)
        {
            string resourceCurie = parameters.RouteParameters.Resource;
            string predicateCurie = parameters.RouteParameters.Predicate;
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(resourceCurie, out string resourceAbsoluteUri) &&
                _namespaceFactoryService.GetAbsoluteUriFromQname(predicateCurie, out string predicateAbsoluteUri))
            {
                SparqlParameterizedString sparqlParameterizedDeleteQuery = new();
                sparqlParameterizedDeleteQuery.CommandText = "DELETE {@sub @pred ?o} WHERE {@sub @pred ?o} ";
                sparqlParameterizedDeleteQuery.SetUri("sub", new Uri(resourceAbsoluteUri));
                sparqlParameterizedDeleteQuery.SetUri("pred", new Uri(predicateAbsoluteUri));
                string? deleteQuery = sparqlParameterizedDeleteQuery.ToString();
                string? insertQuery =
                    ConstructInsertPredicateQueryString(predicateVm, resourceAbsoluteUri, predicateAbsoluteUri);
                if (string.IsNullOrEmpty(insertQuery)) return null;
                deleteQuery = ImplementFromGraphClauseToInsertDeleteQuery(deleteQuery, parameters);
                if (string.IsNullOrEmpty(deleteQuery)) return null;
                string finalQuery = deleteQuery + insertQuery;
                return finalQuery;
            }

            return null;
        }

        #endregion

        #region PostSparqlQueries

        public string? GetFinalPostQueryForResource(Parameters parameters, NamedResourceVm namedResourceVm)
        {
            string resourceCurie = namedResourceVm.ResourceCurie;
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(resourceCurie, out string resourceAbsoluteUri))
            {
                string? insertQuery = ConstructInsertResourceQueryString(namedResourceVm, resourceAbsoluteUri);
                if (string.IsNullOrEmpty(insertQuery)) return null;
                insertQuery = ImplementFromGraphClauseToInsertDeleteQuery(insertQuery, parameters);
                return insertQuery;
            }

            return null;
        }

        public string? GetFinalPostQueryForPredicate(Parameters parameters, NamedPredicateVm namedPredicateVm)
        {
            string resourceCurie = parameters.RouteParameters.Resource;
            string predicateCurie = namedPredicateVm.PredicateCurie;
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(resourceCurie, out string resourceAbsoluteUri) &&
                _namespaceFactoryService.GetAbsoluteUriFromQname(predicateCurie, out string predicateAbsoluteUri))
            {
                string? insertQuery =
                    ConstructInsertPredicateQueryString(namedPredicateVm, resourceAbsoluteUri, predicateAbsoluteUri);
                if (string.IsNullOrEmpty(insertQuery)) return null;
                insertQuery = ImplementFromGraphClauseToInsertDeleteQuery(insertQuery, parameters);
                return insertQuery;
            }

            return null;
        }

        #endregion

        #region DeleteSparqlQueries

        public string? GetFinalDeleteQueryForResource(Parameters parameters)
        {
            string resourceCurie = parameters.RouteParameters.Resource;
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(resourceCurie, out string resourceAbsoluteUri))
            {
                SparqlParameterizedString sparqlParameterizedDeleteQuery = new();
                sparqlParameterizedDeleteQuery.CommandText =
                    "DELETE {?s ?p ?o} WHERE {?s ?p ?o. FILTER (?s = @var || ?o = @var)}";
                sparqlParameterizedDeleteQuery.SetUri("var", new Uri(resourceAbsoluteUri));
                string? deleteQuery = sparqlParameterizedDeleteQuery.ToString();
                deleteQuery = ImplementFromGraphClauseToInsertDeleteQuery(deleteQuery, parameters);
                if (string.IsNullOrEmpty(deleteQuery)) return null;
                return deleteQuery;
            }

            return null;
        }

        public string? GetFinalDeleteQueryForPredicate(Parameters parameters)
        {
            string resourceCurie = parameters.RouteParameters.Resource;
            string predicateCurie = parameters.RouteParameters.Predicate;
            if (_namespaceFactoryService.GetAbsoluteUriFromQname(resourceCurie, out string resourceAbsoluteUri) &&
                _namespaceFactoryService.GetAbsoluteUriFromQname(predicateCurie, out string predicateAbsoluteUri))
            {
                SparqlParameterizedString sparqlParameterizedDeleteQuery = new();
                sparqlParameterizedDeleteQuery.CommandText = "DELETE {@sub @pred ?o} WHERE {@sub @pred ?o}";
                sparqlParameterizedDeleteQuery.SetUri("sub", new Uri(resourceAbsoluteUri));
                sparqlParameterizedDeleteQuery.SetUri("pred", new Uri(predicateAbsoluteUri));
                string? deleteQuery = sparqlParameterizedDeleteQuery.ToString();
                deleteQuery = ImplementFromGraphClauseToInsertDeleteQuery(deleteQuery, parameters);
                if (string.IsNullOrEmpty(deleteQuery)) return null;
                return deleteQuery;
            }

            return null;
        }

        #endregion

        #region PrivateMethods

        private string FindSubjectVariableInQuery(string query)
        {
            var s = query.IndexOf("?", StringComparison.Ordinal);
            string a = query.Substring(s);
            var parts = a.Split(" ", 2);
            return parts[0];
        }

        private string? ConstructInsertResourceQueryString(ResourceVm resourceVm, string resourceAbsoluteUri)
        {
            SparqlParameterizedString sparqlParameterizedInsertQuery = new();
            sparqlParameterizedInsertQuery.CommandText = "INSERT DATA {@sub @pred @obj";
            string? insertQuery = null;
            sparqlParameterizedInsertQuery.SetUri("sub", new Uri(resourceAbsoluteUri));
            SparqlParameterizedString sparqlParameterizedInsertSubQuery = new();
            sparqlParameterizedInsertSubQuery.CommandText = "; @pred @obj";
            foreach (var propertyName in resourceVm.Predicates.Keys)
            {
                if (_namespaceFactoryService.GetAbsoluteUriFromQname(propertyName, out string predAbsoluteUri))
                {
                    PredicateContent propertyContent = resourceVm.Predicates[propertyName];
                    insertQuery += PopulateInsertQueryString(propertyContent,
                        sparqlParameterizedInsertQuery, sparqlParameterizedInsertSubQuery,
                        resourceAbsoluteUri, predAbsoluteUri);
                    if (string.IsNullOrEmpty(insertQuery)) return null;
                    insertQuery += "} ";
                }
                else
                {
                    return null;
                }
            }

            return insertQuery;
        }

        private string? ConstructInsertPredicateQueryString(PredicateVm predicateVm, string resourceAbsoluteUri,
            string predicateAbsoluteUri)
        {
            SparqlParameterizedString sparqlParameterizedInsertQuery = new();
            sparqlParameterizedInsertQuery.CommandText = "INSERT DATA {@sub @pred @obj";
            SparqlParameterizedString sparqlParameterizedInsertSubQuery = new();
            sparqlParameterizedInsertSubQuery.CommandText = "; @pred @obj";
            string? insertQuery = null;
            insertQuery += PopulateInsertQueryString(predicateVm,
                sparqlParameterizedInsertQuery, sparqlParameterizedInsertSubQuery, resourceAbsoluteUri,
                predicateAbsoluteUri);
            if (string.IsNullOrEmpty(insertQuery)) return null;
            insertQuery += "}";
            return insertQuery;
        }


        private SparqlParameterizedString ConstructLiteralString(
            SparqlParameterizedString sparqlParameterizedQuery, Literal literal)
        {
            if (literal.Datatype == null && literal.Language == null)
            {
                sparqlParameterizedQuery.SetLiteral("obj", literal.Value);
                return sparqlParameterizedQuery;
            }

            if (literal.Language != null)
            {
                sparqlParameterizedQuery.SetLiteral("obj", literal.Value, literal.Language);
                return sparqlParameterizedQuery;
            }

            if (literal.Datatype != null)
            {
                if (_namespaceFactoryService.GetAbsoluteUriFromQname(literal.Datatype, out var datatypeAbsoluteUri))
                {
                    sparqlParameterizedQuery.SetLiteral("obj", literal.Value, new Uri(datatypeAbsoluteUri));
                }
                else
                {
                    sparqlParameterizedQuery.SetLiteral("obj", literal.Value);
                }
            }

            return sparqlParameterizedQuery;
        }

        private string ApplyQueryStringParametersToSparqlQuery(string query,
            QueryStringParameters queryStringParameters, string sortAndRegexParameter)
        {
            var limit = queryStringParameters.Limit;
            var offset = queryStringParameters.Offset;
            var regex = queryStringParameters.Regex;
            var sort = queryStringParameters.Sort;
            if (regex != null)
            {
                var i = query.LastIndexOf("}", StringComparison.Ordinal);
                query =
                    $"{query.Substring(0, i)}FILTER regex({sortAndRegexParameter}, \"{regex}\") {query.Substring(i)}";
            }

            if (sort != null)
            {
                if (sort.Equals("asc")) query += $" ORDER BY ({sortAndRegexParameter})";
                if (sort.Equals("desc")) query += $" ORDER BY DESC ({sortAndRegexParameter})";
            }

            query += $" LIMIT {limit} OFFSET {offset}";

            return query;
        }

        private string? PopulateInsertQueryString(PredicateContent propertyContent,
            SparqlParameterizedString sparqlParameterizedInsertQuery,
            SparqlParameterizedString sparqlParameterizedInsertSubQuery,
            string resourceAbsoluteUri, string predicateAbsoluteUri)
        {
            sparqlParameterizedInsertQuery.SetUri("sub", new Uri(resourceAbsoluteUri));
            sparqlParameterizedInsertQuery.SetUri("pred", new Uri(predicateAbsoluteUri));
            sparqlParameterizedInsertSubQuery.SetUri("pred", new Uri(predicateAbsoluteUri));
            string? insertQuery = "";
            bool first = true;
            if (propertyContent.Curies != null)
            {
                foreach (var objCurie in propertyContent.Curies)
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
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            if (propertyContent.Literals != null)
            {
                foreach (var literal in propertyContent.Literals)
                {
                    if (first)
                    {
                        sparqlParameterizedInsertQuery =
                            ConstructLiteralString(sparqlParameterizedInsertQuery, literal);
                        insertQuery = sparqlParameterizedInsertQuery.ToString();
                        first = false;
                        continue;
                    }

                    sparqlParameterizedInsertSubQuery =
                        ConstructLiteralString(sparqlParameterizedInsertSubQuery, literal);
                    insertQuery += sparqlParameterizedInsertSubQuery.ToString();
                }
            }

            return insertQuery;
        }

        private string? ImplementFromGraphClauseToSelectQuery(string query, Parameters parameters)
        {
            Endpoint? endpointConfig = _endpointService.GetEndpointConfiguration(parameters.RouteParameters.Endpoint);
            if (endpointConfig == null) return null;
            if (!string.IsNullOrEmpty(parameters.RouteParameters.Graph) && endpointConfig.NamedGraphs != null &&
                endpointConfig.NamedGraphs.Count > 0)
            {
                var graph = endpointConfig.NamedGraphs.Where(x => x.GraphName.Equals(parameters.RouteParameters.Graph))
                    .Select(y => y.Uri).FirstOrDefault();
                if (string.IsNullOrEmpty(graph)) return null;
                query = Regex.Replace(query, "where",
                    $"FROM <{graph}> WHERE",
                    RegexOptions.IgnoreCase);
                return query;
            }

            if (!string.IsNullOrEmpty(endpointConfig.DefaultGraph))
            {
                query = Regex.Replace(query, "where", $"FROM <{endpointConfig.DefaultGraph}> WHERE",
                    RegexOptions.IgnoreCase);
                return query;
            }

            return query;
        }


        private string? ImplementFromGraphClauseToInsertDeleteQuery(string query, Parameters parameters)
        {
            Endpoint? endpointConfig = _endpointService.GetEndpointConfiguration(parameters.RouteParameters.Endpoint);
            if (endpointConfig == null) return null;
            if (!string.IsNullOrEmpty(parameters.RouteParameters.Graph) && endpointConfig.NamedGraphs != null &&
                endpointConfig.NamedGraphs.Count > 0)
            {
                query =
                    $"WITH <{endpointConfig.NamedGraphs.Where(x => x.GraphName.Equals(parameters.RouteParameters.Graph)).Select(y => y.Uri).FirstOrDefault()}> {query}";
                return query;
            }

            if (!string.IsNullOrEmpty(endpointConfig.DefaultGraph))
            {
                query = $"WITH <{endpointConfig.DefaultGraph}> {query}";
                return query;
            }

            return query;
        }

        #endregion
    }
}