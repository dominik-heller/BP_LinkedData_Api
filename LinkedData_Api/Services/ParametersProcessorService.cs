using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using LinkedData_Api.Model.Domain;
using LinkedData_Api.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LinkedData_Api.Services
{
    public class ParametersProcessorService : IParametersProcessorService
    {
        public Parameters ProcessParameters(RouteValueDictionary requestRouteValues,
            QueryString requestQueryString)
        {
            Parameters parameterDto = new Parameters();
            RouteParameters routeParametersDto = new RouteParameters();
            foreach (var key in requestRouteValues.Keys)
            {
                switch (key)
                {
                    case "endpoint":
                    {
                        routeParametersDto.Endpoint = requestRouteValues[key].ToString();
                        break;
                    }
                    case "graph":
                    {
                        routeParametersDto.Graph = requestRouteValues[key].ToString();
                        break;
                    }
                    case "class":
                    {
                        routeParametersDto.Class = requestRouteValues[key].ToString();
                        break;
                    }
                    case "resource":
                    {
                        routeParametersDto.Resource = requestRouteValues[key].ToString();
                        break;
                    }
                    case "predicate":
                    {
                        routeParametersDto.Predicate = requestRouteValues[key].ToString();
                        break;
                    }
                    default:
                    {
                        if (key.StartsWith("r"))
                        {
                            routeParametersDto.Resource = requestRouteValues[key].ToString();
                            routeParametersDto.Predicate = null;
                        }

                        if (key.StartsWith("p")) routeParametersDto.Predicate = requestRouteValues[key].ToString();
                        break;
                    }
                }
            }

            parameterDto.RouteParameters = routeParametersDto;
            QueryStringParameters queryStringParametersDto = new QueryStringParameters();
            if (requestQueryString.HasValue)
            {
                NameValueCollection requestQueryStringValues = HttpUtility.ParseQueryString(requestQueryString.Value!);
                if (requestQueryStringValues.HasKeys())
                {
                    foreach (string key in requestQueryStringValues.Keys)
                    {
                        switch (key.ToLower())
                        {
                            case "limit":
                            {
                                if (int.TryParse(requestQueryStringValues[key], out var result))
                                {
                                    queryStringParametersDto.Limit = result;
                                }

                                break;
                            }
                            case "offset":
                            {
                                if (int.TryParse(requestQueryStringValues[key], out var result))
                                {
                                    queryStringParametersDto.Offset = result;
                                }

                                break;
                            }
                            case "regex":
                            {
                                queryStringParametersDto.Regex = requestQueryStringValues[key];
                                break;
                            }
                            case "sort":
                            {
                                queryStringParametersDto.Sort = requestQueryStringValues[key];
                                break;
                            }
                            //Případně další parametry
                        }
                    }
                }
            }

            parameterDto.QueryStringParametersDto = queryStringParametersDto;
            return parameterDto;
        }

       
    }
}