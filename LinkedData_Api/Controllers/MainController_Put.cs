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
            ParametersDto parameters = _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalPutQueryForResource(parameters, resourceVm);
            /*   if (!string.IsNullOrEmpty(classQuery))
               {
                   query = _sparqlFactoryService.GetFinalPutQueryForClass(parameters, classQuery, curieVm);
                   if (query != null)
                   {
                       
                                       var sparqlResults = await _endpointService.ExecuteSelectSparqlQueryAsync(parameters.RouteParameters.Endpoint, parameters.RouteParameters.Graph, query);
                                       if (sparqlResults != null)
                                       {
                                           CurieVm curiesVm = _resultFormatterService.FormatSparqlResultToCurieList(sparqlResults);
                                           return Ok(curiesVm);
                                       }
                   }
               }
   */
            return NotFound(new ErrorVm() {ErrorMessage = "No results were found for given class."});
        }

        #endregion
    }
}