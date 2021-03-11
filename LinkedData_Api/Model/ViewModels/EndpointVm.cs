using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{
    public class NamedGraph
    {
        [JsonProperty("graphName")] public string GraphName { get; set; }

        [JsonProperty("uri")] public string Uri { get; set; }
    }

    public class Namespace
    {
        [JsonProperty("prefix")] public string Prefix { get; set; }

        [JsonProperty("uri")] public string Uri { get; set; }
    }

    public class SupportedMethods
    {
        [JsonProperty("sparql1.0")] public string Sparql10 { get; set; }

        [JsonProperty("sparql1.1")] public string Sparql11 { get; set; }
    }

    public class EntryClass
    {
        [JsonProperty("graphName")] public string GraphName { get; set; }

        [JsonProperty("command")] public string Command { get; set; }
    }

    public class EntryResource
    {
        [JsonProperty("graphName")] public string GraphName { get; set; }

        [JsonProperty("command")] public string Command { get; set; }
    }

    public class EndpointVm
    {
        [JsonProperty("endpointName")] public string EndpointName { get; set; }

        [JsonProperty("endpointUrl")] public string EndpointUrl { get; set; }

        [JsonProperty("defaultGraph")] public string DefaultGraph { get; set; }

        [JsonProperty("namedGraphs")] public List<NamedGraph> NamedGraphs { get; set; }

        [JsonProperty("namespaces")] public List<Namespace> Namespaces { get; set; }

        [JsonProperty("supportedMethods")] public SupportedMethods SupportedMethods { get; set; }

        [JsonProperty("entryClass")] public List<EntryClass> EntryClass { get; set; }

        [JsonProperty("entryResource")] public List<EntryResource> EntryResource { get; set; }
    }
}