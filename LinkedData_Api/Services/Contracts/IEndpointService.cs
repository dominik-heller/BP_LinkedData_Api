#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkedData_Api.Model.Domain;
using VDS.RDF.Query;

namespace LinkedData_Api.Services.Contracts
{
    public interface IEndpointService
    {
        Endpoint? GetEndpointConfiguration(string endpointName);
        string? GetEndpointUrl(string endpointName);
        string? GetEndpointDefaultGraph(string endpointName);
        string? GetEntryClassQuery(string endpoint, string? graph);
        string? GetEntryResourceQuery(string endpoint, string? graph);
        IEnumerable<NamedGraph>? GetEndpointGraphs(string endpointName);
        Task<IEnumerable<SparqlResult>?> ExecuteSelectSparqlQueryAsync(string endpointName, string? graphName, string query);
        Task<bool> ExecuteUpdateSparqlQueryAsync(string endpointName, string? graphName, string query);


    }
}