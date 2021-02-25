#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using LinkedData_Api.Model.ParameterDto;
using LinkedData_Api.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using VDS.RDF.Query;

namespace LinkedData_Api.Controllers
{
    public partial class MainController
    {
        //př: https://localhost:5001/api/dbpedia/class/dbo:country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(ApiRoutes.DefaultGraphClassRoute)]
        public async Task<IActionResult> Get_DefaultGraph_ClassStart()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            if (pd.RouteParameters.ClassId == null) //pouze dotaz na api/endpoint/class => vrátí všechny třídy
            {
                string? query =
                    _sparqlFactoryService.GetDefaultEntryClassQuery(pd.RouteParameters.Endpoint);
                if (query != null)
                {
                    IEnumerable<SparqlResult>? sparqlResults = await
                        _sparqlFactoryService.ExecuteRemoteSelectSparqlQueryAsync(pd.RouteParameters.Endpoint, null, query);
                    if (sparqlResults != null)
                    {
                        CurieVm curiesVm = _resultFormatterService.FormatSparqlResultToList(sparqlResults);
                        if (curiesVm != null) return Ok(curiesVm);
                    }
                }
            }

            return NotFound("nenalezeno");
        }

        //př: https://localhost:5001/api/endpoint1/resource/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(ApiRoutes.DefaultGraphResourceRoute)]
        public ActionResult Get_DefaultGraph_ResourcesStart()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            return Ok(pd);
        }

        //př: https://localhost:5001/api/virtuoso/ovm/class/dbo:Country/dbr:Germany/dbo:Capital/dbr:Berlin
        [HttpGet(ApiRoutes.NamedGraphClassRoute)]
        public async Task<IActionResult> Get_GraphSpecific_ClassStart()
        {
            ParameterDto pd = _parametersProcessorService.ProcessParameters(Request.RouteValues,
                Request.QueryString);
            string? query = _sparqlFactoryService.GetGraphSpecificEntryClassQuery(pd.RouteParameters.Endpoint, pd.RouteParameters.Graph);
            if (query != null)
            {
                IEnumerable<SparqlResult>? sparqlResults = await 
                    _sparqlFactoryService.ExecuteRemoteSelectSparqlQueryAsync(pd.RouteParameters.Endpoint,pd.RouteParameters.Graph, query);
                if (sparqlResults != null)
                {
                    CurieVm curiesVm = _resultFormatterService.FormatSparqlResultToList(sparqlResults);
                    if (curiesVm != null) return Ok(curiesVm);
                }
            }

            return NotFound("nenalezeno");
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