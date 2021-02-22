#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LinkedData_Api.Data;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Services.Contracts;
using VDS.RDF.Query;
using VDS.RDF.Update;

namespace LinkedData_Api.Services
{
    public class EndpointService : IEndpointService
    {
        private readonly ReadOnlyCollection<Endpoint> _endpoints;


        public EndpointService(IDataAccess dataAccess)
        {
            _endpoints = dataAccess.ProcessConfigurationFiles();
       //     CreateRemoteEndpointConnectionObjects(_endpointDtos);
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


    }
}