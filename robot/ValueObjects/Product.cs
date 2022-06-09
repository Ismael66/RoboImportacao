using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Robot
{
    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Nome { get; set; }
    }
}