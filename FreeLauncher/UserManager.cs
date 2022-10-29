using System.Collections.Generic;
using Newtonsoft.Json;

namespace FreeLauncher {
    public class UserManager {
        [JsonProperty("selectedUsername")]
        public string SelectedUsername { get; set; }
        
        [JsonProperty("users")] 
        public Dictionary<string, User> Accounts { get; set; }
    }
}