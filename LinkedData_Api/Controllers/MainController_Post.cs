#nullable enable
using Microsoft.AspNetCore.Mvc;

namespace LinkedData_Api.Controllers
{
    public partial class MainController : ControllerBase
    {
        //CLASS_defaultgraph
        //př: https://localhost:5001/api/endpoint1/class/dbo:country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpPost("api/{endpoint}/class/{class_id?}/{subject?}/{predicate?}/{object?}")]
        public string Post_DefaultGraph_ClassStart([FromRoute] string endpoint, [FromRoute] string? class_id = null,
            [FromRoute] string? subject = null,
            [FromRoute] string? predicate = null, [FromRoute] string? @object = null)
        {
            return
                $"CLASS_DefaultGraph\nEndpoint: {endpoint}\t Graph: Default\t \tClass_Id:{class_id}\nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }

        //RESOURCE_defaultgraph
        //př: https://localhost:5001/api/endpoint1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpPost("api/{endpoint}/resource/{object?}/{predicate?}/{subject?}")]
        public string Post_DefaultGraph_ResourcesStart([FromRoute] string endpoint, [FromRoute] string? subject = null,
            [FromRoute] string? predicate = null, [FromRoute] string? @object = null)
        {
            return
                $"CLASS_DefaultGraph\nEndpoint: {endpoint}\t Graph: Default\t \nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }

        //CLASS_namedgraph
        //př: https://localhost:5001/api/endpoint1/graph1/class/dbo:Country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpPost("api/{endpoint}/{graph}/class/{class_id?}/{object?}/{predicate?}/{subject?}")]
        public string Post_GraphSpecific_ClassStart([FromRoute] string endpoint, [FromRoute] string graph,
            [FromRoute] string? class_id = null,
            [FromRoute] string? subject = null, [FromRoute] string? predicate = null,
            [FromRoute] string? @object = null)
        {
            return
                $"CLASS_NamedGraph\nEndpoint: {endpoint}\t Graph: {graph} \tClass_Id:{class_id} \nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }


        //RESOURCE_namedgraph
        //př: https://localhost:5001/api/endpoint1/graph1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpPost("api/{endpoint}/{graph}/resource/{object?}/{predicate?}/{subject?}")]
        public string Post_GraphSpecific_ResourceStart([FromRoute] string endpoint, [FromRoute] string graph,
            [FromRoute] string? subject = null, [FromRoute] string? predicate = null,
            [FromRoute] string? @object = null)
        {
            return
                $"RESOURCE_NamedGraph\nEndpoint: {endpoint}\t Graph: {graph}\t \nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }
    }
}