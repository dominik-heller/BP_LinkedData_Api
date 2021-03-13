#nullable enable
using LinkedData_Api.Model.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LinkedData_Api.Services.Contracts
{
    public interface IParametersProcessorService
    {
        /// <summary>
        /// Processes parameters from route and querystring and returns <see cref="Parameters"/> instance 
        /// </summary>
        /// <param name="requestRouteValues"></param>
        /// <param name="requestQueryString"></param>
        /// <returns></returns>
        Parameters ProcessParameters(RouteValueDictionary requestRouteValues,
            QueryString requestQueryString);
    }
}