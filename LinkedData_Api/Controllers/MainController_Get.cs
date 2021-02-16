#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using LinkedData_Api.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace LinkedData_Api.Controllers
{
    public partial class MainController : ControllerBase
    {
        //CLASS_defaultgraph
        //př: https://localhost:5001/api/endpoint1/class/dbo:country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiClassRoute)]
        public string Get_DefaultGraph_ClassStart()
        {
            ParameterDto pd = Services.ParametersProcessor.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return "tady";
            // $"CLASS_DefaultGraph\nEndpoint: {endpoint}\t Graph: Default\t \tClass_Id:{class_id}\nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }

        //RESOURCE_defaultgraph
        //př: https://localhost:5001/api/endpoint1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiResourceRoute)]
        public string Get_DefaultGraph_ResourcesStart([FromRoute] string endpoint, [FromRoute] string? subject = null,
            [FromRoute] string? predicate = null, [FromRoute] string? @object = null)
        {
            return
                $"CLASS_DefaultGraph\nEndpoint: {endpoint}\t Graph: Default\t \nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }

        //CLASS_namedgraph
        //př: https://localhost:5001/api/endpoint1/graph1/class/dbo:Country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiGraphClassRoute)]
        public string Get_GraphSpecific_ClassStart([FromRoute] string endpoint, [FromRoute] string graph,
            [FromRoute] string? classId = null,
            [FromRoute] string? subject = null, [FromRoute] string? predicate = null,
            [FromRoute] string? @object = null)
        {
            return
                $"CLASS_NamedGraph\nEndpoint: {endpoint}\t Graph: {graph} \tClass_Id:{classId} \nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }


        //RESOURCE_namedgraph
        //př: https://localhost:5001/api/endpoint1/graph1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiGraphResourceRoute)]
        public string Get_GraphSpecific_ResourceStart([FromRoute] string endpoint, [FromRoute] string graph,
            [FromRoute] string? subject = null, [FromRoute] string? predicate = null,
            [FromRoute] string? @object = null)
        {
            return
                $"RESOURCE_NamedGraph\nEndpoint: {endpoint}\t Graph: {graph}\t \nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }
    }
}