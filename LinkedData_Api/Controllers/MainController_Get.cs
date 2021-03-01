#nullable enable

using System.Threading.Tasks;
using LinkedData_Api.Model.ParameterDto;
using LinkedData_Api.Model.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LinkedData_Api.Controllers
{
    public partial class MainController
    {
        #region Classes

        /// <summary>
        /// Returns list of classes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.DefaultGraphClasses)]
        [Route(ApiRoutes.NamedGraphClasses)]
        [ProducesResponseType(typeof(CurieVm), 200)]
        [ProducesResponseType(typeof(ErrorVm), 404)]
        public async Task<IActionResult> GetClasses([FromQuery] QueryStringParametersDto queryStringParametersDto)
        {
            ParametersDto parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalQuery(
                _endpointService.GetEntryClassQuery(parameters.RouteParameters.Endpoint,
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
                    "No results were found. Check endpoint/graph name and consequently entry class query for given endpoint."
            });
        }

        #endregion


        #region Resources

        /// <summary>
        /// Returns list of resources.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.DefaultGraphResources)]
        [Route(ApiRoutes.NamedGraphResources)]
        [ProducesResponseType(typeof(CurieVm), 200)]
        [ProducesResponseType(typeof(ErrorVm), 404)]
        public async Task<IActionResult> GetResources([FromQuery] QueryStringParametersDto queryStringParametersDto)
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


        #region ConcreteClass

        /// <summary>
        /// Returns list of resources for given class.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.DefaultGraphConcreteClass)]
        [Route(ApiRoutes.NamedGraphConcreteClass)]
        [ProducesResponseType(typeof(CurieVm), 200)]
        [ProducesResponseType(typeof(ErrorVm), 404)]
        public async Task<IActionResult> GetConcreteClass([FromQuery] QueryStringParametersDto queryStringParametersDto)
        {
            ParametersDto parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalSelectQueryForClass(parameters);
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

            return NotFound(new ErrorVm() {ErrorMessage = "No results were found for given class."});
        }

        #endregion

        #region ConcreteResource

        /// <summary>
        /// Returns detailed view for given resource.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.DefaultGraphResourcesConcreteResource)]
        [Route(ApiRoutes.NamedGraphResourcesConcreteResource)]
        [Route(ApiRoutes.DefaultGraphClassesConcreteResource)]
        [Route(ApiRoutes.NamedGraphClassesConcreteResource)]
        [ProducesResponseType(typeof(ResourceVm), 200)]
        [ProducesResponseType(typeof(ErrorVm), 404)]
        public async Task<IActionResult> GetConcreteResource(
            [FromQuery] QueryStringParametersDto queryStringParametersDto)
        {
            ParametersDto parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalSelectQueryForResource(parameters);
            if (query != null)
            {
                var sparqlResults = await _endpointService.ExecuteSelectSparqlQueryAsync(
                    parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph, query);
                if (sparqlResults != null)
                {
                    ResourceVm resourceVm = _resultFormatterService.FormatSparqlResultToResourceDetail(sparqlResults);
                    return Ok(resourceVm);
                }
            }

            return NotFound(new ErrorVm() {ErrorMessage = "No results were found for given resource."});
        }

        #endregion


        #region Predicate

        /// <summary>
        /// Returns detailed view for given resource.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.DefaultGraphClassStartConcreteResourcePredicate)]
        [Route(ApiRoutes.NamedGraphClassStartConcreteResourcePredicate)]
        [Route(ApiRoutes.DefaultGraphResourceStartConcreteResourcePredicate)]
        [Route(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate)]
        [ProducesResponseType(typeof(PredicateVm), 200)]
        [ProducesResponseType(typeof(ErrorVm), 404)]
        public async Task<IActionResult> GetConcreteResourcePredicate(
            [FromQuery] QueryStringParametersDto queryStringParametersDto)
        {
            ParametersDto parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalSelectQueryForPredicate(parameters);
            if (query != null)
            {
                var sparqlResults = await _endpointService.ExecuteSelectSparqlQueryAsync(
                    parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph, query);
                if (sparqlResults != null)
                {
                    PredicateVm predicateVm =
                        _resultFormatterService.FormatSparqlResultToCurieAndLiteralList(
                            parameters.RouteParameters.Predicate, sparqlResults);
                    return Ok(predicateVm);
                }
            }

            return NotFound(new ErrorVm() {ErrorMessage = "No results were found for given resource and predicate."});
        }

        #endregion

        #region RecursivePart

        [HttpGet]
        [Route(ApiRoutes.DefaultGraphClassStartRecursiveRoute)]
        [Route(ApiRoutes.NamedGraphClassStartRecursiveRoute)]
        [Route(ApiRoutes.DefaultGraphResourceStartRecursiveRoute)]
        [Route(ApiRoutes.NamedGraphResourceStartRecursiveRoute)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult RecursiveRouteRedirect()
        {
            ParametersDto parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            if (parameters.RouteParameters.Predicate == null)
                return Redirect(_parametersProcessorService.ReduceUrl(Request.GetEncodedUrl(), "resource"));
            return Redirect(_parametersProcessorService.ReduceUrl(Request.GetEncodedUrl(), "predicate"));
        }

        #endregion

/*PŮVODNÍ ROUTOVÁNÍ
//př: https://localhost:5001/api/dbpedia/class/dbo:country/dbr:Germany/dbo:Capital/dbr:Berlin
[HttpGet(ApiRoutes.DefaultGraphClassRoute)]
public async Task<IActionResult> Get_DefaultGraph_ClassStart()
{
  ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
      Request.QueryString);
  var endpoint = pd.RouteParameters.Endpoint;
  if (pd.RouteParameters.Class == null) //pouze dotaz na api/endpoint/class => vrátí všechny třídy
  {
      string? query =
          _sparqlFactoryService.GetFinalQuery(
              _endpointService.GetDefaultEntryClassQuery(endpoint), pd);
      if (query != null)
      {
          IEnumerable<SparqlResult>? sparqlResults = await
              _endpointService.ExecuteSelectSparqlQueryAsync(endpoint, null, query);
          if (sparqlResults != null)
          {
              CurieVm curiesVm = _resultFormatterService.FormatSparqlResultToList(sparqlResults);
              if (curiesVm != null)
              {
                  return Ok(curiesVm);
              }
          }
      }
  }

  return NotFound("nenalezeno");
}

//př: https://localhost:5001/api/endpoint1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
[HttpGet(ApiRoutes.DefaultGraphResourceRoute)]
public ActionResult Get_DefaultGraph_ResourcesStart()
{
  ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
      Request.QueryString);
  return Ok(pd);
}

//př: https://localhost:5001/api/virtuoso/ovm/class/dbo:Country/dbr:Germany/dbo:Capital/dbr:Berlin
[HttpGet(ApiRoutes.NamedGraphClassRoute)]
public async Task<IActionResult> Get_GraphSpecific_ClassStart()
{
  ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
      Request.QueryString);
  string? query = _sparqlFactoryService.GetFinalQuery(
      _endpointService.GetGraphSpecificEntryClassQuery(pd.RouteParameters.Endpoint,
          pd.RouteParameters.Graph), pd);
  if (query != null)
  {
      IEnumerable<SparqlResult>? sparqlResults = await
          _endpointService.ExecuteSelectSparqlQueryAsync(pd.RouteParameters.Endpoint,
              pd.RouteParameters.Graph, query);
      if (sparqlResults != null)
      {
          CurieVm curiesVm = _resultFormatterService.FormatSparqlResultToList(sparqlResults);
          if (curiesVm != null) return Ok(curiesVm);
      }
  }

  return NotFound("nenalezeno");
}

//př: https://localhost:5001/api/endpoint1/graph1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
[HttpGet(ApiRoutes.NamedGraphResourceRoute)]
public ActionResult Get_GraphSpecific_ResourceStart()
{
  ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
      Request.QueryString);
  return Ok(pd);
}

/* Pro proces parametrů zbytečný (není to IO operace => neopouští pamět a není to ani náročné) X pro dotaz na SPARQL ENDPOINT UŽ ANO
public async Task<IActionResult> Get_DefaultGraph_ClassStartAsync()
{
   //TOHLE ŠPATNĚ => nikdy nevolat Task.Run z controlleru => použije to vlákno pro requesty na api,c ož je přesně to co nechceme
  // ParameterDto pd0 = await Task.Run(() => Services.ParametersProcessor.ProcessParameters(Request.RouteValues, Request.QueryString));
  //TOHLE SPRÁVNĚ
  ParameterDto pd = await Services.ParametersProcessor.ProcessParametersAsync(Request.RouteValues,
      Request.QueryString);
  return Ok(pd);
  // $"CLASS_DefaultGraph\nEndpoint: {endpoint}\t Graph: Default\t \tClass_Id:{class_id}\nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
}
*/
    }
}