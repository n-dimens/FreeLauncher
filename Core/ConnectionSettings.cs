// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.ConnectionSettings
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\Core\0.0.4.84\dotMCLauncher.Core.dll

using Newtonsoft.Json;

namespace dotMCLauncher.Core
{
  public class ConnectionSettings
  {
    [JsonProperty("ip")]
    public string ServerIP;
    [JsonProperty("port")]
    public int ServerPort = 25565;

    public string BuildIP() => this.ServerIP + ":" + (object) this.ServerPort;

    public string BuildArguments() => string.Format("--server {0} --port {1}", (object) this.ServerIP, (object) this.ServerPort);
  }
}
