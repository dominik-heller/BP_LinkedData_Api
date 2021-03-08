using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LinkedData_Api.Model.Domain;
using VDS.RDF;

namespace LinkedData_Api.Data
{
    public interface IDataAccess
    {
        ConcurrentBag<Endpoint> GetEndpointsConfiguration();
        NamespaceMapper GetNamespaces();
        NamespaceMapper LoadNamespacesFile(string pathToConfigurationFiles);
        List<Endpoint> LoadConfigurationFiles(string pathToConfigurationFiles);

    }
}