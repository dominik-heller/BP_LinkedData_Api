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
                catch (RdfException)
                {
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


        public bool GetQnameFromAbsoluteUri(string uri, out string qname)
        {
            if (_threadSafeQNameOutputMapper.ReduceToQName(uri, out var _qname))
            {
                qname = _qname;
                if (qname.EndsWith(":")) return false;
                return true;
            }

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
                qname = prefix + ":" + uri.Replace(nsUri, "");
                return true;
            }

            qname = string.Empty;
            return false;
        }


        public void AddNewPrefixes(IEnumerable<Namespace> namespaces)
        {
            foreach (var var in namespaces)
            {
                //if(_threadSafeQNameOutputMapper.HasNamespace(var.Prefix)){continue;} //ignores prefixes that are already defined, if commented out namespace with same prefix will be overwritten
                try
                {
                    _threadSafeQNameOutputMapper.AddNamespace(var.Prefix, new Uri(var.Uri));
                }
                catch (Exception)
                {
                    continue;
                }
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