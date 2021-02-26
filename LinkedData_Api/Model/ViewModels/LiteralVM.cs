using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{
    public class LiteralVm
    {
        public class Literal
        {
            [JsonProperty("value")] public string Value { get; set; }

            [JsonProperty("datatype")] public string Datatype { get; set; }

            [JsonProperty("language")] public string Language { get; set; }
        }

        [JsonProperty("literals")] public List<Literal> Literals { get; set; }

        public LiteralVm()
        {
            Literals = new List<Literal>();
        }
    }
}