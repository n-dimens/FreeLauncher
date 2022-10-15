// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.MinecraftWindowSize
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\Core\0.0.4.84\dotMCLauncher.Core.dll

using Newtonsoft.Json;

namespace dotMCLauncher.Core
{
  public class MinecraftWindowSize
  {
    [JsonProperty("height")]
    public int Y = 480;
    [JsonProperty("width")]
    public int X = 854;

    private void SetDefaultValues()
    {
      this.Y = 480;
      this.X = 854;
    }

    public override string ToString() => "(" + (object) this.X + ";" + (object) this.Y + ")";

    public string ToCommandLineArg() => "--width " + (object) this.X + " --height " + (object) this.Y;
  }
}
