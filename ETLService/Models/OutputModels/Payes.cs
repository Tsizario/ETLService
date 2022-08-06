using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ETLService.Models.OutputModels;

public class Payer
{
    [JsonIgnore]
    public string FirstName { get; set; }

    [JsonIgnore]
    public string LastName { get; set; }

    [JsonProperty(PropertyName = "payment")]
    public decimal Payment { get; set; }

    [JsonProperty(PropertyName = "date")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [JsonProperty(PropertyName = "account_number")]
    public long AccountNumber { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name => $"{LastName} {FirstName}";
}