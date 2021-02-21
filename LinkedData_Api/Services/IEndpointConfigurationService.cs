#nullable enable
using System.Collections.Generic;
using LinkedData_Api.DataModel.EndpointConfigurationDto;
using VDS.RDF.Query;

namespace LinkedData_Api.Services
{
    public interface IEndpointConfigurationService
    {
        void ProcessConfigurationFiles();
        EndpointDto? GetEndpointConfiguration(string endpointName);
        string? GetEndpointUrl(string endpointName);
        string? GetEndpointDefaultGraph(string endpointName);
        IEnumerable<NamedGraph>? GetEndpointGraphs(string endpointName);
        SparqlRemoteEndpoint GetSparqlSelectRemoteSparqlEndpoint(string endpointName);
    }
}