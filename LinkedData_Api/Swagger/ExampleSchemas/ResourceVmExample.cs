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
                        "propertyName",
                        new PropertyContent()
                        {
                            Curies = new List<string>() {"curie1", "curie2", "curie3"},
                            Literals = new List<Literal>()
                            {
                                new() {Value = "value1", Datatype = "datatype1", Language = "lang1"},
                                new() {Value = "value1", Datatype = "datatype1", Language = "lang1"}
                            }
                        }
                    }
                }
            };
        }
    }
}