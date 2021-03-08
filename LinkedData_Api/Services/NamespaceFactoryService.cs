using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AngleSharp.Common;
using LinkedData_Api.Data;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Services.Contracts;
using Newtonsoft.Json.Linq;
using VDS.RDF;

namespace LinkedData_Api.Services
{
    public class NamespaceFactoryService : INamespaceFactoryService
    {
        private readonly ThreadSafeQNameOutputMapper _threadSafeQNameOutputMapper;

        public NamespaceFactoryService(IDataAccess dataAccess)
        {
            _threadSafeQNameOutputMapper = new ThreadSafeQNameOutputMapper(dataAccess.GetNamespaces());
        }

        public bool GetAbsoluteUriFromQname(string qname, out string absoluteUri)
        {
            if (qname.Contains(":"))
            {
                var s = qname.Split(new[] {':'}, 2);
                try
                {
                    absoluteUri = _threadSafeQNameOutputMapper.GetNamespaceUri(s[0]) + s[1];
                    return true;
                }
                catch (RdfException e)
                {
                    Console.WriteLine("Absolute Uri could not have been created. " + e);
                    absoluteUri = String.Empty;
                    return false;
                }
            }

            absoluteUri = null;
            return false;
        }

        public bool GetNamespaceUriByPrefix(string prefix, out string namespaceUri)
        {
            try
            {
               namespaceUri = _threadSafeQNameOutputMapper.GetNamespaceUri(prefix).ToString();
               return true;
            }
            catch (RdfException)
            {
                namespaceUri = null;
                return false;
            }
        }

        //přijde dbpedia.org/resource#Germany a nutno to změnit na př: dbr:Germany
        public bool GetQnameFromAbsoluteUri(string uri, out string qname)
        {
            //pokud existuje definovaný prefix/namespace pro dané uri = > vrátí qname
            if (_threadSafeQNameOutputMapper.ReduceToQName(uri, out var _qname))
            {
                qname = _qname;
                return true;
            }

            //pokud ne vytvoří nový prefix/namespace ve tvaru ns[cislo] a vrati qname (pokud je příchozí ve tvaru http... jinak vrací string.empty)
            //  Console.WriteLine("Namespace undefined.");
            if (GetNamespaceUriFromAbsoluteUri(uri, out var nsUri))
            {
                string prefix;
                if (!_threadSafeQNameOutputMapper.Prefixes.Any(x => x.Equals("_ns0")))
                {
                    prefix = "_ns0";
                }
                else
                {
                    prefix =
                        $"_ns{_threadSafeQNameOutputMapper.Prefixes.Where(x => x.StartsWith("_ns")).Select(x => int.Parse(x.Substring(3))).Max() + 1}";
                }

                _threadSafeQNameOutputMapper.AddNamespace(prefix, new Uri(nsUri));
                //      Console.WriteLine("Namespace added.");
                if (_threadSafeQNameOutputMapper.ReduceToQName(uri, out _qname))
                {
                    qname = _qname;
                    return true;
                }
            }

            qname = string.Empty;
            return false;
        }


        public void AddNewPrefixes(IEnumerable<Namespace> namespaces)
        {
            foreach (var var in namespaces)
            {
                _threadSafeQNameOutputMapper.AddNamespace(var.Prefix, new Uri(var.Uri));
            }
        }

        private static bool GetNamespaceUriFromAbsoluteUri(string uri, out string nsUri)
        {
            nsUri = string.Empty;
            if (!uri.StartsWith("http")) return false;
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

            return false;
        }
    }
}