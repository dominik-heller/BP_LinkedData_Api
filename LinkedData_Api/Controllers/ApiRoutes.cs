namespace LinkedData_Api.Controllers
{
    public static class ApiRoutes
    {
        private const string Root = "api";

        //private const string Version = "v1";
        //route with version
        //private const string Base = Root + "/" + Version;
        private const string Base = Root;

        private const string RecursivePart = "{resource?}/{predicate?}/{r2?}/{p2?}/{r3?}/{p3?}/{r4?}";

        public const string EndpointInfo =
            Base + "/{endpoint}";

        public const string EndpointGraphs =
            Base + "/{endpoint}/graphs";

        public const string DefaultGraphClassRoute =
            Base + "/{endpoint}/class/{classId?}/" + RecursivePart;

        public const string DefaultGraphResourceRoute =
            Base + "/{endpoint}/resource/{subject?}/" + RecursivePart;

        public const string NamedGraphClassRoute =
            Base + "/{endpoint}/{graph}/class/{classId?}/" + RecursivePart;

        public const string NamedGraphResourceRoute =
            Base + "/{endpoint}/{graph}/resource/{subject?}/" + RecursivePart;
    }
}