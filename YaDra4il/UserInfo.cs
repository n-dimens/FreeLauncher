// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.YaDra4il.UserInfo
// Assembly: dotMCLauncher.YaDra4il, Version=0.0.1.67, Culture=neutral, PublicKeyToken=null
// MVID: 1BBB2245-4A1D-4E2A-A2F8-AF41A47D39FB
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\YaDra4il\0.0.1.67\dotMCLauncher.YaDra4il.dll

namespace dotMCLauncher.YaDra4il
{
  public class UserInfo : Request
  {
    public string id;
    public string name;

    public UserInfo(string username)
    {
      this.Url = "https://api.mojang.com/profiles/minecraft";
      this.ToPost = "[\"" + username + "\"]";
    }

    public override Request Parse(string json)
    {
      json = json.Trim('[', ']');
      return base.Parse(json);
    }
  }
}
