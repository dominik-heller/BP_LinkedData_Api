using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using LinkedData_Api.Model.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VDS.RDF;

namespace LinkedData_Api.Data
{
    public class DataAccess : IDataAccess
    {
        //Get Endpoint Configuration from Files and dispose them in ReadOnly = Thread-Safe Collection  
        public ReadOnlyCollection<Endpoint> LoadConfigurationFiles()
        {
            List<Endpoint> endpointDtos = new List<Endpoint>();
            string[] fileEntries = Directory.GetFiles("Data/JsonFiles/EndpointConfiguration");
            foreach (string fileName in fileEntries)
            {
                Endpoint endpoint = JsonConvert.DeserializeObject<Endpoint>(File.ReadAllText(fileName));
                //TODO: Check for same endpoint_names (bud vytvorit provizorni name (jako u namespacu) a nebo vynechat a vyhodit vyjimku) 
                endpointDtos.Add(endpoint);
            }

            ReadOnlyCollection<Endpoint> readOnlyCollection = new ReadOnlyCollection<Endpoint>(endpointDtos);
            return readOnlyCollection;
        }

        public ThreadSafeQNameOutputMapper LoadNamespacesFile()
        {
            ThreadSafeQNameOutputMapper namespaceMapper = new ThreadSafeQNameOutputMapper(new NamespaceMapper());
            JObject o = JObject.Parse(File.ReadAllText(@"Data/JsonFiles/Namespaces/namespaces.json"));
            foreach (var v in o)
            {
                // Debug.WriteLine(v.Key+":"+v.Value);
                if (v.Value != null) namespaceMapper.AddNamespace(v.Key, new Uri(v.Value.ToString()));
            }

            return namespaceMapper;
        }
    }
}