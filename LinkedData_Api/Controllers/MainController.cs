#nullable enable
using Microsoft.AspNetCore.Mvc;
using LinkedData_Api.Services.Contracts;

namespace LinkedData_Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public partial class MainController : ControllerBase
    {
        private readonly INamespaceFactoryService _namespaceFactoryService;
        private readonly IEndpointService _endpointService;
        private readonly IParametersProcessorService _parametersProcessorService;
        private readonly ISparqlFactoryService _sparqlFactoryService;
        private readonly IResultFormatterService _resultFormatterService;
        
        public MainController(INamespaceFactoryService namespaceFactoryService, IEndpointService endpointService, IParametersProcessorService parametersProcessorService, ISparqlFactoryService sparqlFactoryService, IResultFormatterService resultFormatterService)
        {
            _namespaceFactoryService = namespaceFactoryService;
            _endpointService = endpointService;
            _parametersProcessorService = parametersProcessorService;
            _sparqlFactoryService = sparqlFactoryService;
            _resultFormatterService = resultFormatterService;
        }

        //př: https://localhost:5001/api/virtuoso
        /// <summary>
        /// Returns endpoint configuration information.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.EndpointInfo)]
        public IActionResult Get_EndpointSettings([FromRoute] string endpoint)
        {
            var info = _endpointService.GetEndpointConfiguration(endpoint);
            if (info != null) return Ok(info);
            return NotFound("Endpoint does not exist.");
        }
        
        //př: https://localhost:5001/api/virtuoso/graphs
        /// <summary>
        /// Returns endpoint's named graphs.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.EndpointGraphs)]
        public IActionResult Get_GraphsForEndpoint([FromRoute] string endpoint)
        {
            var graphs = _endpointService.GetEndpointGraphs(endpoint);
            if (graphs != null) return Ok(graphs);
            return NotFound("Endpoint does not exist.");
        }
     /*   
        //NAMESPACES_enpoint
        //př: https://localhost:5001/api/endpoint1/namespaces
        [HttpGet("api/namespaces")]
        public string Get_Namespaces()
        {
            return $"NAMESPACES POSTED";
        }

        //NAMESPACES_enpoint_POST
        //př: https://localhost:5001/api/endpoint1/namespaces
        [HttpPost("api/namespaces")]
        public string Post_Namespaces()
        {
            return $"NAMESPACES POSTED";
        }
       */ 
    }
}