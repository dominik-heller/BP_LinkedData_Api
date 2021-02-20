using LinkedData_Api.DataModel.ParameterDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LinkedData_Api.Services
{
    public interface IParametersProcessorService
    {
        ParameterDto ProcessParameters(RouteValueDictionary requestRouteValues,
            QueryString requestQueryString);
    }
}