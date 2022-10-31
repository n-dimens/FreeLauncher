// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.YaDra4il.AuthentificationCheck
// Assembly: dotMCLauncher.YaDra4il, Version=0.0.1.67, Culture=neutral, PublicKeyToken=null
// MVID: 1BBB2245-4A1D-4E2A-A2F8-AF41A47D39FB
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\YaDra4il\0.0.1.67\dotMCLauncher.YaDra4il.dll

namespace dotMCLauncher.Core.Auth {
  public class AuthentificationCheck : Request
  {
    public bool valid;

    public AuthentificationCheck(string session)
    {
      this.Url = "https://authserver.mojang.com/validate";
      this.ToPost = "{\"accessToken\":\"" + session + "\"}";
    }

    public override Request DoPost()
    {
      try
      {
        base.DoPost();
        this.valid = true;
      }
      catch
      {
        this.valid = false;
      }
      return (Request) this;
    }

    public override Request Parse(string json) => (Request) null;
  }
}
