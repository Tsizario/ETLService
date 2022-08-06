using Newtonsoft.Json;

namespace ETLService.Models.OutputModels;

public class Service
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "payers")]
    public Payer[] Payers { get; set; }

    [JsonProperty(PropertyName = "total")]
    public int PayersTotal => Payers.Length;
}