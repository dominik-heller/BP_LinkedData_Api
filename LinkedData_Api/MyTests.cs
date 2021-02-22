using System;
using System.Linq;
using LinkedData_Api.Data;
using LinkedData_Api.Services;

namespace LinkedData_Api
{
    public class MyTests
    {

        public static void Test()
        {
            EndpointConfigurationFilesProcessorTest();
            NamespaceFactoryTest();
        }

        private static void EndpointConfigurationFilesProcessorTest()
        {
            var processor = new EndpointService(new DataAccess());
            string url1 = processor.GetEndpointUrl("dbpedia");
            string url2 = processor.GetEndpointUrl("nic");
        }

        private static void NamespaceFactoryTest()
        {
            var nf = new NamespaceFactoryService(new DataAccess());
            //pokud vrátí false => nepovedlo a uživatel zadal špatný parametry do url cesty ;)
            var b1 = nf.GetQnameFromAbsoluteUri("http://dbpedia.org/resource/A._J._Reynolds", out var w);
            var b2 = nf.GetQnameFromAbsoluteUri("http://dbpedia.org/resource1/A._J._Reynolds", out var x);
            var b3 = nf.GetQnameFromAbsoluteUri("http://dbpedia.org/resource1/A._J._Reynolds", out var y);
            var b4 = nf.GetQnameFromAbsoluteUri("http://dbpedia.org/resource2/A._J._Reynolds", out var z);
            var b5 = nf.GetAbsoluteUriFromQname("_ns0:A._J._Reynolds", out var k);
            var b6 = nf.GetAbsoluteUriFromQname("wrong_prefix:A._J._Reynolds", out var p);
        }
    }
}