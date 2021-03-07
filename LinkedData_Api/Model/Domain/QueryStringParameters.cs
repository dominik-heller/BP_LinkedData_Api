using System.ComponentModel;

namespace LinkedData_Api.Model.Domain
{
    public class QueryStringParameters
    {
        private const int DefaultLimit = 50;
        private const int DefaultOffset = 0;
        [DefaultValue(DefaultLimit)] public int Limit { get; set; } = DefaultLimit;
        [DefaultValue(DefaultOffset)] public int Offset { get; set; } = DefaultOffset;
        public string Regex { get; set; }
        public string Sort { get; set; } // příp. OrderBy
    }
}