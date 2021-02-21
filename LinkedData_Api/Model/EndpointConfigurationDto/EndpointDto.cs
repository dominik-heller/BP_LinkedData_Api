using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace LinkedData_Api.DataModel.EndpointConfigurationDto
{
    public class NamedGraph
    {
        [JsonProperty("graph_name")] public string GraphName { get; set; }

        [JsonProperty("uri")] public string Uri { get; set; }
    }

    public class Namespace
    {
        [JsonProperty("prefix")] public string Prefix { get; set; }

        [JsonProperty("uri")] public string Uri { get; set; }
    }

    public class SupportedMethods
    {
        [JsonProperty("sparql_1.0")] public string Sparql10 { get; set; }

        [JsonProperty("sparql_1.1")] public string Sparql11 { get; set; }
    }

    public class EntryClass
    {
        [JsonProperty("graph_name")] public string GraphName { get; set; }

        [JsonProperty("command")] public string Command { get; set; }
    }

    public class EntryResource
    {
        [JsonProperty("graph_name")] public string GraphName { get; set; }

        [JsonProperty("command")] public string Command { get; set; }
    }

    public class EndpointDto
    {
        [JsonProperty("endpoint_name")] public string EndpointName { get; set; }

        [JsonProperty("endpoint_url")] public string EndpointUrl { get; set; }

        [JsonProperty("default_graph")] public string DefaultGraph { get; set; }

        [JsonProperty("named_graphs")] public List<NamedGraph> NamedGraphs { get; set; }

        [JsonProperty("namespaces")] public List<Namespace> Namespaces { get; set; }

        [JsonProperty("supported_methods")] public SupportedMethods SupportedMethods { get; set; }

        [JsonProperty("entry_class")] public List<EntryClass> EntryClass { get; set; }

        [JsonProperty("entry_resource")] public List<EntryResource> EntryResource { get; set; }
    }
}