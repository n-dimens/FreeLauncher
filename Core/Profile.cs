// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.Profile
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\Core\0.0.4.84\dotMCLauncher.Core.dll

using FreeLauncher;

using Newtonsoft.Json;

namespace dotMCLauncher.Core {
    public class Profile {
        [JsonProperty("name")]
        public string ProfileName { get; set; }

        [JsonProperty("gameDir")]
        public string WorkingDirectory { get; set; }

        [JsonProperty("lastVersionId")]
        public string SelectedVersion { get; set; }

        [JsonProperty("launcherVisibilityOnGameClose")]
        private string _launcherVisibilityOnGameClose { get; set; }

        [JsonProperty("javaDir")]
        public string JavaExecutable { get; set; }

        [JsonProperty("javaArgs")]
        public string JavaArguments { get; set; }

        [JsonIgnore]
        public Profile.LauncherVisibility LauncherVisibilityOnGameClose {
            get {
                string visibilityOnGameClose = this._launcherVisibilityOnGameClose;
                if (visibilityOnGameClose == "hide launcher and re-open when game closes")
                    return Profile.LauncherVisibility.HIDDEN;
                return visibilityOnGameClose == "close launcher when game starts" ? Profile.LauncherVisibility.CLOSED : Profile.LauncherVisibility.VISIBLE;
            }
            set {
                switch (value) {
                    case Profile.LauncherVisibility.HIDDEN:
                        this._launcherVisibilityOnGameClose = "hide launcher and re-open when game closes";
                        break;
                    case Profile.LauncherVisibility.CLOSED:
                        this._launcherVisibilityOnGameClose = "close launcher when game starts";
                        break;
                    default:
                        this._launcherVisibilityOnGameClose = "keep the launcher open";
                        break;
                }
            }
        }

        public string ToString(Formatting formatting = 0) {
            return JsonConvert.SerializeObject(this, formatting, new JsonSerializerSettings() {
                NullValueHandling = (NullValueHandling)1
            });
        }

        public static Profile ParseProfile(string json) {
            return JsonConvert.DeserializeObject<Profile>(json);
        }

        public enum LauncherVisibility {
            VISIBLE,
            HIDDEN,
            CLOSED,
        }

        public static Profile CreateDefault() {
            // TODO: Java exe, gamedir, lastVersionId
            var profile = new Profile {
                ProfileName = "default",
                LauncherVisibilityOnGameClose = LauncherVisibility.VISIBLE,
                JavaExecutable = JavaUtils.GetJavaExecutable(),
                JavaArguments = JavaUtils.DEFAULT_ARGS
            };

            return profile;
        }
    }
}
