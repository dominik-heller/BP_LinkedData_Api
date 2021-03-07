using System.Collections.Generic;
using LinkedData_Api.Model.Domain;
using VDS.RDF;

namespace LinkedData_Api.Services.Contracts
{
    public interface INamespaceFactoryService
    {
        bool GetAbsoluteUriFromQname(string qname, out string absoluteUri);
        bool GetQnameFromAbsoluteUri(string uri, out string qname);
        public void AddNewPrefixes(IEnumerable<Namespace> namespaces);
        public bool GetNamespaceUriByPrefix(string prefix, out string namespaceUri);
    }
}