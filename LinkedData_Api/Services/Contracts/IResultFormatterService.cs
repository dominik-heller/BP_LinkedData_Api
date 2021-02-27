using System.Collections.Generic;
using LinkedData_Api.Model.ViewModels;
using VDS.RDF.Query;

namespace LinkedData_Api.Services.Contracts
{
    public interface IResultFormatterService
    {
        CurieVm FormatSparqlResultToList(IEnumerable<SparqlResult> sparqlResults);
        ResourceVm FormatSparqlResultToResourceDetail(IEnumerable<SparqlResult> sparqlResults);
    }
}