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
using Microsoft.AspNetCore.Mvc.Routing;
using Swashbuckle.AspNetCore.Annotations;

namespace LinkedData_Api.Controllers
{
    [ApiController]
    public partial class MainController : ControllerBase
    {
        //RouteTemplates
        private const string BaseApiClassRoute =
            "api/{endpoint}/class/{classId?}/{subject?}/{predicate?}/{object?}";

        private const string BaseApiResourceRoute =
            "api/{endpoint}/resource/{subject?}/{predicate?}/{object?}";

        private const string BaseApiGraphClassRoute =
            "api/{endpoint}/{graph}/class/{classId?}/{subject?}/{predicate?}/{object?}";
        
        private const string BaseApiGraphResourceRoute =
            "api/{endpoint}/{graph}/resource/{subject?}/{predicate?}/{object?}";


        //ENDPOINT_settings
        //př: https://localhost:5001/api/endpoint1
        [HttpGet("api/{endpoint}")]
        public string Get_EndpointSettings([FromRoute] string endpoint)
        {
            return $"SETTINGS FOR ENDPOINT {endpoint}";
        }

        //ENDPOINT_settings_POST
        //př: https://localhost:5001/api/endpoint1
        [HttpPost("api/{endpoint}")]
        public string Post_EndpointSettings([FromRoute] string endpoint)
        {
            return $"POSTED SETTINGS FOR ENDPOINT {endpoint}";
        }

        //ENDPOINT_settings_DELETE
        //př: https://localhost:5001/api/endpoint1
        [HttpDelete("api/{endpoint}")]
        public string Delete_EndpointSettings([FromRoute] string endpoint)
        {
            return $"DELETED SETTINGS FOR ENDPOINT {endpoint}";
        }

        //NAMESPACES_enpoint
        //př: https://localhost:5001/api/endpoint1/namespaces
        [HttpGet("api/{endpoint}/namespaces")]
        public string Get_NamespacesForEndpoint([FromRoute] string endpoint)
        {
            Console.WriteLine(Request.RouteValues);
            Console.WriteLine(Request.QueryString);
            return $"NAMESPACES FOR ENDPOINT {endpoint}";
        }

        //NAMESPACES_enpoint_POST
        //př: https://localhost:5001/api/endpoint1/namespaces
        [HttpPost("api/{endpoint}/namespaces")]
        public string Post_NamespacesForEndpoint([FromRoute] string endpoint)
        {
            return $"POSTED NAMESPACES FOR ENDPOINT {endpoint}";
        }

        //NAMESPACES_enpoint_DELETE
        //př: https://localhost:5001/api/endpoint1/namespaces
        [HttpDelete("api/{endpoint}/namespaces")]
        public string Delete_NamespacesForEndpoint([FromRoute] string endpoint)
        {
            return $"DELETED NAMESPACES FOR ENDPOINT {endpoint}";
        }
    }
}