using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{
    public class NamedResourceVm : ResourceVm
    {
        [JsonProperty("resourceCurie")]
        public string ResourceCurie { get; set; }
    }
}