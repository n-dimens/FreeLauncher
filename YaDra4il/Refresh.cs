// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.YaDra4il.Refresh
// Assembly: dotMCLauncher.YaDra4il, Version=0.0.1.67, Culture=neutral, PublicKeyToken=null
// MVID: 1BBB2245-4A1D-4E2A-A2F8-AF41A47D39FB
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\YaDra4il\0.0.1.67\dotMCLauncher.YaDra4il.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text;

namespace dotMCLauncher.YaDra4il
{
  public class Refresh
  {
    [JsonProperty("accessToken")]
    public string accessToken;
    [JsonProperty("clientToken")]
    public string clientToken;
    public JObject user;

    public Refresh(string accessToken, string clientToken)
    {
      JObject jobject = new JObject();
      jobject.Add(nameof (accessToken), accessToken);
      jobject.Add(nameof (clientToken), clientToken);
      jobject.Add("requestUser", true);
      this.DoPost("https://authserver.mojang.com/refresh", ((object) jobject).ToString());
    }

    public void DoPost(string Url, string ToPost)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(ToPost);
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(Url);
      httpWebRequest.Method = "POST";
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.ContentLength = (long) bytes.Length;
      using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
      {
        streamWriter.Write(ToPost);
        streamWriter.Flush();
        streamWriter.Close();
      }
      JObject jobject = JObject.Parse(new StreamReader(httpWebRequest.GetResponse().GetResponseStream()).ReadToEnd());
      this.user = (JObject) jobject["user"];
      this.clientToken = ((object) jobject["clientToken"]).ToString();
      this.accessToken = ((object) jobject["accessToken"]).ToString();
    }
  }
}
