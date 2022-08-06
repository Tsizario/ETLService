using Newtonsoft.Json;

namespace ETLService.Models.OutputModels;

public class TransactionInfo
{
    [JsonProperty(PropertyName = "city")]
    public string City { get; set; }

    [JsonProperty(PropertyName = "services")]
    public Service[] Services { get; set; }

    [JsonProperty(PropertyName = "total")]
    public int ServicesTotal => Services.Length;
}