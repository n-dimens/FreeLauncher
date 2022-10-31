// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.YaDra4il.AuthManager
// Assembly: dotMCLauncher.YaDra4il, Version=0.0.1.67, Culture=neutral, PublicKeyToken=null
// MVID: 1BBB2245-4A1D-4E2A-A2F8-AF41A47D39FB
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\YaDra4il\0.0.1.67\dotMCLauncher.YaDra4il.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Core.Auth {
  public class AuthManager
  {
    [JsonProperty("email")]
    public string Email;
    [JsonProperty("password")]
    public string Password;
    [JsonProperty("username")]
    public string Username;
    [JsonProperty("uuid")]
    public string Uuid;
    [JsonProperty("sessionToken")]
    public string SessionToken;
    [JsonProperty("accessToken")]
    public string AccessToken;
    [JsonProperty("demo")]
    public bool IsDemo;
    [JsonProperty("legacy")]
    public bool IsLegacy;
    public JArray UserProperties;

    public Authenticate Login()
    {
      Authenticate authenticate = this.Login(this.Email, this.Password);
      this.SessionToken = authenticate.accessToken;
      this.AccessToken = authenticate.clientToken;
      this.Username = authenticate.selectedProfile.name;
      this.Uuid = authenticate.selectedProfile.id;
      this.UserProperties = (JArray) authenticate.user["properties"];
      return authenticate;
    }

    private Authenticate Login(string email, string password) => (Authenticate) new Authenticate(email, password).DoPost();

    public void Logout() => AuthManager.Logout(this.Email, this.Password);

    private static void Logout(string email, string password) => new Signout(email, password).DoPost();

    public Refresh Refresh()
    {
      Refresh refresh = new Refresh(this.SessionToken, this.AccessToken);
      this.SessionToken = refresh.accessToken;
      this.AccessToken = refresh.clientToken;
      this.UserProperties = (JArray) refresh.user["properties"];
      return refresh;
    }

    public bool CheckSessionToken() => AuthManager.CheckSessionToken(this.SessionToken);

    private static bool CheckSessionToken(string sessionToken) => ((AuthentificationCheck) new AuthentificationCheck(sessionToken).DoPost()).valid;

    public string GetUsernameByUUID()
    {
      this.Username = new Username() { Uuid = this.Uuid }.GetUsernameByUuid();
      return this.Username;
    }

    public UserInfo GetUserInfo()
    {
      UserInfo userInfo = AuthManager.GetUserInfo(this.Username);
      this.Uuid = userInfo.id;
      return userInfo;
    }

    public static UserInfo GetUserInfo(string username) => (UserInfo) new UserInfo(username).DoPost();
  }
}
