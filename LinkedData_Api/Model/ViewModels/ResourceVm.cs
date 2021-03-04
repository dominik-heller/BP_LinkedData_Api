using System.Collections.Generic;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace LinkedData_Api.Model.ViewModels
{
    public class PredicateContent
    {
        [JsonProperty("curies", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Curies;

        [JsonProperty("literals", NullValueHandling = NullValueHandling.Ignore)]
        public List<Literal> Literals;
    }

    public class ResourceVm
    {
        [JsonProperty("properties")]
        public Dictionary<string, PredicateContent> Predicates { get; set; }
    }
}