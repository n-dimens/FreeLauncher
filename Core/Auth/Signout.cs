// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.YaDra4il.Signout
// Assembly: dotMCLauncher.YaDra4il, Version=0.0.1.67, Culture=neutral, PublicKeyToken=null
// MVID: 1BBB2245-4A1D-4E2A-A2F8-AF41A47D39FB
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\YaDra4il\0.0.1.67\dotMCLauncher.YaDra4il.dll

using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Core.Auth {
  public class Signout : Request
  {
    public Signout(string email, string password)
    {
      this.Url = "https://authserver.mojang.com/signout";
      JObject jobject = new JObject();
      jobject.Add("username", email);
      jobject.Add(nameof (password), password);
      this.ToPost = ((object) jobject).ToString();
    }

    public override Request Parse(string json) => (Request) null;
  }
}
