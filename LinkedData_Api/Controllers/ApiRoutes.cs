namespace LinkedData_Api.Controllers
{
    public static class ApiRoutes
    {
        private const string Root = "api";

        //private const string Version = "v1";
        //route with version
        //private const string Base = Root + "/" + Version;
        private const string Base = Root;
        private const string regexConstraint = ":regex(.+:.+)";
        private const string RecursivePart = "/{r2" + regexConstraint + "}"; // /{p2?}/{r3?}/{p3?}/{r4?}";
        public const string DefaultGraphClasses = Base + "/{endpoint}/classes";
        public const string NamedGraphClasses = Base + "/{endpoint}/{graph}/classes";
        public const string DefaultGraphResources = Base + "/{endpoint}/resources";
        public const string NamedGraphResources = Base + "/{endpoint}/{graph}/resources";
        public const string DefaultGraphConcreteClass = DefaultGraphClasses + "/{class}";
        public const string NamedGraphConcreteClass = NamedGraphClasses + "/{class}";

        public const string DefaultGraphClassesConcreteResource =
            DefaultGraphConcreteClass + "/{resource" + regexConstraint + "}";

        public const string NamedGraphClassesConcreteResource =
            NamedGraphConcreteClass + "/{resource" + regexConstraint + "}";

        public const string DefaultGraphResourcesConcreteResource =
            DefaultGraphResources + "/{resource" + regexConstraint + "}";

        public const string NamedGraphResourcesConcreteResource =
            NamedGraphResources + "/{resource" + regexConstraint + "}";

        public const string DefaultGraphClassStartConcreteResourcePredicate =
            DefaultGraphClassesConcreteResource + "/{predicate" + regexConstraint + "}";

        public const string NamedGraphClassStartConcreteResourcePredicate =
            NamedGraphClassesConcreteResource + "/{predicate" + regexConstraint + "}";

        public const string DefaultGraphResourceStartConcreteResourcePredicate =
            DefaultGraphResourcesConcreteResource + "/{predicate" + regexConstraint + "}";

        public const string NamedGraphResourceStartConcreteResourcePredicate =
            NamedGraphResourcesConcreteResource + "/{predicate" + regexConstraint + "}";

        public const string DefaultGraphClassStartRecursiveRoute =
            DefaultGraphClassStartConcreteResourcePredicate + RecursivePart;

        public const string NamedGraphClassStartRecursiveRoute =
            NamedGraphClassStartConcreteResourcePredicate + RecursivePart;

        public const string DefaultGraphResourceStartRecursiveRoute =
            DefaultGraphResourceStartConcreteResourcePredicate + RecursivePart;

        public const string NamedGraphResourceStartRecursiveRoute =
            NamedGraphResourceStartConcreteResourcePredicate + RecursivePart;

        public const string EndpointConfiguration =
            Base + "/{endpoint}";

        public const string PostEndpoints =
            Base + "/endpoints";

        public const string EndpointGraphs =
            Base + "/{endpoint}/graphs";

        public const string EndpointNamespacePrefix = Base + "/namespaces/{prefix}";
    }
}