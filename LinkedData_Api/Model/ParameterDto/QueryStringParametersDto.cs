namespace LinkedData_Api.Model.ParameterDto
{
    public class QueryStringParametersDto
    {
        private const int DefaultLimit = 50; 
        private const int DefaultOffset = 0; 
        public int Limit { get; set; } = DefaultLimit; //SizeOfPage (default=10)
        public int Offset { get; set; } = DefaultOffset; //PageNumber //Offset
        public string Regex { get; set; }
        public string Sort { get; set; } // příp. OrderBy
    }
}