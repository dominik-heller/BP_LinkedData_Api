using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{
    public class NamedPredicateVm : PredicateVm
    {
        [JsonProperty("predicateCurie")]
        public string PredicateCurie { get; set; }
    }
}