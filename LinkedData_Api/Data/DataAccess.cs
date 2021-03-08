using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AngleSharp.Common;
using LinkedData_Api.Model.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VDS.RDF;

namespace LinkedData_Api.Data
{
    public class DataAccess : IDataAccess
    {
        private readonly ConcurrentBag<Endpoint> _threadSafeConfigurationFilesCollection;
        //Get Endpoint Configuration from Files and dispose them in ReadOnly = Thread-Safe Collection  
        private readonly NamespaceMapper _namespaceMapper;


        public DataAccess()
        {
            _threadSafeConfigurationFilesCollection =
                new ConcurrentBag<Endpoint>(LoadConfigurationFiles(@"Data/JsonFiles/EndpointConfiguration"));
            _namespaceMapper = new NamespaceMapper();
            _namespaceMapper=LoadNamespacesFile(@"Data/JsonFiles/Namespaces/namespaces.json");
        }

        public ConcurrentBag<Endpoint> GetEndpointsConfiguration()
        {
            return _threadSafeConfigurationFilesCollection;
        }

        public NamespaceMapper GetNamespaces()
        {
            return _namespaceMapper;
        }

        public NamespaceMapper LoadNamespacesFile(string pathToConfigurationFiles)
        {
            JObject o = JObject.Parse(File.ReadAllText(pathToConfigurationFiles));
            foreach (var v in o)
            {
                if (v.Value != null) _namespaceMapper.AddNamespace(v.Key, new Uri(v.Value.ToString()));
            }

            return _namespaceMapper;
        }

        public List<Endpoint> LoadConfigurationFiles(string pathToConfigurationFiles)
        {
            List<Endpoint> endpointDtos = new List<Endpoint>();
            string[] fileEntries = Directory.GetFiles(pathToConfigurationFiles);
            foreach (string fileName in fileEntries)
            {
                try
                {
                    Endpoint endpoint = JsonConvert.DeserializeObject<Endpoint>(File.ReadAllText(fileName));
                    if (CheckAndAdjustEndpointConfigurationFile(endpoint, endpointDtos, out Endpoint checkedEndpoint))
                    {
                        endpointDtos.Add(checkedEndpoint);
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

        private bool CheckAndAdjustEndpointConfigurationFile(Endpoint endpoint, List<Endpoint> endpoints,
            out Endpoint checkedEndpoint)
        {
            checkedEndpoint = endpoint;

            if (string.IsNullOrWhiteSpace(endpoint.EndpointName) || string.IsNullOrWhiteSpace(endpoint.EndpointUrl))
            {
                Console.WriteLine(
                    "Invalid endpoint configuration file settings. Missing endpoint name or url. Endpoint settings will be ignored.");
                return false;
            }

            if (endpoints.Any(x => x.EndpointName.Equals(endpoint.EndpointName)))
            {
                Console.WriteLine(
                    "Endpoint with given name already exists. Endpoint settings will be ignored.");
                return false;
            }

            if (!(Uri.TryCreate(endpoint.EndpointUrl, UriKind.Absolute, out var uriResult)
                  && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
            {
                Console.WriteLine(
                    "Endpoint URL is not valid. Endpoint settings will be ignored.");
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
                    "Invalid endpoint configuration file settings. Missing supported methods information (\"yes\" | \"no\" expected). Endpoint settings will be ignored.");
                return false;
            }

            return true;
        }
    }
}