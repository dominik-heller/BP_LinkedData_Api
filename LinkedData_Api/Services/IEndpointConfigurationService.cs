#nullable enable
namespace LinkedData_Api.Services
{
    public interface IEndpointConfigurationService
    {
        void ProcessConfigurationFiles();
        string? GetEndpointUrl(string endpointName);
    }
}