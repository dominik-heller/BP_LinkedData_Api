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

        public CurieVm FormatSparqlResultToCurieList(IEnumerable<SparqlResult> sparqlResults)
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
            ResourceVm resourceVm = new ResourceVm() {Predicates = new Dictionary<string, PredicateContent>()};

            foreach (var sparqlResult in sparqlResults)
            {
                string property = sparqlResult.Value("p").ToString();
                if (!_namespaceFactoryService.GetQnameFromAbsoluteUri(property, out var propertyQname)) continue;
                if (!resourceVm.Predicates.ContainsKey(propertyQname))
                    resourceVm.Predicates.Add(propertyQname, new PredicateContent());
                string obj = sparqlResult.Value("o").ToString();
                if (CheckIfValueIsLiteral(obj, propertyQname, out Literal literal))
                {
                    if (resourceVm.Predicates[propertyQname].Literals == null)
                        resourceVm.Predicates[propertyQname].Literals = new List<Literal>();
                    resourceVm.Predicates[propertyQname].Literals.Add(literal);
                }
                else
                {
                    if (_namespaceFactoryService.GetQnameFromAbsoluteUri(obj, out var objQname))
                    {
                        if (resourceVm.Predicates[propertyQname].Curies == null)
                            resourceVm.Predicates[propertyQname].Curies = new List<string>();
                        resourceVm.Predicates[propertyQname].Curies.Add(HttpUtility.UrlDecode(objQname));
                    }
                }
            }

            return resourceVm;
        }

        public PredicateVm FormatSparqlResultToCurieAndLiteralList(string predicate,
            IEnumerable<SparqlResult> sparqlResults)
        {
            PredicateVm predicateVm = new PredicateVm();
            foreach (var sparqlResult in sparqlResults)
            {
                string obj = sparqlResult.Value("o").ToString();
                if (CheckIfValueIsLiteral(obj, predicate, out Literal literal))
                {
                    if (predicateVm.Literals == null) predicateVm.Literals = new List<Literal>();
                    predicateVm.Literals.Add(literal);
                }
                else
                {
                    if (_namespaceFactoryService.GetQnameFromAbsoluteUri(obj, out var objQname))
                    {
                        if (predicateVm.Curies == null)
                            predicateVm.Curies = new List<string>();
                        predicateVm.Curies.Add(HttpUtility.UrlDecode(objQname));
                    }
                }
            }

            return predicateVm;
        }

        private bool CheckIfValueIsLiteral(string obj, string propertyQname, out Literal _literal)
        {
            if (propertyQname.Contains("label") || propertyQname.Contains("comment") ||
                propertyQname.Contains("homepage"))
            {
                if (LiteralContainsLangNotation(obj, out var lit))
                {
                    _literal = lit;
                }
                else
                {
                    _literal = new Literal() {Value = obj, Datatype = "xsd:string"};
                }

                return true;
            }

            if (GetLiteralFromUriIfContainsDatatypeDeclaration(obj, out var literal))
            {
                _literal = literal;
                return true;
            }

            if (CheckIfUriIsUrlLink(obj))
            {
                _literal = new Literal() {Value = obj, Datatype = "xsd:string"};
                return true;
            }

            if (!_namespaceFactoryService.GetQnameFromAbsoluteUri(obj, out var objQname))
            {
                if (LiteralContainsLangNotation(obj, out var lit))
                {
                    _literal = lit;
                }
                else
                {
                    _literal = new Literal() {Value = obj, Datatype = "xsd:string"};
                }

                return true;
            }

            _literal = null;
            return false;
        }

        private bool LiteralContainsLangNotation(string obj, out Literal literal)
        {
            literal = null;
            if (obj.Length > 4)
            {
                if (obj.LastIndexOf("@", StringComparison.Ordinal) > obj.Length - 4)
                {
                    literal = new Literal()
                    {
                        Value = obj.Remove(obj.LastIndexOf("@", StringComparison.Ordinal)),
                        Language = obj.Substring(obj.LastIndexOf("@", StringComparison.Ordinal) + 1),
                        Datatype = "rdf:langString"
                    };
                    return true;
                }
            }

            return false;
        }


        private bool CheckIfUriIsUrlLink(string uri)
        {
        //    Console.WriteLine(uri.LastIndexOf("/", StringComparison.Ordinal));
            if (uri.EndsWith("/")) return true;
            if (uri.EndsWith(".html")) return true;
            if (uri.Contains(".json")) return true;
            if (uri.Contains(".xml")) return true;
            if (uri.Contains(".jpg")) return true;
            if (uri.Contains(".rdf")) return true;
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