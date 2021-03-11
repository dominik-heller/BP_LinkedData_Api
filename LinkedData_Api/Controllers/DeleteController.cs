#nullable enable
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
    public class DeleteController : ControllerBase
    {
        private readonly IEndpointService _endpointService;
        private readonly IParametersProcessorService _parametersProcessorService;
        private readonly ISparqlFactoryService _sparqlFactoryService;

        public DeleteController(IEndpointService endpointService,
            IParametersProcessorService parametersProcessorService,
            ISparqlFactoryService sparqlFactoryService)
        {
            _endpointService = endpointService;
            _parametersProcessorService = parametersProcessorService;
            _sparqlFactoryService = sparqlFactoryService;
        }

        #region GeneralInfo

        /// <summary>
        /// Deletes endpoint configuration.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(ApiRoutes.EndpointConfiguration)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(CustomErrorVm), 404)]
        public IActionResult DeleteEndpointConfiguration(string endpoint)
        {
            if (_endpointService.RemoveEndpoint(endpoint)) return NoContent();
            return NotFound(new CustomErrorVm()
            {
                CustomErrorMessage =
                    "Endpoint with given name does not exist!"
            });
        }

        #endregion

        #region ConcreteClass

        /// <summary>
        /// Deletes given resource and all its references.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route(ApiRoutes.DefaultGraphResourcesConcreteResource)]
        [Route(ApiRoutes.NamedGraphResourcesConcreteResource)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(CustomErrorVm), 404)]
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

                query = $"Generated sparql query: {query}.";
            }

            return BadRequest(new CustomErrorVm()
            {
                CustomErrorMessage =
                    $"Resource could not have been deleted! Check endpoint configuration at {UrlHelperClass.GetEndpointUrl(Request.GetEncodedUrl())}.",
                GeneratedQuery = query
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
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(CustomErrorVm), 404)]
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

                query = $"Generated sparql query: {query}.";
            }

            return BadRequest(new CustomErrorVm()
            {
                CustomErrorMessage =
                    $"Predicate could not have been deleted! Check endpoint configuration at {UrlHelperClass.GetEndpointUrl(Request.GetEncodedUrl())}.",
                GeneratedQuery = query
            });
        }
    }

    #endregion
}