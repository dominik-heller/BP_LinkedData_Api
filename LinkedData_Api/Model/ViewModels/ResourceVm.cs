using System.Collections.Generic;
using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{


    public class PropertyContent
    {
        [JsonProperty("curies", NullValueHandling = NullValueHandling.Ignore)]public List<string> Curies;
        [JsonProperty("literals", NullValueHandling = NullValueHandling.Ignore)] public List<Literal> Literals;
    }

    public class ResourceVm
    {
        [JsonProperty("properties")]
        public Dictionary<string, PropertyContent> Properties { get; set; }
    }
}