// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.YaDra4il.Username
// Assembly: dotMCLauncher.YaDra4il, Version=0.0.1.67, Culture=neutral, PublicKeyToken=null
// MVID: 1BBB2245-4A1D-4E2A-A2F8-AF41A47D39FB
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\YaDra4il\0.0.1.67\dotMCLauncher.YaDra4il.dll

using Newtonsoft.Json.Linq;
using System.Net;

namespace dotMCLauncher.YaDra4il
{
  public class Username
  {
    public string Uuid { private get; set; }

    public string GetUsernameByUuid() => ((object) JObject.Parse(new WebClient().DownloadString("https://sessionserver.mojang.com/session/minecraft/profile/" + this.Uuid))["name"]).ToString();
  }
}
