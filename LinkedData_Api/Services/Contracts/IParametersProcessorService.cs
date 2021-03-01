using LinkedData_Api.Model.ParameterDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LinkedData_Api.Services.Contracts
{
    public interface IParametersProcessorService
    {
        ParameterDto ProcessParameters(RouteValueDictionary requestRouteValues,
            QueryString requestQueryString);

        string ReduceUrl(string url, string type);
    }
}