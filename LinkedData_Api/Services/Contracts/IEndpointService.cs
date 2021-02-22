#nullable enable
using System.Collections.Generic;
using LinkedData_Api.Model.Domain;
using VDS.RDF.Query;

namespace LinkedData_Api.Services.Contracts
{
    public interface IEndpointService
    {
        Endpoint? GetEndpointConfiguration(string endpointName);
        string? GetEndpointUrl(string endpointName);
        string? GetEndpointDefaultGraph(string endpointName);
        IEnumerable<NamedGraph>? GetEndpointGraphs(string endpointName);
    }
}