using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Services.Contracts;
using VDS.Common.Collections.Enumerations;
using VDS.RDF;
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
                string key = sparqlResult.FirstOrDefault().Key;
                string value = sparqlResult.Value(key).ToString();
                if (!_namespaceFactoryService.GetQnameFromAbsoluteUri(value, out var qname)) continue;
                curieVm.Curies.Add(HttpUtility.UrlDecode(qname));
            }

            return curieVm;
        }

        public ResourceVm FormatSparqlResultToResourceDetail(IEnumerable<SparqlResult> sparqlResults)
        {
           
            ResourceVm resourceVm = new ResourceVm();
            
           foreach (var sparqlResult in sparqlResults)
           {
               string property = sparqlResult.Value("p").ToString();
               if (!_namespaceFactoryService.GetQnameFromAbsoluteUri(property, out var propertyQname)) continue;
               if (!resourceVm.Properties.ContainsKey(propertyQname))
                   resourceVm.Properties.Add(propertyQname, new PropertyContent());
               string obj = sparqlResult.Value("o").ToString();
               if (GetLiteralFromUriIfContainsDatatypeDeclaration(obj, out var literal))
               {
                   resourceVm.Properties[propertyQname].Literals.Add(literal);
                   continue;
               }

               if (CheckIfUriIsUrlLink(obj))
               {
                   resourceVm.Properties[propertyQname].Literals.Add((new Literal() {Value = obj}));
                   continue;
               }

               if (!_namespaceFactoryService.GetQnameFromAbsoluteUri(obj, out var objQname))
               {
                   resourceVm.Properties[propertyQname].Literals.Add((new Literal() {Value = obj}));
                   //   Console.WriteLine("Literal:" + obj);
                   continue;
               }

               resourceVm.Properties[propertyQname].Curies.Add(objQname);
               // Console.WriteLine("Curie:" + objQname);
           }
           
            return resourceVm;

        }

        private bool CheckIfUriIsUrlLink(string uri)
        {
            Console.WriteLine(uri.LastIndexOf("/", StringComparison.Ordinal));
            if (uri.EndsWith("/")) return true;
            if (uri.EndsWith(".html")) return true;
            if (uri.Contains(".jpg")) return true;
            if (uri.LastIndexOf("/", StringComparison.Ordinal) > 6 &&
                uri.LastIndexOf("/", StringComparison.Ordinal) < 8 && !uri.Contains("#")) return true;
            return false;
        }

        private bool GetLiteralFromUriIfContainsDatatypeDeclaration(string uri, out Literal literal)
        {
            literal = null;
            if (uri.Contains("^^"))
            {
                literal = new Literal();
                literal.Value = uri.Split("^")[0];
                if (_namespaceFactoryService.GetQnameFromAbsoluteUri(uri.Split("^")[2], out var qname))
                    literal.Datatype = qname;
                return true;
            }

            return false;
        }
    }
}