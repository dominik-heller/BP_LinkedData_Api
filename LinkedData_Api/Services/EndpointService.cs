#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LinkedData_Api.Data;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Services.Contracts;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Patterns;
using VDS.RDF.Update;
using VDS.RDF.Update.Commands;

namespace LinkedData_Api.Services
{
    public class EndpointService : IEndpointService
    {
        private readonly INamespaceFactoryService _namespaceFactoryService;
        private readonly ReadOnlyCollection<Endpoint> _endpoints;
        private const int DefaultSparqlEndpointConnectionTimeout = 10000;

        private const string DefaultSparqlEndpointAcceptHeaders =
            "application/sparql-results+xml,application/sparql-results+json,text/boolean,application/rdf+xml,text/xml,application/xml,application/json,text/json,application/rdf+json,text/csv,text/comma-separated-values";

        public EndpointService(IDataAccess dataAccess, INamespaceFactoryService namespaceFactoryService)
        {
            _namespaceFactoryService = namespaceFactoryService;
            _endpoints = dataAccess.GetEndpointsConfiguration();
            AddEndpointsNamespaces(_endpoints.Select(x => x.Namespaces));
        }

        private void AddEndpointsNamespaces(IEnumerable<List<Namespace>> endpointsNamespaces)
        {
            foreach (var namespaces in endpointsNamespaces)
            {
                if (namespaces != null)
                {
                    _namespaceFactoryService.AddNewPrefixes(namespaces);
                }
            }
        }

        public Endpoint? GetEndpointConfiguration(string endpointName)
        {
            return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName));
        }

        public string? GetEndpointUrl(string endpointName)
        {
            return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.EndpointUrl;
        }

        public string? GetEndpointDefaultGraph(string endpointName)
        {
            return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.DefaultGraph;
        }

        public string? GetEntryClassQuery(string endpoint, string? graph)
        {
            if (graph == null)
                return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpoint))?.EntryClass?
                    .FirstOrDefault(x => x.GraphName.Equals("default"))?.Command;
            return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpoint))?.EntryClass?
                .FirstOrDefault(x => x.GraphName.Equals(graph))?.Command;
        }

        public string? GetEntryResourceQuery(string endpoint, string? graph)
        {
            var z = _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpoint))?.EntryResource
                .FirstOrDefault(x => x.GraphName.Equals("default"))?.Command;
            var y = _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpoint))?.EntryResource
                .FirstOrDefault(x => x.GraphName.Equals(graph))?.Command;

            if (graph == null)
                return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpoint))?.EntryResource
                    .FirstOrDefault(x => x.GraphName.Equals("default"))?.Command;
            return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpoint))?.EntryResource
                .FirstOrDefault(x => x.GraphName.Equals(graph))?.Command;
        }

        public IEnumerable<NamedGraph>? GetEndpointGraphs(string endpointName)
        {
            string? defaultGraph =
                _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.DefaultGraph;
            List<NamedGraph>? graphsList =
                _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.NamedGraphs;
            NamedGraph namedGraph = new NamedGraph() {GraphName = "default", Uri = defaultGraph};
            graphsList?.Insert(0, namedGraph);
            return graphsList;
        }

        public async Task<IEnumerable<SparqlResult>?> ExecuteSelectSparqlQueryAsync(string endpointName,
            string? graphName, string query)
        {
            Endpoint? endpoint = _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName));
            if (endpoint != null && endpoint.SupportedMethods.Sparql10.Equals("yes"))
            {
                SparqlRemoteEndpoint sparqlEndpoint;
                if (graphName != null && endpoint.NamedGraphs == null) return null;
                if (graphName != null && !endpoint.NamedGraphs.Exists(x => x.GraphName.Equals(graphName))) return null;
                try
                {
                    sparqlEndpoint = new SparqlRemoteEndpoint(new Uri(endpoint.EndpointUrl));
                    sparqlEndpoint.ResultsAcceptHeader = DefaultSparqlEndpointAcceptHeaders;
                    sparqlEndpoint.Timeout = DefaultSparqlEndpointConnectionTimeout;
                    var sparqlResultSet = await Task.Run(() => sparqlEndpoint.QueryWithResultSet(query));
                    if (sparqlResultSet?.Results.Count == 0) return null;
                    return sparqlResultSet?.Results;
                }
                catch (RdfException e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }

            return null;
        }

        public async Task<bool> ExecuteUpdateSparqlQueryAsync(string endpointName,
            string? graphName, string query)
        {
            Endpoint? endpoint = _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName));
            if (endpoint != null && endpoint.SupportedMethods.Sparql11.Equals("yes"))
            {
                SparqlRemoteEndpoint sparqlEndpoint;
                if (graphName != null && endpoint.NamedGraphs == null) return false;
                if (graphName != null && !endpoint.NamedGraphs.Exists(x => x.GraphName.Equals(graphName))) return false;
                try
                {
                    sparqlEndpoint = new SparqlRemoteEndpoint(new Uri(endpoint.EndpointUrl));
                    sparqlEndpoint.Timeout = DefaultSparqlEndpointConnectionTimeout;
                    sparqlEndpoint.HttpMode = "POST";
                    var httpWebResponse = await Task.Run(() => sparqlEndpoint.QueryRaw(query));
                    if (httpWebResponse.StatusCode == HttpStatusCode.OK) return true;
                }
                catch (RdfException)
                {
                    return false;
                }
            }

            return false;
        }
    }
}