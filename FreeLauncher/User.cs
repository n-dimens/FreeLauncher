using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FreeLauncher {
    public class User {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("sessionToken")]
        public string SessionToken { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("properties")]
        public JArray UserProperties { get; set; }
    }
}