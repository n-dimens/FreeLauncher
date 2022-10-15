// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.Lib
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\Core\0.0.4.84\dotMCLauncher.Core.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace dotMCLauncher.Core
{
  public class Lib
  {
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("url")]
    public string Url;
    [JsonProperty("natives")]
    private JObject _natives;
    [JsonProperty("rules")]
    private List<Rule> Rules;

    [JsonIgnore]
    public string IsNative => ((object) this._natives?["windows"])?.ToString().Replace("${arch}", IntPtr.Size == 8 ? "64" : "32");

    public bool IsForWindows()
    {
      if (this.Rules == null)
        return true;
      bool flag = false;
      foreach (Rule rule in this.Rules)
      {
        string action = rule.action;
        if (!(action == "allow"))
        {
          if (action == "disallow")
            flag = rule.os == null ? flag : ((object) rule.os["name"]).ToString() != "windows";
        }
        else
          flag = rule.os == null || ((object) rule.os["name"]).ToString() == "windows";
      }
      return flag;
    }

    public string ToPath()
    {
      string[] strArray = this.Name.Split(':');
      return string.Format("{0}\\{1}\\{2}\\{1}-{2}" + (!string.IsNullOrEmpty(this.IsNative) ? "-" + this.IsNative : string.Empty) + ".jar", (object) strArray[0].Replace('.', '\\'), (object) strArray[1], (object) strArray[2]);
    }
  }
}
