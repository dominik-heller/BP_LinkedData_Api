#nullable enable
using System;
using System.Threading.Tasks;
using LinkedData_Api.Model.ParameterDto;
using LinkedData_Api.Model.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LinkedData_Api.Controllers
{
    public partial class MainController
    {
        #region ConcreteClass

        /// <summary>
        /// Creates new resource or replaces existing one.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route(ApiRoutes.DefaultGraphResourcesConcreteResource)]
        [Route(ApiRoutes.NamedGraphResourcesConcreteResource)]
        [ProducesResponseType(typeof(ResourceVm),201)]
        [ProducesResponseType(typeof(ErrorVm), 400)]
        public async Task<IActionResult> PutConcreteClass([FromBody] ResourceVm resourceVm)
        {
            ParametersDto parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalPutQueryForResource(parameters, resourceVm);

            if (query != null)
            {
                bool successful = await _endpointService.ExecuteUpdateSparqlQueryAsync(
                    parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph, query);
                if (successful)
                {
                    return Created(new Uri(Request.GetEncodedUrl()), resourceVm);
                }

                return BadRequest(new ErrorVm()
                {
                    ErrorMessage =
                        "Resource could not have been created! Check if SPARQL endpoint supports UPDATE operation."
                });
            }

            return BadRequest(new ErrorVm()
            {
                ErrorMessage =
                    "Resource could not have been created! Check if SPARQL submitted query has correct syntax."
            });
        }

        #endregion


        #region Predicate

        /// <summary>
        /// Creates new predicate for given resource or replaces existing one.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route(ApiRoutes.DefaultGraphResourceStartConcreteResourcePredicate)]
        [Route(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate)]
        [ProducesResponseType(typeof(PredicateVm),201)]
        [ProducesResponseType(typeof(ErrorVm), 400)]
        public async Task<IActionResult> PutConcreteResourcePredicate(PredicateVm predicateVm)
        {
            ParametersDto parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalPutQueryForPredicate(parameters, predicateVm);

            if (query != null)
            {
                bool successful = await _endpointService.ExecuteUpdateSparqlQueryAsync(
                    parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph, query);
                if (successful)
                {
                    return Created(new Uri(Request.GetEncodedUrl()), predicateVm);
                }

                return BadRequest(new ErrorVm()
                {
                    ErrorMessage =
                        "Resource could not have been created! Check if SPARQL endpoint supports UPDATE operation and if submitted values has correct semantic syntax."
                });
            }

            return BadRequest(new ErrorVm()
            {
                ErrorMessage =
                    "Resource could not have been created! Check if SPARQL submitted values has correct semantic syntax."
            });
        }

        #endregion
    }
}