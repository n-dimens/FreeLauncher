using System.Collections.Generic;
using Newtonsoft.Json;

namespace dotMCLauncher.Core {
    public class UserManager {
        public string SelectedUsername { get; set; } = string.Empty;

        public Dictionary<string, User> Users { get; set; } = new Dictionary<string, User>();
    }
}