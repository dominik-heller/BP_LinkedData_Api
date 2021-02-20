using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using VDS.RDF;

namespace LinkedData_Api.Services
{
    public class NamespaceFactoryService : INamespaceFactoryService
    {
        private readonly NamespaceMapper _namespaceMapper;

        public NamespaceFactoryService()
        {
            _namespaceMapper = new NamespaceMapper();
            LoadNamespacesFromFile();
        }

        //načte asi 2500 výchozích prefixů z filu
        public void LoadNamespacesFromFile()
        {
            JObject o = JObject.Parse(File.ReadAllText(@"JsonFiles/Namespaces/namespaces.json"));
            foreach (var v in o)
            {
                // Debug.WriteLine(v.Key+":"+v.Value);
                if (v.Value != null) _namespaceMapper.AddNamespace(v.Key, new Uri(v.Value.ToString()));
            }

            Debug.WriteLine("tady");
        }

        
        public bool GetAbsoluteUriFromQname(string qname, out string absoluteUri)
        {
            var s = qname.Split(":");
            try
            {
                absoluteUri = _namespaceMapper.GetNamespaceUri(s[0])+s[1];
                return true;
            }
            catch (RdfException)
            {
                Console.WriteLine("Absolute Uri could not have been created.");
                absoluteUri=String.Empty;
                return false;
            }
        }
        
        //přijde dbpedia.org/resource#Germany a nutno to změnit na př: dbr:Germany
        public bool GetQnameFromAbsoluteUri(string uri, out string qname)
        {
            //pokud existuje definovaný prefix/namespace pro dané uri = > vrátí qname
            if (_namespaceMapper.ReduceToQName(uri, out var _qname))
            {
                qname = _qname;
                return true;
            }

            //pokud ne vytvoří nový prefix/namespace ve tvaru ns[cislo] a vrati qname
            Console.WriteLine("Namespace undefined.");
            if (GetNamespaceUriFromAbsoluteUri(uri, out var nsUri))
            {
                string prefix;
                if (!_namespaceMapper.Prefixes.Any(x => x.Equals("_ns0")))
                {
                    prefix = "_ns0";
                }
                else
                {
                    prefix = $"_ns{_namespaceMapper.Prefixes.Where(x => x.StartsWith("_ns")).Select(x => int.Parse(x.Substring(3))).Max() + 1}";
                }

                _namespaceMapper.AddNamespace(prefix, new Uri(nsUri));
                Console.WriteLine("Namespace added.");
                if (_namespaceMapper.ReduceToQName(uri, out _qname))
                {
                    qname = _qname;
                    return true;
                }
            }
            qname = string.Empty;
            return false;
        }

        private static bool GetNamespaceUriFromAbsoluteUri(string uri, out string nsUri)
        {
            if (uri.Contains('#'))
            {
                nsUri = uri.Substring(0, uri.LastIndexOf('#') + 1);
                return true;
            }

            if (uri.LastIndexOf('/') > 8)
            {
                nsUri = uri.Substring(0, uri.LastIndexOf('/') + 1);
                return true;
            }

            nsUri = string.Empty;
            return false;
        }
    }
}