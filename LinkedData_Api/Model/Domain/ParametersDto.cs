using LinkedData_Api.Model.Domain;

namespace LinkedData_Api.Model.ParameterDto
{
    public class Parameters
    {
        public RouteParameters RouteParameters { get; set; }
        public QueryStringParameters QueryStringParametersDto { get; set; }
    }
}