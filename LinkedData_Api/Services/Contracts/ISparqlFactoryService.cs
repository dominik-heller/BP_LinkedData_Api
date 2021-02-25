#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace LinkedData_Api.Services.Contracts
{
    public interface ISparqlFactoryService
    {
        string? GetDefaultEntryClassQuery(string endpointName);
        Task<IEnumerable<SparqlResult>?> ExecuteRemoteSelectSparqlQueryAsync(string endpointName, string? graphname, string query);
        string? GetGraphSpecificEntryClassQuery(string endpointName, string graphName);
    }
}