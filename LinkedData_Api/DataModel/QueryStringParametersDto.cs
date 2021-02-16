namespace LinkedData_Api.DataModel
{
    public class QueryStringParametersDto
    {
        public int Size { get; set; } //SizeOfPage
        public int Page { get; set; } //PageNumber
        public int Limit { get; set; }
        public string Regex { get; set; }
        public string Sort { get; set; } // příp. OrderBy
    }
}