#nullable enable
using System;
using System.Threading.Tasks;
using LinkedData_Api.Helpers;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Services.Contracts;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LinkedData_Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PutController : ControllerBase
    {
        private readonly IEndpointService _endpointService;
        private readonly IParametersProcessorService _parametersProcessorService;
        private readonly ISparqlFactoryService _sparqlFactoryService;

        public PutController(IEndpointService endpointService, IParametersProcessorService parametersProcessorService,
            ISparqlFactoryService sparqlFactoryService)
        {
            _endpointService = endpointService;
            _parametersProcessorService = parametersProcessorService;
            _sparqlFactoryService = sparqlFactoryService;
        }

        #region ConcreteClass

        /// <summary>
        /// Creates new resource or replaces existing one.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route(ApiRoutes.DefaultGraphResourcesConcreteResource)]
        [Route(ApiRoutes.NamedGraphResourcesConcreteResource)]
        [ProducesResponseType(typeof(ResourceVm), 201)]
        [ProducesResponseType(typeof(ValidationErrorVm), 400)]
        public async Task<IActionResult> PutResource([FromBody] ResourceVm resourceVm)
        {
            Parameters parameters =
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
            }

            return BadRequest(new ValidationErrorVm()
            {
                CustomError = new CustomErrorVm()
                {
                    CustomErrorMessage =
                        $"Resource could not have been created! Check selected endpoint configuration at {UrlHelperClass.GetEndpointUrl(Request.GetEncodedUrl())}.",
                    GeneratedQuery = query
                }
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
        [ProducesResponseType(typeof(PredicateVm), 201)]
        [ProducesResponseType(typeof(ValidationErrorVm), 400)]
        public async Task<IActionResult> PutPredicate(PredicateVm predicateVm)
        {
            Parameters parameters =
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
            }

            return BadRequest(new ValidationErrorVm()
            {
                CustomError = new CustomErrorVm()
                {
                    CustomErrorMessage =
                        $"Predicate could not have been created! Check selected endpoint configuration at {UrlHelperClass.GetEndpointUrl(Request.GetEncodedUrl())}.",
                    GeneratedQuery = query
                }
            });
        }

        #endregion
    }
}