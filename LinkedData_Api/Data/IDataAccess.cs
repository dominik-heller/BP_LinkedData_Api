using System.Collections.Generic;
using System.Collections.ObjectModel;
using LinkedData_Api.Model.Domain;
using VDS.RDF;

namespace LinkedData_Api.Data
{
    public interface IDataAccess
    {
        ReadOnlyCollection<Endpoint> ProcessConfigurationFiles();
        ThreadSafeQNameOutputMapper LoadNamespacesFromFile();
    }
}