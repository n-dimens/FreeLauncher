namespace NDimens.Minecraft.FreeLauncher.Core;

using System.Collections.Generic;

public class UserManager {
    public string SelectedUsername { get; set; } = string.Empty;

    public Dictionary<string, User> Users { get; set; } = new Dictionary<string, User>();
}