// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.YaDra4il.Authenticate
// Assembly: dotMCLauncher.YaDra4il, Version=0.0.1.67, Culture=neutral, PublicKeyToken=null
// MVID: 1BBB2245-4A1D-4E2A-A2F8-AF41A47D39FB
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\YaDra4il\0.0.1.67\dotMCLauncher.YaDra4il.dll

using Newtonsoft.Json.Linq;

namespace dotMCLauncher.YaDra4il
{
  public class Authenticate : Request
  {
    public string accessToken;
    public string clientToken;
    public UserInfo selectedProfile;
    public JObject user;

    public Authenticate(string email, string password)
    {
      this.Url = "https://authserver.mojang.com/authenticate";
      JObject jobject1 = new JObject();
      JObject jobject2 = jobject1;
      JObject jobject3 = new JObject();
      jobject3.Add("name", "Minecraft");
      jobject3.Add("version", 1);
      JObject jobject4 = jobject3;
      jobject2.Add("agent", (JToken) jobject4);
      jobject1.Add("username", email);
      jobject1.Add(nameof (password), password);
      jobject1.Add("requestUser", true);
      this.ToPost = ((object) jobject1).ToString();
    }
  }
}
