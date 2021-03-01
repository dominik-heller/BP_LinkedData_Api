using System.Collections.Generic;
using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{
    public class PredicateVm
    {
        [JsonProperty("literals")] public List<Literal> Literals { get; set; }
        [JsonProperty("curies")] public List<string> Curies { get; set; }
    }
}