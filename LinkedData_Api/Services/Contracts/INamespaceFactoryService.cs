namespace LinkedData_Api.Services.Contracts
{
    public interface INamespaceFactoryService
    {
        bool GetAbsoluteUriFromQname(string qname, out string absoluteUri);
        bool GetQnameFromAbsoluteUri(string uri, out string qname);
    }
}