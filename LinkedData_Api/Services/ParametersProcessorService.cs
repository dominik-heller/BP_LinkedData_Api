using System.Collections.Specialized;
using System.Web;
using LinkedData_Api.DataModel.ParameterDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LinkedData_Api.Services
{
    public class ParametersProcessorService : IParametersProcessorService
    {
        /*Není to I/O operace (neopouští pamět PC (př na sít či do db)) => může to být, ale není to stěžejní udělat asynchronně
        public static async Task<ParameterDto> ProcessParametersAsync(RouteValueDictionary requestRouteValues,
            QueryString requestQueryString)
        {
            ParameterDto parameterDto = new ParameterDto();
            await Task.Run(() =>
            {
                parameterDto = ProcessParameters(requestRouteValues, requestQueryString);
            });
            return parameterDto;
        }
        */

        public ParameterDto ProcessParameters(RouteValueDictionary requestRouteValues,
            QueryString requestQueryString)
        {
            ParameterDto parameterDto = new ParameterDto();
            RouteParametersDto routeParametersDto = new RouteParametersDto();
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
                    case "classId":
                    {
                        routeParametersDto.ClassId = requestRouteValues[key].ToString();
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
                        if (key.StartsWith("r")) routeParametersDto.Resource = requestRouteValues[key].ToString();
                        if (key.StartsWith("p")) routeParametersDto.Predicate = requestRouteValues[key].ToString();
                        break;
                    }
                }
            }

            parameterDto.RouteParameters = routeParametersDto;
            QueryStringParametersDto queryStringParametersDto = new QueryStringParametersDto();
            if (requestQueryString.HasValue)
            {
                NameValueCollection requestQueryStringValues = HttpUtility.ParseQueryString(requestQueryString.Value!);
                foreach (string key in requestQueryStringValues.Keys)
                {
                    switch (key)
                    {
                        case "limit":
                        {
                            if (int.TryParse(requestQueryStringValues[key], out var result))
                            {
                                queryStringParametersDto.Limit = result;
                            }

                            break;
                        }
                        case "page":
                        {
                            if (int.TryParse(requestQueryStringValues[key], out var result))
                            {
                                queryStringParametersDto.Page = result;
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

            parameterDto.QueryStringParametersDto = queryStringParametersDto;
            return parameterDto;
        }
    }
}