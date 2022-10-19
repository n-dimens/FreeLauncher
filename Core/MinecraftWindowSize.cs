// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.MinecraftWindowSize
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\Core\0.0.4.84\dotMCLauncher.Core.dll

using Newtonsoft.Json;

namespace dotMCLauncher.Core {
    public class MinecraftWindowSize {
        [JsonProperty("width")]
        public int X { get; set; } = 854;

        [JsonProperty("height")]
        public int Y { get; set; } = 480;

        public override string ToString() {
            return $"({X};{Y})";
        }
    }
}
