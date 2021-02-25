namespace LinkedData_Api.Model.ParameterDto
{
    public class QueryStringParametersDto
    {
        public int Limit { get; set; } = 100; //SizeOfPage (default=10)
        public int Offset { get; set; } = 0; //PageNumber //Offset
        public string Regex { get; set; }
        public string Sort { get; set; } // příp. OrderBy
    }
}