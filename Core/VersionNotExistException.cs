// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.VersionIsNotExists
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\Core\0.0.4.84\dotMCLauncher.Core.dll

using System;

namespace dotMCLauncher.Core {
    public class VersionNotExistException : Exception {
        public string Version;

        public VersionNotExistException(string message)
          : base(message) {
        }
    }
}
