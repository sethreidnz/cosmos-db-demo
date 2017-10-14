using Newtonsoft.Json;

namespace CosmosDbDemo.Server
{
  public class EntityModel
  {
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
  }
}
