using System.Collections.Generic;
using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{
    public class PropertyName
    {
        [JsonProperty("literals")] public List<LiteralVm.Literal> Literals { get; set; }

        [JsonProperty("curies")] public List<string> Curies { get; set; }
    }

    public class Property
    {
        [JsonProperty("property_name")] public PropertyName PropertyName { get; set; }
    }

    public class Resource
    {
        [JsonProperty("properties")] public List<Property> Properties { get; set; }
    }

    public class ResourceVm
    {
        [JsonProperty("resource")] public Resource Resource { get; set; }
    }

}

