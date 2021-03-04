﻿#nullable enable
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
    public class DeleteController : ControllerBase
    {
        
        private readonly IEndpointService _endpointService;
        private readonly IParametersProcessorService _parametersProcessorService;
        private readonly ISparqlFactoryService _sparqlFactoryService;
        private readonly IResultFormatterService _resultFormatterService;

        public DeleteController(IEndpointService endpointService, IParametersProcessorService parametersProcessorService,
            ISparqlFactoryService sparqlFactoryService, IResultFormatterService resultFormatterService)
        {
            _endpointService = endpointService;
            _parametersProcessorService = parametersProcessorService;
            _sparqlFactoryService = sparqlFactoryService;
            _resultFormatterService = resultFormatterService;
        }
        

        #region ConcreteClass

        /// <summary>
        /// Deletes given resource and all its references.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route(ApiRoutes.DefaultGraphResourcesConcreteResource)]
        [Route(ApiRoutes.NamedGraphResourcesConcreteResource)]
        [ProducesResponseType( 204)]
        [ProducesResponseType(typeof(ErrorVm), 404)]
        public async Task<IActionResult> DeleteResource()
        {
            Parameters parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalDeleteQueryForResource(parameters);

            if (query != null)
            {
                bool successful = await _endpointService.ExecuteUpdateSparqlQueryAsync(
                    parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph, query);
                if (successful)
                {
                    return NoContent();
                }

                return NotFound(new ErrorVm()
                {
                    ErrorMessage =
                        "Resource could not have been deleted! Check if SPARQL endpoint supports UPDATE operation."
                });
            }

            return NotFound(new ErrorVm()
            {
                ErrorMessage =
                    "Resource could not have been created! Check if submitted values has correct semantic syntax."
            });
        }

        #endregion
        
        #region Predicate

        /// <summary>
        /// Deletes given predicate of concrete resource.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route(ApiRoutes.DefaultGraphResourceStartConcreteResourcePredicate)]
        [Route(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate)]
        [ProducesResponseType( 204)]
        [ProducesResponseType(typeof(ErrorVm), 404)]
        public async Task<IActionResult> DeletePredicate()
        {
            Parameters parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalDeleteQueryForPredicate(parameters);

            if (query != null)
            {
                bool successful = await _endpointService.ExecuteUpdateSparqlQueryAsync(
                    parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph, query);
                if (successful)
                {
                    return NoContent();
                }

                return NotFound(new ErrorVm()
                {
                    ErrorMessage =
                        "Resource could not have been deleted! Check if SPARQL endpoint supports UPDATE operation."
                });
            }

            return NotFound(new ErrorVm()
            {
                ErrorMessage =
                    "Resource could not have been deleted! Check if submitted values has correct semantic syntax."
            });
        }
        #endregion

        
        
        
    }
}