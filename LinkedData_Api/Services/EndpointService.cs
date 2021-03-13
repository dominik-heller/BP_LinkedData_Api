#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LinkedData_Api.Data;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Services.Contracts;
using VDS.RDF;
using VDS.RDF.Query;

namespace LinkedData_Api.Services
{
    public class EndpointService : IEndpointService
    {
        private readonly INamespaceFactoryService _namespaceFactoryService;
        private readonly ConcurrentDictionary<string, Endpoint> _endpoints;
        private const int DefaultSparqlEndpointConnectionTimeout = 30000;

        private const string DefaultSparqlEndpointAcceptHeaders =
            "application/sparql-results+xml,application/sparql-results+json,text/boolean,application/rdf+xml,text/xml,application/xml,application/json,text/json,application/rdf+json,text/csv,text/comma-separated-values";

        public EndpointService(IDataAccess dataAccess, INamespaceFactoryService namespaceFactoryService)
        {
            _namespaceFactoryService = namespaceFactoryService;
            _endpoints = new ConcurrentDictionary<string, Endpoint>(dataAccess.GetEndpointsConfiguration());
            AddEndpointsNamespaces(_endpoints.Select(x => x.Value.Namespaces));
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
            if (_endpoints.TryGetValue(endpointName, out var endpoint)) return endpoint;
            return null;
        }

        public string? GetEntryClassQuery(string endpointName, string? graph)
        {
            if (_endpoints.TryGetValue(endpointName, out var endpoint))
            {
                if (graph == null)
                    return endpoint?.EntryClass?.FirstOrDefault(x => x.GraphName.Equals("default"))?.Command;
                return endpoint?.EntryClass?.FirstOrDefault(x => x.GraphName.Equals(graph))?.Command;
            }

            return null;
        }

        public string? GetEntryResourceQuery(string endpointName, string? graph)
        {
            if (_endpoints.TryGetValue(endpointName, out var endpoint))
            {
                if (graph == null)
                    return endpoint?.EntryResource.FirstOrDefault(x => x.GraphName.Equals("default"))
                        ?.Command;

                if (endpoint?.NamedGraphs.FirstOrDefault(x => x.GraphName.Equals(graph)) != null)
                {
                    //this condition enables searching in named graphs even without entry resource sparql query defined
                    if (endpoint?.EntryResource.FirstOrDefault(x => x.GraphName.Equals(graph))?.Command == null)
                        return "SELECT ?s WHERE {?s ?p ?o}";
                    return endpoint?.EntryResource.FirstOrDefault(x => x.GraphName.Equals(graph))?.Command;
                }
            }

            return null;
        }

        public IEnumerable<NamedGraph>? GetEndpointGraphs(string endpointName)
        {
            if (_endpoints.TryGetValue(endpointName, out var endpoint))
            {
                string? defaultGraph =
                    endpoint?.DefaultGraph;
                List<NamedGraph>? graphsList =
                    endpoint?.NamedGraphs;
                NamedGraph namedGraph = new NamedGraph() {GraphName = "default", Uri = defaultGraph};
                graphsList?.Insert(0, namedGraph);
                return graphsList;
            }

            return null;
        }

        public async Task<IEnumerable<SparqlResult>?> ExecuteSelectSparqlQueryAsync(string endpointName,
            string? graphName, string query)
        {
            if (!_endpoints.TryGetValue(endpointName, out var endpoint)) return null;
            if (!endpoint.SupportedMethods.Sparql10.Equals("yes")) return null;
            SparqlRemoteEndpoint sparqlEndpoint;
            if (graphName != null && endpoint.NamedGraphs == null) return null;
            if (graphName != null && !endpoint.NamedGraphs.Exists(x => x.GraphName.Equals(graphName)))
                return null;
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
               // Console.WriteLine(e);
                return null;
            }
        }

        public async Task<bool> ExecuteUpdateSparqlQueryAsync(string endpointName,
            string? graphName, string query)
        {
            if (!_endpoints.TryGetValue(endpointName, out var endpoint)) return false;
            if (!endpoint.SupportedMethods.Sparql11.Equals("yes")) return false;
            SparqlRemoteEndpoint sparqlEndpoint;
            if (graphName != null && endpoint.NamedGraphs == null) return false;
            if (graphName != null && !endpoint.NamedGraphs.Exists(x => x.GraphName.Equals(graphName)))
                return false;
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
                // Console.WriteLine(e);
                return false;
            }

            return false;
        }

        public bool AddEndpoint(Endpoint endpoint, out Endpoint adjustedEndpoint)
        {
            adjustedEndpoint = CheckAdditionalConditionsAndAdjustEndpointConfiguration(endpoint);
            if (adjustedEndpoint.Namespaces != null && adjustedEndpoint.Namespaces.Count > 0)
                _namespaceFactoryService.AddNewPrefixes(adjustedEndpoint.Namespaces);
            return _endpoints.TryAdd(endpoint.EndpointName, adjustedEndpoint);
        }

        public bool RemoveEndpoint(string endpointName)
        {
            return _endpoints.TryRemove(endpointName, out var e);
        }

        private Endpoint CheckAdditionalConditionsAndAdjustEndpointConfiguration(Endpoint endpoint)
        {
            if (endpoint.SupportedMethods == null)
            {
                endpoint.SupportedMethods = new SupportedMethods() {Sparql10 = "yes", Sparql11 = "no"};
            }

            if (endpoint.EntryResource == null || endpoint.EntryResource.Count == 0)
            {
                endpoint.EntryResource = new List<EntryResource>
                {
                    new() {GraphName = "default", Command = "SELECT ?s WHERE { ?s ?p ?o }"}
                };
            }

            return endpoint;
        }
    }
}