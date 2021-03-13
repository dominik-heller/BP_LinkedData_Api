using System.Collections.Generic;
using LinkedData_Api.Model.ViewModels;
using VDS.RDF.Query;

namespace LinkedData_Api.Services.Contracts
{
    public interface IResultFormatterService
    {
        /// <summary>
        /// Formats sparqlResults to <see cref="CurieVm"/> instance
        /// </summary>
        /// <param name="sparqlResults"></param>
        /// <returns></returns>
        CurieVm FormatSparqlResultToCurieList(IEnumerable<SparqlResult> sparqlResults);

        /// <summary>
        /// Formats sparqlResults to <see cref="ResourceVm"/> instance
        /// </summary>
        /// <param name="sparqlResults"></param>
        /// <returns></returns>
        ResourceVm FormatSparqlResultToResourceDetail(IEnumerable<SparqlResult> sparqlResults);

        /// <summary>
        /// Formats sparqlResults to <see cref="PredicateVm"/> instance
        /// </summary>
        /// <param name="routeParametersPredicate"></param>
        /// <param name="sparqlResults"></param>
        /// <returns></returns>
        PredicateVm FormatSparqlResultToCurieAndLiteralList(string routeParametersPredicate,
            IEnumerable<SparqlResult> sparqlResults);
    }
}