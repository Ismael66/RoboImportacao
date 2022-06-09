using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Robot
{
    public class Person
    {
        [JsonProperty("first_name")]
        public string Nome { get; set; }

        [JsonProperty("last_name")]
        public string Sobrenome { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Telefone { get; set; }
    }
}