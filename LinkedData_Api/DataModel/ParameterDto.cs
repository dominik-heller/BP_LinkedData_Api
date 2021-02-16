using System.Collections.Generic;

namespace LinkedData_Api.DataModel
{
    public class ParameterDto
    {
        public RouteParametersDto RouteParameters { get; set; }
        public QueryStringParametersDto QueryStringParametersDto { get; set; }
    }
}