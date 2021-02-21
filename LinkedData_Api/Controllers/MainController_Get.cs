#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using LinkedData_Api.DataModel;
using LinkedData_Api.DataModel.ParameterDto;
using LinkedData_Api.Model.Contracts.ResponsesVM;
using LinkedData_Api.Services;
using Microsoft.AspNetCore.Mvc;
using VDS.RDF.Query;

namespace LinkedData_Api.Controllers
{
    public partial class MainController : ControllerBase
    {
        //př: https://localhost:5001/api/dbpedia/class/dbo:country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(ApiRoutes.DefaultGraphClassRoute)]
        public ActionResult Get_DefaultGraph_ClassStart()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            var x = pd.RouteParameters.ClassId;
            if (pd.RouteParameters.ClassId == null)
            {
                string defaultClassQuery =
                    _sparqlFactoryService.ConstructEntryClassQuery(pd.RouteParameters.Endpoint);
                if (defaultClassQuery != null)
                {
                    IEnumerable<SparqlResult> sparqlResults = _sparqlFactoryService.ExecuteSelectSparqlQuery(pd.RouteParameters.Endpoint, defaultClassQuery);
                    if (sparqlResults != null)
                    {
                        IdsVm idsVm = _resultFormatterService.FormatSparqlResultToList(sparqlResults);
                        if (idsVm != null) return Ok(idsVm);
                    }
                }
            }

            IdsVm idsVm2 = new(){Ids = new List<string>(){"pea", "adasf"}};
            return NotFound(idsVm2);
        }

        //př: https://localhost:5001/api/endpoint1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(ApiRoutes.DefaultGraphResourceRoute)]
        public ActionResult Get_DefaultGraph_ResourcesStart()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return Ok(pd);
        }

        //př: https://localhost:5001/api/endpoint1/graph1/class/dbo:Country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(ApiRoutes.NamedGraphClassRoute)]
        public ActionResult Get_GraphSpecific_ClassStart()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return Ok(pd);
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