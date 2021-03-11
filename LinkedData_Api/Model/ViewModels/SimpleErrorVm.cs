using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{
    public class CustomErrorVm
    {
        [JsonProperty("customErrorMessage")]
        public string CustomErrorMessage { get; set; }
        
        [JsonProperty("generatedQuery",NullValueHandling=NullValueHandling.Ignore)]
        public string GeneratedQuery { get; set; }
    }
}