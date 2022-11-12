namespace FreeLauncher.Forms {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using dotMCLauncher.Core;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal static class LauncherExtensions {
        internal static string GetSelectedVersion(this Profile profile) {
            return profile.SelectedVersion;
        }

        public static ProfileManager ParseProfile(string pathToFile) {
            return JsonConvert.DeserializeObject<ProfileManager>(File.ReadAllText(pathToFile));
        }
    }
}
