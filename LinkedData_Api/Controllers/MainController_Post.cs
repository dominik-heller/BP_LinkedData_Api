#nullable enable
using System.Threading.Tasks;
using LinkedData_Api.Model.ParameterDto;
using LinkedData_Api.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LinkedData_Api.Controllers
{
    public partial class MainController
    {
        
/*
        #region Resources

        /// <summary>
        /// Returns list of resources.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.DefaultGraphResources)]
        [Route(ApiRoutes.NamedGraphResources)]
        public async Task<IActionResult> PostResource(NamedResourceVm namedResourceVm)
        {
            ParametersDto parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalQuery(
                _endpointService.GetEntryResourceQuery(parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph), parameters.QueryStringParametersDto);
            if (query != null)
            {
                var sparqlResults =
                    await _endpointService.ExecuteSelectSparqlQueryAsync(parameters.RouteParameters.Endpoint,
                        parameters.RouteParameters.Graph, query);
                if (sparqlResults != null)
                {
                    CurieVm curiesVm = _resultFormatterService.FormatSparqlResultToCurieList(sparqlResults);
                    return Ok(curiesVm);
                }
            }

            return NotFound(new ErrorVm()
            {
                ErrorMessage =
                    "No results were found. Check endpoint/graph name and consequently entry resource query for given endpoint."
            });
        }

        #endregion
  */
    }
}