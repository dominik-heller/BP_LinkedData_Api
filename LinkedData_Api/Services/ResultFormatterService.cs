using System;
using System.Collections.Generic;
using System.Linq;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Services.Contracts;
using VDS.RDF.Query;

namespace LinkedData_Api.Services
{
    public class ResultFormatterService : IResultFormatterService
    {
        private readonly INamespaceFactoryService _namespaceFactoryService;

        public ResultFormatterService(INamespaceFactoryService namespaceFactoryService)
        {
            _namespaceFactoryService = namespaceFactoryService;
        }

        public CurieVm FormatSparqlResultToList(IEnumerable<SparqlResult> sparqlResults)
        {
            CurieVm curieVm = new CurieVm();
            foreach (var sparqlResult in sparqlResults)
            {
                string variable = sparqlResult.Variables.First();
                curieVm.Curies.Add(sparqlResult.Value(variable).ToString());
                //TODO: dále zformátovat výstup do Curie => TransformResultToQnameRepresentation()
            }

            curieVm = TransformResultToQnameRepresentation(curieVm);
            return curieVm;
        }

        private CurieVm TransformResultToQnameRepresentation(CurieVm _curieVm)
        {
            CurieVm curieVm = new CurieVm();
            foreach (var c in _curieVm.Curies)
            {
                _namespaceFactoryService.GetQnameFromAbsoluteUri(c, out var qname);
                /*př u lexvo přijde na nejvyšší úrovni lvont:Language místo http://lexvo.org/ontology#Language ale neni to chápaný jako qname ale jako skutečný absolute uri => když ho vložím do <> vím že je to uri a nebudu ho nijak modifikovat
                if (String.IsNullOrEmpty(qname)){
                    curieVm.Curies.Add($"<{c}>");
                    continue;
                }
                */
                /*př. u lexvo to někdy nevrátí celé uri ale rovnou qname... ceknu jestli mame pro dany qname definici, pokud ano vratim... hned
                if (String.IsNullOrEmpty(qname) && _namespaceFactoryService.GetAbsoluteUriFromQname(c, out var absoluteUri))
                {
                    curieVm.Curies.Add(c);
                    continue;
                }
                */
                curieVm.Curies.Add(qname);
            }

            return curieVm;
        }

        public void FormatSparqlResultToResourceDetail()
        {
        }
    }
}