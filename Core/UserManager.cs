using System.Collections.Generic;
using Newtonsoft.Json;

namespace dotMCLauncher.Core {
    public class UserManager {
        [JsonProperty("selectedUsername")]
        public string SelectedUsername { get; set; }
        
        [JsonProperty("users")] 
        public Dictionary<string, User> Accounts { get; set; }
    }
}