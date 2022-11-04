using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;

namespace dotMCLauncher.Core {
    public class Version {
        [JsonIgnore]
        private string _arguments;

        [JsonIgnore]
        public ArgumentCollection<string, string> ArgumentCollection { get; private set; }

        public string Id { get; set; }

        public string Type { get; set; }

        [JsonProperty("minecraftArguments")]
        public string Arguments {
            get => _arguments;
            set {
                ArgumentCollection = new ArgumentCollection<string, string>();
                ArgumentCollection.Parse(value);
                _arguments = value;
            }
        }

        public Artifact AssetIndex { get; set; }

        public string Assets { get; set; }

        public VersionDownloads Downloads { get; set; }

        public string MainClass { get; set; }

        public List<Lib> Libraries { get; set; }

        [JsonProperty("inheritsFrom")]
        public string InheritsFrom { get; set; }

        [JsonIgnore]
        public Version InheritableVersion { get; set; }

        public static Version ParseVersion(DirectoryInfo versionDirectory, bool parseInheritableVersion = true) {
            var versionName = versionDirectory.Name;
            var versionFile = Path.Combine(versionDirectory.ToString(), versionName + ".json");
            if (!File.Exists(versionFile)) {
                throw new VersionNotExistException($"Directory '{versionName}' doesn't contain JSON file. Path: {versionDirectory}") {
                    Version = versionName
                };
            }

            var version = JsonConvert.DeserializeObject<Version>(File.ReadAllText(versionFile));
            if (version.InheritsFrom == null || !parseInheritableVersion) {
                return version;
            }

            version.InheritableVersion = ParseVersion(new DirectoryInfo(Path.Combine(versionDirectory.Parent.FullName, version.InheritsFrom)));
            version.Libraries.AddRange(version.InheritableVersion.Libraries);
            return version;
        }
    }

    public class VersionDownloads {
        public Artifact Client { get; set; }

        public Artifact Server { get; set; }

        public Artifact WindowsServer { get; set; }
    }
}
