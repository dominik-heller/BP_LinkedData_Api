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
        public ParameterDto Get_DefaultGraph_ClassStart()
        {
            ParameterDto pd = Services.ParametersProcessor.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return pd;
            // $"CLASS_DefaultGraph\nEndpoint: {endpoint}\t Graph: Default\t \tClass_Id:{class_id}\nPředané parametry: {subject} -> {predicate} -> {@object}.\nZde tedy bude logika pro zpracování spraql dotazu a následné zobrazení. :)";
        }

        //RESOURCE_defaultgraph
        //př: https://localhost:5001/api/endpoint1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiResourceRoute)]
        public ParameterDto Get_DefaultGraph_ResourcesStart()
        {
            ParameterDto pd = Services.ParametersProcessor.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return pd;
        }

        //CLASS_namedgraph
        //př: https://localhost:5001/api/endpoint1/graph1/class/dbo:Country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiGraphClassRoute)]
        public ParameterDto Get_GraphSpecific_ClassStart() {
            ParameterDto pd = Services.ParametersProcessor.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return pd;
        }


        //RESOURCE_namedgraph
        //př: https://localhost:5001/api/endpoint1/graph1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiGraphResourceRoute)]
        public ParameterDto Get_GraphSpecific_ResourceStart()
        {
            ParameterDto pd = Services.ParametersProcessor.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return pd;
        }
    }
}