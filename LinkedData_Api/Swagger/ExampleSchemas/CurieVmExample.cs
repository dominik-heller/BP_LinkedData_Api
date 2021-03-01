using System.Collections.Generic;
using LinkedData_Api.Model.ViewModels;
using Swashbuckle.AspNetCore.Filters;

namespace LinkedData_Api.Swagger.ExampleSchemas
{
    public class CurieVmExample : IExamplesProvider<CurieVm>
    {
        public CurieVm GetExamples()
        {
            return new CurieVm()
            {
                Curies = new List<string>()
                {
                    "curie1", "curie2", "curie3"
                }
            };
        }
    }
}