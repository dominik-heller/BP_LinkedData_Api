#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LinkedData_Api.Helpers;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Services.Contracts;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NamedGraph = LinkedData_Api.Model.ViewModels.NamedGraph;


namespace LinkedData_Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class GetController : ControllerBase
    {
        private readonly IEndpointService _endpointService;
        private readonly IParametersProcessorService _parametersProcessorService;
        private readonly ISparqlFactoryService _sparqlFactoryService;
        private readonly IResultFormatterService _resultFormatterService;
        private readonly INamespaceFactoryService _namespaceFactoryService;
        private readonly IMapper _mapper;

        public GetController(IEndpointService endpointService, IParametersProcessorService parametersProcessorService,
            ISparqlFactoryService sparqlFactoryService, IResultFormatterService resultFormatterService,
            INamespaceFactoryService namespaceFactoryService, IMapper mapper)
        {
            _endpointService = endpointService;
            _parametersProcessorService = parametersProcessorService;
            _sparqlFactoryService = sparqlFactoryService;
            _resultFormatterService = resultFormatterService;
            _namespaceFactoryService = namespaceFactoryService;
            _mapper = mapper;
        }


        #region GeneralInfo

        /// <summary>
        /// Returns endpoint configuration information.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.EndpointConfiguration)]
        [ProducesResponseType(typeof(EndpointVm), 200)]
        [ProducesResponseType(typeof(ErrorVm), 404)]
        public IActionResult Get_EndpointConfiguration([FromRoute] string endpoint)
        {
            var info = _endpointService.GetEndpointConfiguration(endpoint);
            if (info != null) return Ok(_mapper.Map<Endpoint, EndpointVm>(info));
            return NotFound(new ErrorVm() {ErrorMessage = "Endpoint does not exist."});
        }

        /// <summary>
        /// Returns endpoint's named graphs.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.EndpointGraphs)]
        [ProducesResponseType(typeof(List<NamedGraph>), 200)]
        [ProducesResponseType(typeof(ErrorVm), 404)]
        public IActionResult Get_GraphsForEndpoint([FromRoute] string endpoint)
        {
            var graphs = _endpointService.GetEndpointGraphs(endpoint);
            if (graphs != null) return Ok(graphs);
            return NotFound(new ErrorVm()
            {
                ErrorMessage =
                    $"No graphs found. Check selected endpoint configuration at {UrlHelperClass.GetEndpointUrl(Request.GetEncodedUrl())}"
            });
        }

        /// <summary>
        /// Returns namespace URI for given prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.EndpointNamespacePrefix)]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorVm), 404)]
        public IActionResult GetNamespaces([FromRoute] string prefix)
        {
            if (_namespaceFactoryService.GetNamespaceUriByPrefix(prefix, out var namespaceUri))
            {
                return Ok(namespaceUri);
            }

            return NotFound(new ErrorVm() {ErrorMessage = "Namespace uri for given prefix not found!"});
        }

        #endregion

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
        public async Task<IActionResult> GetClasses([FromQuery] QueryStringParameters queryStringParametersDto)
        {
            Parameters parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalQuery(
                _endpointService.GetEntryClassQuery(parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph), parameters);
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


                return NotFound(new ErrorVm()
                    {ErrorMessage = $"No results were found! Generated sparql query: {query}"});
            }

            return NotFound(new ErrorVm()
            {
                ErrorMessage =
                    $"No results could have been acquired due to invalid request parameters! Check submitted URL or endpoint configuration at {UrlHelperClass.GetEndpointUrl(Request.GetEncodedUrl())}."
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
        public async Task<IActionResult> GetResources([FromQuery] QueryStringParameters queryStringParametersDto)
        {
            Parameters parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            string? query = _sparqlFactoryService.GetFinalQuery(
                _endpointService.GetEntryResourceQuery(parameters.RouteParameters.Endpoint,
                    parameters.RouteParameters.Graph), parameters);
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


                return NotFound(new ErrorVm()
                    {ErrorMessage = $"No results were found! Generated sparql query: {query}"});
            }

            return NotFound(new ErrorVm()
            {
                ErrorMessage =
                    $"No results could have been acquired due to invalid request parameters! Check submitted URL or endpoint configuration at {UrlHelperClass.GetEndpointUrl(Request.GetEncodedUrl())}."
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
        public async Task<IActionResult> GetConcreteClass([FromQuery] QueryStringParameters queryStringParametersDto)
        {
            Parameters parameters =
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


                return NotFound(new ErrorVm()
                    {ErrorMessage = $"No results were found! Generated sparql query: {query}"});
            }

            return NotFound(new ErrorVm()
            {
                ErrorMessage =
                    $"No results could have been acquired due to invalid request parameters! Check submitted URL or endpoint configuration, if exists, at {UrlHelperClass.GetEndpointUrl(Request.GetEncodedUrl())}."
            });
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
            [FromQuery] QueryStringParameters queryStringParametersDto)
        {
            Parameters parameters =
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

                return NotFound(new ErrorVm()
                    {ErrorMessage = $"No results were found! Generated sparql query: {query}"});
            }

            return NotFound(new ErrorVm()
            {
                ErrorMessage =
                    $"No results could have been acquired due to invalid request parameters! Check submitted URL or endpoint configuration, if exists, at {UrlHelperClass.GetEndpointUrl(Request.GetEncodedUrl())}."
            });
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
            [FromQuery] QueryStringParameters queryStringParametersDto)
        {
            Parameters parameters =
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


                return NotFound(new ErrorVm()
                    {ErrorMessage = $"No results were found! Generated sparql query: {query}"});
            }

            return NotFound(new ErrorVm()
            {
                ErrorMessage =
                    $"No results could have been acquired due to invalid request parameters! Check submitted URL or endpoint configuration, if exists, at {UrlHelperClass.GetEndpointUrl(Request.GetEncodedUrl())}."
            });
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
            Parameters parameters =
                _parametersProcessorService.ProcessParameters(Request.RouteValues, Request.QueryString);
            if (parameters.RouteParameters.Predicate == null)
                return Redirect(UrlHelperClass.ReduceUrl(Request.GetEncodedUrl(), "resource"));
            return Redirect(UrlHelperClass.ReduceUrl(Request.GetEncodedUrl(), "predicate"));
        }

        #endregion
    }
}