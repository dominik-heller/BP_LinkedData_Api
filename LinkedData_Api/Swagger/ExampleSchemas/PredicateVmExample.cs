using System.Collections.Generic;
using LinkedData_Api.Model.ViewModels;
using Swashbuckle.AspNetCore.Filters;

namespace LinkedData_Api.Swagger.ExampleSchemas
{
    public class PredicateVmExample : IExamplesProvider<PredicateVm>
    {
        public PredicateVm GetExamples()
        {
            return new PredicateVm()
            {
                Curies = new List<string>()
                {
                    "curie1", "curie2", "curie3"
                },
                Literals = new List<Literal>()
                {
                    new() {Value = "value1", Datatype = "datatype1", Language = "lang1"},
                    new() {Value = "value1", Datatype = "datatype1", Language = "lang2"}
                }
            };
        }
    }
}