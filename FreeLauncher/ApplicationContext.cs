using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CommandLine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FreeLauncher {
    public class ApplicationContext {
        private readonly string _configurationFile;

        public static readonly string VersionsFileUrl = "https://s3.amazonaws.com/Minecraft.Download/versions/versions.json";

        public Localization ProgramLocalization { get; } = new Localization();

        public string McDirectory { get; }
        public string McLauncher { get; }
        public string McVersions { get; }
        public string McVersionsFile { get; }
        public string McLibs { get; }
        public string McNatives { get; }
        public string McAssets { get; }
        public string McLegacyAssets { get; }
        public string McObjectsAssets { get; }
        public string McLauncherProfiles { get; }
        public string LauncherProfiles { get; }
        public string LauncherUsers { get; }

        public string Libraries { get; set; }

        public Configuration Configuration { get; }

        public ApplicationContext() {
            Libraries = string.Empty;
            McDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft\\");
            if (!Directory.Exists(McDirectory)) {
                Directory.CreateDirectory(McDirectory);
            }

            McLauncher = Path.Combine(McDirectory, "freelauncher\\");
            if (!Directory.Exists(McLauncher)) {
                Directory.CreateDirectory(McLauncher);
            }

            McVersions = Path.Combine(McDirectory, "versions\\");
            McVersionsFile = Path.Combine(McVersions, "versions.json");
            McLibs = Path.Combine(McDirectory, "libraries\\");
            McNatives = Path.Combine(McDirectory, "natives\\");
            McAssets = Path.Combine(McDirectory, "assets\\");
            McLegacyAssets = Path.Combine(McAssets, "legacy\\");
            McObjectsAssets = Path.Combine(McAssets, "objects\\");
            McLauncherProfiles = Path.Combine(McDirectory, "launcher_profiles.json"); // TODO: Как защитить только на чтение через API?
            LauncherProfiles = Path.Combine(McLauncher, "profiles.json");
            LauncherUsers = Path.Combine(McLauncher, "users.json");

            _configurationFile = McLauncher + "\\configuration.json";
            Configuration = GetConfiguration();
        }

        public void SaveConfiguration() {
            File.WriteAllText(_configurationFile, JsonConvert.SerializeObject(Configuration, Formatting.Indented));
        }

        private Configuration GetConfiguration() {
            Configuration configuration = null;
            if (File.Exists(_configurationFile)) {
                configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(_configurationFile));
            } else {
                configuration = new Configuration();
            }

            return configuration;
        }
    }
}