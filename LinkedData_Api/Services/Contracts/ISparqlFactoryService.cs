#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkedData_Api.Model.ParameterDto;
using LinkedData_Api.Model.ViewModels;
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
        /// Return select sparql query for concrete {class} endpoint based on route and querystring parameters or null.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string? GetFinalSelectQueryForClass(ParametersDto parameters);

        /// <summary>
        /// Return select sparql query for concrete {resource} endpoint based on route and querystring parameters or null.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string? GetFinalSelectQueryForResource(ParametersDto parameters);

        /// <summary>
        /// Return select sparql query for concrete {predicate} endpoint based on route and querystring parameters or null.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string? GetFinalSelectQueryForPredicate(ParametersDto parameters);

        /// <summary>
        /// Returns update sparql query deleting resource (if exists) and creating new one.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="resourceVm"></param>
        /// <returns></returns>
        string? GetFinalPutQueryForResource(ParametersDto parameters, ResourceVm resourceVm);

        /// <summary>
        /// Returns update sparql query deleting resource predicate (if exists) and creating new one.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="predicateVm"></param>
        /// <returns></returns>
        string? GetFinalPutQueryForPredicate(ParametersDto parameters, PredicateVm predicateVm);
    }
}