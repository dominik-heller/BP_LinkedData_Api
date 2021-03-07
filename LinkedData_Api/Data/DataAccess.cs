using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LinkedData_Api.Model.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VDS.RDF;

namespace LinkedData_Api.Data
{
    public class DataAccess : IDataAccess
    {
        private readonly ReadOnlyCollection<Endpoint> _readOnlyCollection;
        //Get Endpoint Configuration from Files and dispose them in ReadOnly = Thread-Safe Collection  

        public DataAccess()
        {
            _readOnlyCollection = new ReadOnlyCollection<Endpoint>(LoadConfigurationFiles());
        }

        public ReadOnlyCollection<Endpoint> GetEndpointsConfiguration()
        {
            return _readOnlyCollection;
        }

        public ThreadSafeQNameOutputMapper LoadNamespacesFile()
        {
            ThreadSafeQNameOutputMapper namespaceMapper = new ThreadSafeQNameOutputMapper(new NamespaceMapper());
            JObject o = JObject.Parse(File.ReadAllText(@"Data/JsonFiles/Namespaces/namespaces.json"));
            foreach (var v in o)
            {
                if (v.Value != null) namespaceMapper.AddNamespace(v.Key, new Uri(v.Value.ToString()));
            }

            return namespaceMapper;
        }

        private List<Endpoint> LoadConfigurationFiles()
        {
            List<Endpoint> endpointDtos = new List<Endpoint>();
            string[] fileEntries = Directory.GetFiles("Data/JsonFiles/EndpointConfiguration");
            foreach (string fileName in fileEntries)
            {
                try
                {
                    Endpoint endpoint = JsonConvert.DeserializeObject<Endpoint>(File.ReadAllText(fileName));
                    if (CheckAndAdjustEndpointConfiguration(endpoint, out Endpoint _endpoint))
                    {
                        endpointDtos.Add(_endpoint);
                    }
                }
                catch (JsonSerializationException e)
                {
                    Console.WriteLine(
                        "Invalid endpoint configuration file settings. Endpoint will be ignored.\n\nError: " + e);
                }
            }

            return endpointDtos;
        }

        private bool CheckAndAdjustEndpointConfiguration(Endpoint endpoint, out Endpoint _endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint.EndpointName) || string.IsNullOrWhiteSpace(endpoint.EndpointUrl))
            {
                Console.WriteLine(
                    "Invalid endpoint configuration file settings. Missing endpoint name or url. Endpoint will be ignored.");
                _endpoint = endpoint;
                return false;
            }

            if (endpoint.EntryResource == null)
            {
                endpoint.EntryResource = new List<EntryResource>
                {
                    new() {GraphName = "default", Command = "SELECT ?s WHERE { ?s ?p ?o }"}
                };
            }

            if (endpoint.SupportedMethods == null)
            {
                endpoint.SupportedMethods = new SupportedMethods() {Sparql10 = "yes", Sparql11 = "no"};
            }

            var values = new[] {"yes", "no"};
            if (string.IsNullOrWhiteSpace(endpoint.SupportedMethods.Sparql10) ||
                string.IsNullOrWhiteSpace(endpoint.SupportedMethods.Sparql11) ||
                !values.Contains(endpoint.SupportedMethods.Sparql10) ||
                !values.Contains(endpoint.SupportedMethods.Sparql11))
            {
                Console.WriteLine(
                    "Invalid endpoint configuration file settings. Missing supported methods information (\"yes\" | \"no\" expected). Endpoint will be ignored.");
                _endpoint = endpoint;
                return false;
            }

            _endpoint = endpoint;
            return true;
        }
    }
}