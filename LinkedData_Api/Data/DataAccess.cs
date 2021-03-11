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
        private readonly Dictionary<string, Endpoint> _configurationFilesDictionary;
        private readonly NamespaceMapper _namespaceMapper;


        public DataAccess()
        {
            _configurationFilesDictionary =
                new Dictionary<string, Endpoint>(
                    LoadConfigurationFiles(@"Data/JsonFiles/EndpointConfiguration"));
            _namespaceMapper = new NamespaceMapper();
            _namespaceMapper = LoadNamespacesFile(@"Data/JsonFiles/Namespaces/namespaces.json");
        }

        public Dictionary<string, Endpoint> GetEndpointsConfiguration()
        {
            return _configurationFilesDictionary;
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

        public Dictionary<string, Endpoint> LoadConfigurationFiles(string pathToConfigurationFiles)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            Dictionary<string, Endpoint> endpoints = new Dictionary<string, Endpoint>();
            string[] fileEntries = Directory.GetFiles(pathToConfigurationFiles);
            foreach (string fileName in fileEntries)
            {
                try
                {
                    Endpoint endpoint = JsonConvert.DeserializeObject<Endpoint>(File.ReadAllText(fileName));
                    if (CheckAndAdjustEndpointConfigurationFile(endpoint, endpoints, out Endpoint checkedEndpoint))
                    {
                        endpoints.Add(endpoint.EndpointName, checkedEndpoint);
                    }
                }
                catch (JsonSerializationException e)
                {
                    Console.WriteLine(
                        "Invalid endpoint configuration file settings. Endpoint will be ignored.\n\nError: " + e);
                }
            }

            return endpoints;
        }

        private bool CheckAndAdjustEndpointConfigurationFile(Endpoint endpoint, Dictionary<string, Endpoint> endpoints,
            out Endpoint checkedEndpoint)
        {
            checkedEndpoint = endpoint;

            if (string.IsNullOrWhiteSpace(endpoint.EndpointName) || string.IsNullOrWhiteSpace(endpoint.EndpointUrl))
            {
                Console.WriteLine(
                    "Invalid endpoint configuration file settings. Missing endpoint name or url. Endpoint settings will be ignored.");
                return false;
            }

            if (endpoints.Any(x => x.Key.Contains(endpoint.EndpointName)))
            {
                Console.WriteLine(
                    "Endpoint with given name already exists. Endpoint settings will be ignored.");
                return false;
            }

            if (!CheckIfIsUrl(endpoint.EndpointUrl))
            {
                Console.WriteLine(
                    "Endpoint URL is not valid. Endpoint settings will be ignored.");
                return false;
            }

            if (endpoint.Namespaces != null && (endpoint.Namespaces.Any(x =>
                string.IsNullOrWhiteSpace(x.Prefix)) || endpoint.Namespaces.Any(y =>
                    string.IsNullOrWhiteSpace(y.Uri)) || endpoint.Namespaces.Any(z => !CheckIfIsUrl(z.Uri))))
            {
                Console.WriteLine(
                    "Namespace definition is not valid. Uri must be valid url, prefix and uri must not be empty. Endpoint settings will be ignored.");
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
            if (string.IsNullOrWhiteSpace(endpoint.SupportedMethods?.Sparql10) ||
                string.IsNullOrWhiteSpace(endpoint.SupportedMethods?.Sparql11) ||
                !values.Contains(endpoint.SupportedMethods?.Sparql10) ||
                !values.Contains(endpoint.SupportedMethods?.Sparql11))
            {
                Console.WriteLine(
                    "Invalid endpoint configuration file settings. Missing supported methods information (\"yes\" | \"no\" expected). Endpoint settings will be ignored.");
                return false;
            }

            return true;
        }

        private bool CheckIfIsUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                     && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}