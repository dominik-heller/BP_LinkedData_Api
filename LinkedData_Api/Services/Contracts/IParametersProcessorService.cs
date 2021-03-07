using LinkedData_Api.Model.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LinkedData_Api.Services.Contracts
{
    public interface IParametersProcessorService
    {
        Parameters ProcessParameters(RouteValueDictionary requestRouteValues,
            QueryString requestQueryString);

        string ReduceUrl(string url, string type);
    }
}