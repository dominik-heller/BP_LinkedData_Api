using System.Collections.Generic;
using System.Linq;
using LinkedData_Api.Model.Contracts.ResponsesVM;
using VDS.RDF.Query;

namespace LinkedData_Api.Services
{
    public class ResultFormatterService : IResultFormatterService
    {
        public IdsVm FormatSparqlResultToList(IEnumerable<SparqlResult> sparqlResults)
        {
            IdsVm idsVm = new IdsVm();
            foreach (var sparqlResult in sparqlResults)
            {
                string variable = sparqlResult.Variables.First();
                idsVm.Ids.Add(sparqlResult.Value(variable).ToString());
                //TODO: dále zformátovat výstup do Curie => TransformResultToQnameRepresentation()
            }

            return idsVm;
        }

        public void TransformResultToQnameRepresentation()
        {
            
        }
        
        public void FormatSparqlResultToResourceDetail()
        {
        }
    }
}