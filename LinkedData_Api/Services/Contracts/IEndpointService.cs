#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkedData_Api.Model.Domain;
using VDS.RDF.Query;

namespace LinkedData_Api.Services.Contracts
{
    public interface IEndpointService
    {
        /// <summary>
        /// Returns given endpoint
        /// </summary>
        /// <param name="endpointName"></param>
        /// <returns></returns>
        Endpoint? GetEndpointConfiguration(string endpointName);

        /// <summary>
        /// Returns entry class sparql query for given endpoint or null
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        string? GetEntryClassQuery(string endpoint, string? graph);

        /// <summary>
        /// Returns entry resource sparql query for given endpoint or null
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        string? GetEntryResourceQuery(string endpoint, string? graph);

        /// <summary>
        /// Returns all graphs for given endpoint or null
        /// </summary>
        /// <param name="endpointName"></param>
        /// <returns></returns>
        IEnumerable<NamedGraph>? GetEndpointGraphs(string endpointName);

        /// <summary>
        /// Sends select sparql query for execution to given endpoint and returns sparql results or null
        /// </summary>
        /// <param name="endpointName"></param>
        /// <param name="graphName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<SparqlResult>?> ExecuteSelectSparqlQueryAsync(string endpointName, string? graphName,
            string query);

        /// <summary>
        /// Sends update sparql query for execution to given endpoint and returns sparql results or null
        /// </summary>
        /// <param name="endpointName"></param>
        /// <param name="graphName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<bool> ExecuteUpdateSparqlQueryAsync(string endpointName, string? graphName, string query);

        /// <summary>
        /// Tries to store given endpoint on the server and returns true if succeeded
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="adjusted"></param>
        /// <returns></returns>
        bool AddEndpoint(Endpoint endpoint, out Endpoint adjusted);

        /// <summary>
        /// Tries to remove given endpoint from the server and returns true if succeeded
        /// </summary>
        /// <param name="endpointName"></param>
        /// <returns></returns>
        bool RemoveEndpoint(string endpointName);

        /// <summary>
        /// Returns names of all registered endpoints.
        /// </summary>
        /// <returns></returns>
        List<string>? GetEndpointNames();
    }
}