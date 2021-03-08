using System;
using System.Linq;

namespace LinkedData_Api.Helpers
{
    public static class UrlFactoryClass
    {
        public static string GetEndpointUrl(string url)
        {
            var x = url.Split("api/");
            var y = x[1].Split("/");
            return x[0] + "api/" + y[0];
        }
        
        public static string CreateEndpointUrl(string url, string endpointName)
        {
            var x = url.Split("api/");
            var y = x[1].Split("/");
            return x[0] + "api/" + endpointName;
        }
        
         public static string ReduceUrl(string url, string type)
        {
            var queryString = "";
            var finalUrl = "";
            if (type.Equals("resource"))
            {
                if (url.Contains("?")) queryString = url.Split("?").Last();
                if (url.Contains("/classes/"))
                {
                    var split = url.Split("/classes/");
                    var firstPart = split[0];
                    var secondPart = split[1];
                    var newResource = secondPart.Split("/").Last();
                    finalUrl = firstPart + "/resources/" + newResource + "/" + queryString;
                    return finalUrl;
                }
                else
                {
                    var split = url.Split("/resources/");
                    var firstPart = split[0];
                    var secondPart = split[1];
                    var newResource = secondPart.Split("/").Last();
                    finalUrl = firstPart + "/resources/" + newResource + "/" + queryString;
                    return finalUrl;
                }
            }

            if (type.Equals("predicate"))
            {
                if (url.Contains("?")) queryString = url.Split("?").Last();
                if (url.Contains("/classes/"))
                {
                    var split = url.Split("/classes/");
                    var firstPart = split[0];
                    var secondPart = split[1];
                    var newPredicate = secondPart.Split("/").Last();
                    var newResource = secondPart.Split("/")[^2];
                    finalUrl = firstPart + "/resources/" + newResource + "/" + newPredicate + "?" + queryString;
                    return finalUrl;
                }
                else
                {
                    var split = url.Split("/resources/");
                    var firstPart = split[0];
                    var secondPart = split[1];
                    var newPredicate = secondPart.Split("/").Last();
                    var newResource = secondPart.Split("/")[^2];
                    finalUrl = firstPart + "/resources/" + newResource + "/" + newPredicate + "?" + queryString;
                    return finalUrl;
                }
            }

            var y = url.IndexOf("resources", StringComparison.Ordinal) + 10;
            return finalUrl;
        }
    }
}