#nullable enable
using Microsoft.AspNetCore.Mvc;

namespace LinkedData_Api.Controllers
{
    public partial class MainController
    {
        //CLASS_defaultgraph
        //př: https://localhost:5001/api/endpoint1/class/dbo:country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpPut(ApiRoutes.DefaultGraphClassRoute)]
        public string Put_DefaultGraph_ClassStart([FromRoute] string endpoint, [FromRoute] string? class_id = null,
            [FromRoute] string? subject = null,
            [FromRoute] string? predicate = null, [FromRoute] string? @object = null)
        {
            return
                $"CLASS_DefaultGraph\nEndpoint: {endpoint}\t Graph: Default\t \tClass_Id:{class_id}\nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }

        //RESOURCE_defaultgraph
        //př: https://localhost:5001/api/endpoint1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpPut(ApiRoutes.DefaultGraphResourceRoute)]
        public string Put_DefaultGraph_ResourcesStart([FromRoute] string endpoint, [FromRoute] string? subject = null,
            [FromRoute] string? predicate = null, [FromRoute] string? @object = null)
        {
            return
                $"CLASS_DefaultGraph\nEndpoint: {endpoint}\t Graph: Default\t \nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }

        //CLASS_namedgraph
        //př: https://localhost:5001/api/endpoint1/graph1/class/dbo:Country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpPut(ApiRoutes.NamedGraphClassRoute)]
        public string Put_GraphSpecific_ClassStart([FromRoute] string endpoint, [FromRoute] string graph,
            [FromRoute] string? classId = null,
            [FromRoute] string? subject = null, [FromRoute] string? predicate = null,
            [FromRoute] string? @object = null)
        {
            return
                $"CLASS_NamedGraph\nEndpoint: {endpoint}\t Graph: {graph} \tClass_Id:{classId} \nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }


        //RESOURCE_namedgraph
        //př: https://localhost:5001/api/endpoint1/graph1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpPut(ApiRoutes.NamedGraphResourceRoute)]
        public string Put_GraphSpecific_ResourceStart([FromRoute] string endpoint, [FromRoute] string graph,
            [FromRoute] string? subject = null, [FromRoute] string? predicate = null,
            [FromRoute] string? @object = null)
        {
            return
                $"RESOURCE_NamedGraph\nEndpoint: {endpoint}\t Graph: {graph}\t \nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }
    }
}