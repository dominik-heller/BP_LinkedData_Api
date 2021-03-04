using System.Collections.Generic;
using LinkedData_Api.Model.ViewModels;
using Swashbuckle.AspNetCore.Filters;

namespace LinkedData_Api.Swagger.ExampleSchemas
{
    public class NamedPredicateVmExample : IExamplesProvider<NamedPredicateVm>
    {
        public NamedPredicateVm GetExamples()
        {
            return new NamedPredicateVm()
            {
                PredicateCurie = "ex:examplePredicate",
                Curies = new List<string>()
                {
                    "ex:curie1", "ex:curie2", "ex:curie3"
                },
                Literals = new List<Literal>()
                {
                    new() {Value = "value1", Datatype = "ex:exampleDatatype"},
                    new() {Value = "value2", Language = "en"},
                    new() {Value = "value3"}
                }
            };
        }
    }
}