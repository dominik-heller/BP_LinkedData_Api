using System.Collections.Generic;
using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{
    public class CurieVm
    {
        [JsonProperty("curies")]
        public List<string> Curies { get; set; }

        public CurieVm()
        {
            Curies = new List<string>();
        }
    }
}