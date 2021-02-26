namespace LinkedData_Api.Model.ParameterDto
{
    public class RouteParametersDto
    {
        public string Endpoint { get; set; }
        public string Graph { get; set; }
        public string Class { get; set; }
        public string Resource { get; set; }
        public string Predicate { get; set; }

        //potřeba pokud bych implementoval wildcards * -> př. api/end1/resource/dbo:Berlin/*/*/foaf:name/....
        //public IEnumerable<string> Objects { get; set; }
        //public IEnumerable<string> Predicates { get; set; }
        //public IEnumerable<string> Subjects { get; set; }
        //+úpravy v ParametersProcessor => nutno nikoli přepsat resource a predicate, ale pridat do kolekce
    }
}