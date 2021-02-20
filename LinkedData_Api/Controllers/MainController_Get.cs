#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using LinkedData_Api.DataModel;
using LinkedData_Api.DataModel.ParameterDto;
using LinkedData_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkedData_Api.Controllers
{
    public partial class MainController : ControllerBase
    {
        //př: https://localhost:5001/api/dbpedia/class/dbo:country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiClassRoute)]
        public ActionResult Get_DefaultGraph_ClassStart()
        {
           // MyTests.Test();
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            var c = _endpointConfigurationService.GetEndpointUrl(pd.RouteParameters.Endpoint);
            if (c != null)
            {
                return Ok(c);
            }

            return NotFound("Nenalezeno");
        }

        //př: https://localhost:5001/api/endpoint1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiResourceRoute)]
        public ActionResult Get_DefaultGraph_ResourcesStart()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return Ok(pd);
        }

        //př: https://localhost:5001/api/endpoint1/graph1/class/dbo:Country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiGraphClassRoute)]
        public ActionResult Get_GraphSpecific_ClassStart()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return Ok(pd);
        }


        //př: https://localhost:5001/api/endpoint1/graph1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(BaseApiGraphResourceRoute)]
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