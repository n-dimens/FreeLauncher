// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.Rule
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7

using Newtonsoft.Json.Linq;

namespace dotMCLauncher.Core {
    public class Rule {
        public string action { get; set; }

        public JObject os { get; set; }
    }
}
