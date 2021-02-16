using System;
using System.Web;
using LinkedData_Api.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LinkedData_Api.Services
{
    public class ParametersProcessor
    {
        public static ParameterDto ProcessParameters(RouteValueDictionary requestRouteValues, QueryString requestQueryString)
        {
            if(requestQueryString.HasValue)
            {
                var nameValueCollection = HttpUtility.ParseQueryString(requestQueryString.Value!);
                foreach (string key in nameValueCollection.Keys)
                {
                    Console.WriteLine(key);
                    Console.WriteLine(nameValueCollection[key]);
                    Console.WriteLine();
                }
            }
            
            foreach (var key in requestRouteValues.Keys)
            {
                Console.WriteLine(key);
                Console.WriteLine(requestRouteValues[key]);
                Console.WriteLine();
            } 
            // return new ParameterDto( new RouteParametersDto(){}, new QueryStringParametersDto());
           return  new ParameterDto();
        }
    }
}