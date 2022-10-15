// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.ProfileManager
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\Core\0.0.4.84\dotMCLauncher.Core.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dotMCLauncher.Core
{
  public class ProfileManager
  {
    [JsonProperty("selectedProfile")]
    public string LastUsedProfile { get; set; }

    [JsonProperty("profiles")]
    public Dictionary<string, Profile> Profiles { get; set; }

    public static ProfileManager ParseProfile(string pathToFile) => (ProfileManager) JsonConvert.DeserializeObject(File.ReadAllText(pathToFile), typeof (ProfileManager));

    public string ToJson() => this.ToJson((Formatting) 1, new JsonSerializerSettings()
    {
      NullValueHandling = (NullValueHandling) 1
    });

    public string ToJson(Formatting formatting) => this.ToJson(formatting, new JsonSerializerSettings()
    {
      NullValueHandling = (NullValueHandling) 1
    });

    public string ToJson(Formatting formatting, JsonSerializerSettings settings) => JsonConvert.SerializeObject((object) this, (Formatting) 1, new JsonSerializerSettings()
    {
      NullValueHandling = (NullValueHandling) 1
    });

    public void AddProfile(Profile profile)
    {
      if (string.IsNullOrWhiteSpace(profile.ProfileName))
        throw new Exception("Field 'ProfileName' couldn't be empty");
      if (this.Profiles.Keys.Contains<string>(profile.ProfileName))
        throw new Exception("Profile '" + profile.ProfileName + "' already exist in list.");
      this.Profiles.Add(profile.ProfileName, profile);
    }

    public void DeleteProfile(Profile profile)
    {
      if (string.IsNullOrWhiteSpace(profile.ProfileName))
        throw new Exception("Field 'ProfileName' couldn't be empty");
      this.Profiles.Remove(profile.ProfileName);
    }

    public void RenameProfile(Profile profile, string newName)
    {
      if (this.LastUsedProfile == profile.ProfileName)
        this.LastUsedProfile = newName;
      this.Profiles.Remove(profile.ProfileName);
      profile.ProfileName = newName;
      this.Profiles[newName] = profile;
    }
  }
}
