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
        public string? GetGraphSpecificEntryClassQuery(string endpointName, string graphName);
        public string? GetDefaultEntryClassQuery(string endpointName);
        string? GetEndpointUrl(string endpointName);
        string? GetEndpointDefaultGraph(string endpointName);
        IEnumerable<NamedGraph>? GetEndpointGraphs(string endpointName);
        Task<IEnumerable<SparqlResult>?> ExecuteSelectSparqlQueryAsync(string endpointName,
            string? graphName, string query);
    }
}