using System.Collections.Generic;
using LinkedData_Api.Model.ViewModels;
using Swashbuckle.AspNetCore.Filters;

namespace LinkedData_Api.Swagger.ExampleSchemas
{
    public class ResourceVmExample : IExamplesProvider<ResourceVm>
    {
        public ResourceVm GetExamples()
        {
            return new ResourceVm()
            {
                Properties = new Dictionary<string, PropertyContent>()
                {
                    {
                        "ex:exampleProperty",
                        new PropertyContent()
                        {
                            Curies = new List<string>() {"ex:curie1", "ex:curie2", "ex:curie3"},
                            Literals = new List<Literal>()
                            {
                                new() {Value = "Example language text...", Language = "en"},
                                new() {Value = "2021-03-02T20:00:00-01:00", Datatype = "xsd:dateTime"},
                                new() {Value = "String value"}
                            }
                        }
                    }
                }
            };
        }
    }
}