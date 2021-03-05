#nullable enable
using System;
using System.Threading.Tasks;
using LinkedData_Api.Model.ParameterDto;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Services.Contracts;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LinkedData_Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class PostController : ControllerBase
    {
        private readonly IEndpointService _endpointService;
        private readonly IParametersProcessorService _parametersProcessorService;
        private readonly ISparqlFactoryService _sparqlFactoryService;
        private readonly IResultFormatterService _resultFormatterService;

        public PostController(IEndpointService endpointService, IParametersProcessorService parametersProcessorService,
            ISparqlFactoryService sparqlFactoryService, IResultFormatterService resultFormatterService)
        {
            _endpointService = endpointService;
            _parametersProcessorService = parametersProcessorService;
            _sparqlFactoryService = sparqlFactoryService;
            _resultFormatterService = resultFormatterService;
        }


        #region Resources

        /// <summary>
        /// Creates new resource or appends to existing one.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.DefaultGraphResources)]
        [Route(ApiRoutes.NamedGraphResources)]
        [ProducesResponseType(typeof(NamedResourceVm), 201)]
        [ProducesResponseType(typeof(ErrorVm), 400)]
        public async Task<IActionResult> PostResource(NamedResourceVm namedResourceVm)
        {
            if (!ModelState.IsValid) Console.WriteLine("Invalid Model");
            Parameters parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalPostQueryForResource(parameters, namedResourceVm);
            if (query != null)
            {
                bool successful = await _endpointService.ExecuteUpdateSparqlQueryAsync(
                    parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph, query);
                if (successful)
                {
                    return Created(new Uri(Request.GetEncodedUrl()), namedResourceVm);
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
                    "Resource could not have been created! Check if submitted values has correct semantic syntax."
            });
        }

        #endregion

        #region Predicates

        /// <summary>
        /// Creates new predicate for given resource or appends to existing one.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.DefaultGraphResourcesConcreteResource)]
        [Route(ApiRoutes.NamedGraphResourcesConcreteResource)]
        [ProducesResponseType(typeof(NamedPredicateVm), 201)]
        [ProducesResponseType(typeof(ErrorVm), 400)]
        public async Task<IActionResult> PostPredicate(NamedPredicateVm namedPredicateVm)
        {
            Parameters parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalPostQueryForPredicate(parameters, namedPredicateVm);
            if (query != null)
            {
                bool successful = await _endpointService.ExecuteUpdateSparqlQueryAsync(
                    parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph, query);
                if (successful)
                {
                    return Created(new Uri(Request.GetEncodedUrl()), namedPredicateVm);
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
                    "Resource could not have been created! Check if submitted values has correct semantic syntax."
            });
        }

        #endregion
    }
}