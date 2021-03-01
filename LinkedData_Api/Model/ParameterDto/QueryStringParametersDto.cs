using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace LinkedData_Api.Model.ParameterDto
{
    public class QueryStringParametersDto
    {
        private const int DefaultLimit = 50;
        private const int DefaultOffset = 0;
        [DefaultValue(DefaultLimit)] public int Limit { get; set; } = DefaultLimit;
        [DefaultValue(DefaultOffset)] public int Offset { get; set; } = DefaultOffset;
        public string Regex { get; set; }
         public string Sort { get; set; } // příp. OrderBy
    }
}