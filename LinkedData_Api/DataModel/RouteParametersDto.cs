using System.Collections.Generic;

namespace LinkedData_Api.DataModel
{
    public class RouteParametersDto
    {
        public string Endpoint { get; set; }
        public string Graph { get; set; }
        public IEnumerable<string> Objects { get; set; }
        public IEnumerable<string> Predicates { get; set; }
        public IEnumerable<string> Subjects { get; set; }
    }
}