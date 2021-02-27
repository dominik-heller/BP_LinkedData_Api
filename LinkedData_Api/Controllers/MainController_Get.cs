#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkedData_Api.Model.ParameterDto;
using LinkedData_Api.Model.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using VDS.RDF.Query;

namespace LinkedData_Api.Controllers
{
    public partial class MainController
    {
        #region Classes

        /// <summary>
        /// Returns list of classes.
        /// </summary>
        /// <param name="limit">Default = 50</param>
        /// <param name="offset">Default = 0</param>
        /// <param name="regex"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.DefaultGraphClasses)]
        [Route(ApiRoutes.NamedGraphClasses)]
        [ProducesResponseType(typeof(CurieVm), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetClasses([FromQuery] string? limit, [FromQuery] string? offset,
            [FromQuery] string? regex, [FromQuery] string? sort)
        {
            ParameterDto parameters =
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
                    CurieVm curiesVm = _resultFormatterService.FormatSparqlResultToList(sparqlResults);
                    return Ok(curiesVm);
                }
            }

            return NotFound("Not found!");
        }

/*
        [HttpGet(ApiRoutes.NamedGraphClasses)]
        public async Task<IActionResult> Get_NamedGraphClasses([FromRoute] string endpoint, [FromRoute] string graph,
            [FromQuery] string? limit, [FromQuery] string? offset, [FromQuery] string? regex, [FromQuery] string? sort)
        {
            string? query = _sparqlFactoryService.GetFinalQuery(_endpointService.GetDefaultEntryClassQuery(endpoint),
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString));
            if (query != null)
            {
                var sparqlResults = await _endpointService.ExecuteSelectSparqlQueryAsync(endpoint, graph, query);
                if (sparqlResults != null)
                {
                    CurieVm curiesVm = _resultFormatterService.FormatSparqlResultToList(sparqlResults);
                    return Ok(curiesVm);
                }
            }
            return NotFound("Not found!");
        }
*/

        #endregion


        #region Resources

        /// <summary>
        /// Returns list of resources.
        /// </summary>
        /// <param name="limit">Default = 50</param>
        /// <param name="offset">Default = 0</param>
        /// <param name="regex"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.DefaultGraphResources)]
        [Route(ApiRoutes.NamedGraphResources)]
        [ProducesResponseType(typeof(CurieVm), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetResources([FromQuery] string? limit,
            [FromQuery] string? offset, [FromQuery] string? regex, [FromQuery] string? sort)
        {
            ParameterDto parameters =
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
                    CurieVm curiesVm = _resultFormatterService.FormatSparqlResultToList(sparqlResults);
                    return Ok(curiesVm);
                }
            }

            return NotFound("Not found!");
        }

/*
        [HttpGet(ApiRoutes.NamedGraphResources)]
        public string Get_NamedGraphResources()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_NamedGraphResources";
        }
*/

        #endregion


        #region ConcreteClass

        /// <summary>
        /// Returns list of resources for given class.
        /// </summary>
        /// <param name="limit">Default = 50</param>
        /// <param name="offset">Default = 0</param>
        /// <param name="regex"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.DefaultGraphConcreteClass)]
        [Route(ApiRoutes.NamedGraphConcreteClass)]
        [ProducesResponseType(typeof(CurieVm), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetConcreteClass([FromQuery] string? limit, [FromQuery] string? offset,
            [FromQuery] string? regex, [FromQuery] string? sort)
        {
            ParameterDto parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalQueryForClass(parameters);
            if (query != null)
            {
                var sparqlResults =
                    await _endpointService.ExecuteSelectSparqlQueryAsync(parameters.RouteParameters.Endpoint,
                        parameters.RouteParameters.Graph, query);
                if (sparqlResults != null)
                {
                    CurieVm curiesVm = _resultFormatterService.FormatSparqlResultToList(sparqlResults);
                    return Ok(curiesVm);
                }
            }

            return NotFound("Not found!");
        }

/*
        [HttpGet(ApiRoutes.NamedGraphConcreteClass)]
        public string Get_NamedGraphConcreteClass()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_NamedGraphConcreteClass";
        }
*/

        #endregion

        #region ConcreteResource

        /// <summary>
        /// Returns detailed view for given resource.
        /// </summary>
        /// <param name="limit">Default = 50</param>
        /// <param name="offset">Default = 0</param>
        /// <param name="regex"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.DefaultGraphResourcesConcreteResource)]
        public async Task<IActionResult> GetConcreteResource([FromQuery] string? limit, [FromQuery] string? offset,
            [FromQuery] string? regex, [FromQuery] string? sort)
        {
            ParameterDto parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalQueryForResource(parameters);
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

            return NotFound("Not found!");
            }

/*
        [HttpGet(ApiRoutes.NamedGraphResourcesConcreteResource)]
        public string Get_NamedGraphResourcesConcreteResource()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_NamedGraphClassesConcreteResource";
        }
*/

            #endregion

/*
        #region ConcreteResourceForClass

        [HttpGet(ApiRoutes.DefaultGraphClassesConcreteResource)]
        public string Get_DefaultGraphClassesConcreteResource()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_DefaultGraphClassesConcreteResource";
        }

        [HttpGet(ApiRoutes.NamedGraphClassesConcreteResource)]
        public string Get_NamedGraphClassesConcreteResource()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_NamedGraphClassesConcreteResource";
        }

        #endregion

        #region Predicate

        [HttpGet(ApiRoutes.DefaultGraphClassStartConcreteResourcePredicate)]
        public string Get_DefaultGraphClassConcreteResourcePredicate()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_DefaultGraphClassConcreteResourcePredicate";
        }

        [HttpGet(ApiRoutes.NamedGraphClassStartConcreteResourcePredicate)]
        public string Get_NamedGraphClassStartConcreteResourcePredicate()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_NamedGraphClassStartConcreteResourcePredicate";
        }

        [HttpGet(ApiRoutes.DefaultGraphResourceStartConcreteResourcePredicate)]
        public string Get_DefaultGraphResourceStartConcreteResourcePredicate()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return " Get_DefaultGraphResourceStartConcreteResourcePredicate";
        }

        [HttpGet(ApiRoutes.NamedGraphResourceStartConcreteResourcePredicate)]
        public string Get_NamedGraphResourceStartConcreteResourcePredicate()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_NamedGraphResourceStartConcreteResourcePredicate";
        }

        #endregion

        #region RecursiveRoutes

        [HttpGet(ApiRoutes.DefaultGraphClassStartRecursiveRoute)]
        public string Get_DefaultGraphClassStartRecursiveRoute()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_DefaultGraphClassStartRecursiveRoute";
        }

        [HttpGet(ApiRoutes.NamedGraphClassStartRecursiveRoute)]
        public string Get_NamedGraphClassStartRecursiveRoute()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_NamedGraphClassStartRecursiveRoute";
        }

        [HttpGet(ApiRoutes.DefaultGraphResourceStartRecursiveRoute)]
        public string Get_DefaultGraphResourceStartRecursiveRoute()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_DefaultGraphResourceStartRecursiveRoute";
        }

        [HttpGet(ApiRoutes.NamedGraphResourceStartRecursiveRoute)]
        public string Get_NamedGraphResourceStartRecursiveRoute()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "Get_NamedGraphResourceStartRecursiveRoute";
        }

        #endregion

/*
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