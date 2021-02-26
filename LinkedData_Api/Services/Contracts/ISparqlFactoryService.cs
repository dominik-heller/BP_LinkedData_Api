#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkedData_Api.Model.ParameterDto;
using VDS.RDF.Query;

namespace LinkedData_Api.Services.Contracts
{
    public interface ISparqlFactoryService
    {
        /// <summary>
        /// Return updated sparql query based on querystring parameters or null.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryStringParameters"></param>
        /// <returns></returns>
        string? GetFinalQuery(string? query, QueryStringParametersDto queryStringParameters);

        /// <summary>
        /// Return new sparql query for {class} endpoint based on route and querystring parameters or null.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string? GetFinalQueryForClass(ParameterDto parameters);
    }
}