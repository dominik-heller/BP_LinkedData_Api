using System.Collections.Generic;
using LinkedData_Api.Model.Domain;
using VDS.RDF;

namespace LinkedData_Api.Services.Contracts
{
    public interface INamespaceFactoryService
    {
        /// <summary>
        /// Tries to get uri from given qname and returns true if succeeded
        /// </summary>
        /// <param name="qname"></param>
        /// <param name="absoluteUri"></param>
        /// <returns></returns>
        bool GetAbsoluteUriFromQname(string qname, out string absoluteUri);

        /// <summary>
        /// Tries to get qname from given uri and returns true if succeeded
        /// </summary>
        /// <param name="qname"></param>
        /// <param name="uri">
        bool GetQnameFromAbsoluteUri(string uri, out string qname);

        /// <summary>
        /// Adds new prefixes to NamespaceMapper instance
        /// </summary>
        /// <param name="namespaces"></param>
        public void AddNewPrefixes(IEnumerable<Namespace> namespaces);

        /// <summary>
        /// Tries to get namespace for given prefix and returns true if succeeded
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="namespaceUri"></param>
        /// <returns></returns>
        public bool GetNamespaceUriByPrefix(string prefix, out string namespaceUri);
    }
}