#nullable enable
using System.Threading.Tasks;
using LinkedData_Api.Model.ParameterDto;
using LinkedData_Api.Model.ViewModels;
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
        public async Task<IActionResult> PutConcreteClass(ResourceVm resourceVm)
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
                    return Ok("Resource created!");
                }

                return NotFound(new ErrorVm()
                {
                    ErrorMessage =
                        "Resource could not have been created! Check if SPARQL endpoint supports UPDATE operation."
                });
            }

            return NotFound(new ErrorVm()
            {
                ErrorMessage =
                    "Resource could not have been created! Check if SPARQL submitted query has correct syntax."
            });
        }

        #endregion
    }
}