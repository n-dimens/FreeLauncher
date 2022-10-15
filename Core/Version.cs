// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.Version
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\Core\0.0.4.84\dotMCLauncher.Core.dll

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace dotMCLauncher.Core
{
  public class Version
  {
    [JsonIgnore]
    private string _arguments;
    [JsonIgnore]
    public dotMCLauncher.Core.ArgumentCollection<string, string> ArgumentCollection;

    [JsonProperty("id")]
    public string VersionId { get; set; }

    [JsonProperty("type")]
    public string ReleaseType { get; set; }

    [JsonProperty("minecraftArguments")]
    public string Arguments
    {
      get => this._arguments;
      set
      {
        this.ArgumentCollection = new dotMCLauncher.Core.ArgumentCollection<string, string>();
        this.ArgumentCollection.Parse(value);
        this._arguments = value;
      }
    }

    [JsonProperty("assets")]
    public string AssetsIndex { get; set; }

    [JsonProperty("mainClass")]
    public string MainClass { get; set; }

    [JsonProperty("libraries")]
    public List<Lib> Libs { get; set; }

    [JsonProperty("inheritsFrom")]
    public string InheritsFrom { get; set; }

    [JsonIgnore]
    public Version InheritableVersion { get; set; }

    public static Version ParseVersion(
      DirectoryInfo pathToDirectory,
      bool parseInheritableVersion = true)
    {
      string name = pathToDirectory.Name;
      Version version = File.Exists(Path.Combine(pathToDirectory.ToString(), name + ".json")) ? (Version) JsonConvert.DeserializeObject(File.ReadAllText(Path.Combine(pathToDirectory.ToString(), name + ".json")), typeof (Version)) : throw new VersionIsNotExists(string.Format("Directory '{0}' doesn't contain JSON file. Path: {1}", (object) name, (object) pathToDirectory))
      {
        Version = name
      };
      if (version.InheritsFrom == null || !parseInheritableVersion)
        return version;
      version.InheritableVersion = Version.ParseVersion(new DirectoryInfo(Path.Combine(pathToDirectory.Parent.FullName, version.InheritsFrom)));
      version.Libs.AddRange((IEnumerable<Lib>) version.InheritableVersion.Libs);
      return version;
    }
  }
}
