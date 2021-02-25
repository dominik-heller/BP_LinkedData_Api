namespace LinkedData_Api.Model.ParameterDto
{
    public class QueryStringParametersDto
    {
        public int Limit { get; set; } = 10; //SizeOfPage (default=10)
        public int Page { get; set; } = 1; //PageNumber //Offset
        public string Regex { get; set; }
        public string Sort { get; set; } // příp. OrderBy
    }
}