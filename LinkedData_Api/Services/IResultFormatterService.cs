using System.Collections.Generic;
using LinkedData_Api.Model.Contracts.ResponsesVM;
using VDS.RDF.Query;

namespace LinkedData_Api.Services
{
    public interface IResultFormatterService
    {
        IdsVm FormatSparqlResultToList(IEnumerable<SparqlResult> sparqlResult);
        void FormatSparqlResultToResourceDetail();
    }
}