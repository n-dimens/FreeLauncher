// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.ProfileManager
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\Core\0.0.4.84\dotMCLauncher.Core.dll

using dotMCLauncher.Core;

using Newtonsoft.Json;

namespace NDimens.Minecraft.FreeLauncher.Core;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ProfileManager {
    [JsonProperty("selectedProfile")]
    public string LastUsedProfile { get; set; }

    [JsonProperty("profiles")]
    public Dictionary<string, Profile> Profiles { get; set; }

    public static ProfileManager Default() {
        return new ProfileManager {
            Profiles = new Dictionary<string, Profile> {
                { "default", Profile.CreateDefault() }
            },
            LastUsedProfile = "default"
        };
    }
}
