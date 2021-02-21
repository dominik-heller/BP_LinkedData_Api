using System.Collections.Generic;
using LinkedData_Api.DataModel.ParameterDto;
using VDS.RDF.Query;

namespace LinkedData_Api.Services
{
    public interface ISparqlFactoryService
    {
        string ConstructEntryClassQuery(string endpointName);
        IEnumerable<SparqlResult> ExecuteSelectSparqlQuery(string endpointName, string query);

    }
}