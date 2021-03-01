using System.Collections.Generic;
using LinkedData_Api.Model.ViewModels;
using VDS.RDF.Query;

namespace LinkedData_Api.Services.Contracts
{
    public interface IResultFormatterService
    {
        CurieVm FormatSparqlResultToCurieList(IEnumerable<SparqlResult> sparqlResults);
        ResourceVm FormatSparqlResultToResourceDetail(IEnumerable<SparqlResult> sparqlResults);
        PredicateVm FormatSparqlResultToCurieAndLiteralList(string routeParametersPredicate,IEnumerable<SparqlResult> sparqlResults);
    }
}