#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinkedData_Api.DataModel.EndpointConfigurationDto;
using LinkedData_Api.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace LinkedData_Api.Controllers
{
    [ApiController]
    public partial class MainController : ControllerBase
    {
        private readonly INamespaceFactoryService _namespaceFactoryService;
        private readonly IEndpointConfigurationService _endpointConfigurationService;
        private readonly IParametersProcessorService _parametersProcessorService;
        private readonly ISparqlFactoryService _sparqlFactoryService;
        private readonly IResultFormatterService _resultFormatterService;
        
        public MainController(INamespaceFactoryService namespaceFactoryService, IEndpointConfigurationService endpointConfigurationService, IParametersProcessorService parametersProcessorService, ISparqlFactoryService sparqlFactoryService, IResultFormatterService resultFormatterService)
        {
            _namespaceFactoryService = namespaceFactoryService;
            _endpointConfigurationService = endpointConfigurationService;
            _parametersProcessorService = parametersProcessorService;
            _sparqlFactoryService = sparqlFactoryService;
            _resultFormatterService = resultFormatterService;
        }

        //př: https://localhost:5001/api/virtuoso
        [HttpGet(ApiRoutes.EndpointInfo)]
        public IActionResult Get_EndpointSettings([FromRoute] string endpoint)
        {
            var info = _endpointConfigurationService.GetEndpointConfiguration(endpoint);
            if (info != null) return Ok(info);
            return NotFound("Endpoint does not exist.");
        }
        
        //př: https://localhost:5001/api/virtuoso/graphs
        [HttpGet(ApiRoutes.EndpointGraphs)]
        public IActionResult Get_GraphsForEndpoint([FromRoute] string endpoint)
        {
            var graphs = _endpointConfigurationService.GetEndpointGraphs(endpoint);
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