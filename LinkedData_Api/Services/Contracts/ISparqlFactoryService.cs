#nullable enable
using System.Collections.Generic;
using VDS.RDF.Query;

namespace LinkedData_Api.Services.Contracts
{
    public interface ISparqlFactoryService
    {
        string? GetDefaultEntryClassQuery(string endpointName);
        IEnumerable<SparqlResult>? ExecuteSelectSparqlQuery(string endpointName, string? graphname, string query);
        string? GetGraphSpecificEntryClassQuery(string endpointName, string graphName);
    }
}