using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LinkedData_Api.Model.Domain;
using VDS.RDF;

namespace LinkedData_Api.Data
{
    public interface IDataAccess
    {
        /// <summary>
        /// Returns all endpoint configurations loaded from configuration files
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Endpoint> GetEndpointsConfiguration();

        /// <summary>
        /// Returns NamespaceMapper instance with namespaces and prefixes loaded from namespace file
        /// </summary>
        /// <returns></returns>
        NamespaceMapper GetNamespaces();

        /// <summary>
        /// Loads namespace file into NamespaceMapper instance
        /// </summary>
        /// <param name="pathToConfigurationFiles"></param>
        /// <returns></returns>
        NamespaceMapper LoadNamespacesFile(string pathToConfigurationFiles);

        /// <summary>
        /// Loads all configuration files into Dictionary
        /// </summary>
        /// <param name="pathToConfigurationFiles"></param>
        /// <returns></returns>
        Dictionary<string, Endpoint> LoadConfigurationFiles(string pathToConfigurationFiles);
    }
}