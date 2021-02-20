namespace LinkedData_Api.Services
{
    public interface INamespaceFactoryService
    {
        void LoadNamespacesFromFile();
        bool GetAbsoluteUriFromQname(string qname, out string absoluteUri);
        bool GetQnameFromAbsoluteUri(string uri, out string qname);
    }
}