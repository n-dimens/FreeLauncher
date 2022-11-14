namespace NDimens.Minecraft.FreeLauncher.Core;

using System.Collections.Generic;

using Newtonsoft.Json;

public class UserManager {
    [JsonProperty("SelectedUsername")]
    public string LastUserName { get; set; } = string.Empty;

    public Dictionary<string, User> Users { get; set; } = new Dictionary<string, User>();
}