#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LinkedData_Api.Data;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Services.Contracts;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Update;

namespace LinkedData_Api.Services
{
    public class EndpointService : IEndpointService
    {
        private readonly ReadOnlyCollection<Endpoint> _endpoints;


        public EndpointService(IDataAccess dataAccess)
        {
            _endpoints = dataAccess.LoadConfigurationFiles();
        }
        
        public Endpoint? GetEndpointConfiguration(string endpointName)
        {
            return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName));
        }

        public string? GetGraphSpecificEntryClassQuery(string endpointName, string graphName)
        {
            return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.EntryClass.FirstOrDefault(x => x.GraphName.Equals(graphName))?.Command;
        }

        public string? GetDefaultEntryClassQuery(string endpointName)
        {
            return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.EntryClass.FirstOrDefault(x => x.GraphName.Equals("default"))?.Command;
        }

        public string? GetEndpointUrl(string endpointName)
        {
            return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.EndpointUrl;
        }

        public string? GetEndpointDefaultGraph(string endpointName)
        {
            return _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.DefaultGraph;
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
        public async Task<IEnumerable<SparqlResult>?> ExecuteSelectSparqlQueryAsync(string endpointName, string? graphName, string query)
        {
            SparqlResultSet? sparqlResultSet = null;
            Endpoint? endpoint = _endpoints.FirstOrDefault(x => x.EndpointName.Equals(endpointName));
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