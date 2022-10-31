// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.YaDra4il.Request
// Assembly: dotMCLauncher.YaDra4il, Version=0.0.1.67, Culture=neutral, PublicKeyToken=null
// MVID: 1BBB2245-4A1D-4E2A-A2F8-AF41A47D39FB
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\YaDra4il\0.0.1.67\dotMCLauncher.YaDra4il.dll

using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace dotMCLauncher.Core.Auth {
  public abstract class Request
  {
    public string Url;
    public string ToPost;

    public virtual Request DoPost()
    {
      byte[] bytes = Encoding.UTF8.GetBytes(this.ToPost);
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(this.Url);
      httpWebRequest.Method = "POST";
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.ContentLength = (long) bytes.Length;
      using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
      {
        streamWriter.Write(this.ToPost);
        streamWriter.Flush();
        streamWriter.Close();
      }
      return this.Parse(new StreamReader(httpWebRequest.GetResponse().GetResponseStream()).ReadToEnd());
    }

    public virtual Request Parse(string json) => (Request) JsonConvert.DeserializeObject(json, this.GetType());
  }
}
