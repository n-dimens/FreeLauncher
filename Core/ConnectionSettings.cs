// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.ConnectionSettings
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7

namespace dotMCLauncher.Core {
    using Newtonsoft.Json;

    public class ConnectionSettings {
        [JsonProperty("ip")]
        public string ServerIP;

        [JsonProperty("port")]
        public int ServerPort = 25565;
    }
}
