namespace LinkedData_Api.Controllers
{
    public static class ApiRoutes
    {
        private const string Root = "api";

        //private const string Version = "v1";
        //route with version
        //private const string Base = Root + "/" + Version;
        private const string Base = Root;


       // private const string ClassRecursivePart = "/{resource?}/{predicate?}/{r2?}/{p2?}/{r3?}/{p3?}/{r4?}";
        private const string RecursivePart = "/{r2?}/{p2?}/{r3?}/{p3?}/{r4?}";

        public const string DefaultGraphClasses = Base + "/{endpoint}/classes";
        public const string NamedGraphClasses = Base + "/{endpoint}/{graph}/classes";
        public const string DefaultGraphResources = Base + "/{endpoint}/resources";
        public const string NamedGraphResources = Base + "/{endpoint}/{graph}/resources";
        public const string DefaultGraphConcreteClass = DefaultGraphClasses + "/{class}";
        public const string NamedGraphConcreteClass = NamedGraphClasses + "/{class}";
        public const string DefaultGraphClassesConcreteResource = DefaultGraphConcreteClass + "/{resource}";
        public const string NamedGraphClassesConcreteResource = NamedGraphConcreteClass + "/{resource}";
        public const string DefaultGraphResourcesConcreteResource = DefaultGraphResources + "/{resource}";
        public const string NamedGraphResourcesConcreteResource = NamedGraphResources + "/{resource}";
        public const string DefaultGraphClassStartConcreteResourcePredicate = DefaultGraphClassesConcreteResource + "/{predicate}";
        public const string NamedGraphClassStartConcreteResourcePredicate = NamedGraphClassesConcreteResource + "/{predicate}";
        public const string DefaultGraphResourceStartConcreteResourcePredicate = DefaultGraphResourcesConcreteResource + "/{predicate}";
        public const string NamedGraphResourceStartConcreteResourcePredicate = NamedGraphResourcesConcreteResource + "/{predicate}";
        public const string DefaultGraphClassStartRecursiveRoute = NamedGraphClassStartConcreteResourcePredicate + RecursivePart;
        public const string NamedGraphClassStartRecursiveRoute = DefaultGraphResourceStartConcreteResourcePredicate + RecursivePart;
        public const string DefaultGraphResourceStartRecursiveRoute = NamedGraphResourcesConcreteResource + RecursivePart;
        public const string NamedGraphResourceStartRecursiveRoute = NamedGraphResourceStartConcreteResourcePredicate + RecursivePart;
        
        
        
        public const string EndpointInfo =
            Base + "/{endpoint}";

        public const string EndpointGraphs =
            Base + "/{endpoint}/graphs";
        /*
        private const string RecursivePart = "/{resource?}/{predicate?}/{r2?}/{p2?}/{r3?}/{p3?}/{r4?}";


        public const string DefaultGraphClassRoute =
            Base + "/{endpoint}/class/{class?}" + RecursivePart;

        public const string DefaultGraphResourceRoute =
            Base + "/{endpoint}/resource/{subject?}" + RecursivePart;

        public const string NamedGraphClassRoute =
            Base + "/{endpoint}/{graph}/class/{class?}" + RecursivePart;

        public const string NamedGraphResourceRoute =
            Base + "/{endpoint}/{graph}/resource/{subject?}" + RecursivePart;
            */
    }
}