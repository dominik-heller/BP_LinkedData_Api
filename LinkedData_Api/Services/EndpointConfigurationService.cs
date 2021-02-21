#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using LinkedData_Api.DataModel.EndpointConfigurationDto;
using Newtonsoft.Json;
using VDS.RDF.Query;
using VDS.RDF.Update;

namespace LinkedData_Api.Services
{
    public class EndpointConfigurationService : IEndpointConfigurationService
    {
        private readonly List<EndpointDto> _endpointsDtos;
        private readonly Dictionary<string, SparqlRemoteEndpoint> _sparqlSelectRemoteEndpoints;
        private readonly Dictionary<string, SparqlRemoteUpdateEndpoint> _sparqlUpdateRemoteEndpoints;

        public EndpointConfigurationService()
        {
            _sparqlSelectRemoteEndpoints = new Dictionary<string, SparqlRemoteEndpoint>();
            _sparqlUpdateRemoteEndpoints = new Dictionary<string, SparqlRemoteUpdateEndpoint>();
            _endpointsDtos = new List<EndpointDto>();
            ProcessConfigurationFiles();
        }

        public void ProcessConfigurationFiles()
        {
            string[] fileEntries = Directory.GetFiles("JsonFiles/EndpointConfiguration");
            foreach (string fileName in fileEntries)
            {
                EndpointDto endpoint = JsonConvert.DeserializeObject<EndpointDto>(File.ReadAllText(fileName));
                //TODO: Check for same endpoint_names (bud vytvorit provizorni name (jako u namespacu) a nebo vynechat a vyhodit vyjimku) 
                _endpointsDtos.Add(endpoint);
            }

            CreateRemoteEndpointConnectionObjects(_endpointsDtos);
        }

        private void CreateRemoteEndpointConnectionObjects(IEnumerable<EndpointDto> endpointDtos)
        {
            foreach (var endpoint in endpointDtos)
            {
                if (endpoint.SupportedMethods.Sparql10.Equals("yes"))
                    _sparqlSelectRemoteEndpoints.Add(endpoint.EndpointName,new SparqlRemoteEndpoint(new Uri(endpoint.EndpointUrl)));
                if (endpoint.SupportedMethods.Sparql11.Equals("yes"))
                    _sparqlUpdateRemoteEndpoints.Add(endpoint.EndpointName,new SparqlRemoteUpdateEndpoint(new Uri(endpoint.EndpointUrl)));
            }
        }


        public EndpointDto? GetEndpointConfiguration(string endpointName)
        {
            return _endpointsDtos.FirstOrDefault(x => x.EndpointName.Equals(endpointName));
        }

        public string? GetEndpointUrl(string endpointName)
        {
            return _endpointsDtos.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.EndpointUrl;
        }

        public string? GetEndpointDefaultGraph(string endpointName)
        {
            return _endpointsDtos.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.DefaultGraph;
        }

        public IEnumerable<NamedGraph>? GetEndpointGraphs(string endpointName)
        {
            string? defaultGraph =
                _endpointsDtos.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.DefaultGraph;
            List<NamedGraph>? graphsList =
                _endpointsDtos.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.NamedGraphs;
            NamedGraph namedGraph = new NamedGraph() {GraphName = "default", Uri = defaultGraph};
            graphsList?.Insert(0, namedGraph);
            return graphsList;
        }

        public SparqlRemoteEndpoint GetSparqlSelectRemoteSparqlEndpoint(string endpointName)
        {
            return _sparqlSelectRemoteEndpoints[endpointName];
        }
    }
}