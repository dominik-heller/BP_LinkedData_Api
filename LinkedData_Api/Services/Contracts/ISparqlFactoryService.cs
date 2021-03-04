#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkedData_Api.Model.Domain;
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
        string? GetFinalQuery(string? query, QueryStringParameters queryStringParameters);

        /// <summary>
        /// Return select sparql query for concrete {class} endpoint based on route and querystring parameters or null.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string? GetFinalSelectQueryForClass(Parameters parameters);

        /// <summary>
        /// Return select sparql query for concrete {resource} endpoint based on route and querystring parameters or null.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string? GetFinalSelectQueryForResource(Parameters parameters);

        /// <summary>
        /// Return select sparql query for concrete {predicate} endpoint based on route and querystring parameters or null.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string? GetFinalSelectQueryForPredicate(Parameters parameters);

        /// <summary>
        /// Returns update sparql query deleting resource (if exists) and creating new one.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="resourceVm"></param>
        /// <returns></returns>
        string? GetFinalPutQueryForResource(Parameters parameters, ResourceVm resourceVm);

        /// <summary>
        /// Returns update sparql query deleting resource predicate (if exists) and creating new one.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="predicateVm"></param>
        /// <returns></returns>
        string? GetFinalPutQueryForPredicate(Parameters parameters, PredicateVm predicateVm);

        /// <summary>
        /// Returns update sparql query creating new resource or appending to existing one.
        /// </summary>
        /// <param name="namedResourceVm"></param>
        /// <returns></returns>
        string? GetFinalPostQueryForResource(NamedResourceVm namedResourceVm);

        /// <summary>
        /// Returns update sparql query creating new predicate for given resource or appending to existing one.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="namedPredicateVm"></param>
        /// <returns></returns>
        string? GetFinalPostQueryForPredicate(Parameters parameters, NamedPredicateVm namedPredicateVm);
        
        /// <summary>
        /// Returns update sparql query deleting given resource and all of its references.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string? GetFinalDeleteQueryForResource(Parameters parameters);

        /// <summary>
        /// Returns update sparql query deleting predicate of given resource.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string? GetFinalDeleteQueryForPredicate(Parameters parameters);
    }
}