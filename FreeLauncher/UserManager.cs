using System.Collections.Generic;
using Newtonsoft.Json;

namespace FreeLauncher {
    public class UserManager {
        [JsonProperty("selectedUsername")] public string SelectedUsername;
        
        [JsonProperty("users")] public Dictionary<string, User> Accounts = new Dictionary<string, User>();
    }
}