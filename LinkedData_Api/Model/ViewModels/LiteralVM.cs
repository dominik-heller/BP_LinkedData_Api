using System.Collections.Generic;
using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{
    public class Literal
    {
        [JsonProperty("value")] public string Value { get; set; }

        [JsonProperty("datatype")] public string Datatype { get; set; }

        [JsonProperty("language")] public string Language { get; set; }
    }

    public class LiteralVm
    {
        [JsonProperty("literals")] public List<Literal> Literals { get; set; }

        public LiteralVm()
        {
            Literals = new List<Literal>();
        }
    }
}