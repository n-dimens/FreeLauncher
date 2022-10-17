namespace FreeLauncher.Forms {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using dotMCLauncher.Core;

    using Newtonsoft.Json.Linq;

    internal static class LauncherExtensions {
        internal static string GetSelectedVersion(this Profile profile, ApplicationContext appContext) {
            return profile.SelectedVersion ?? profile.GetLatestVersion(appContext);
        }

        internal static string GetLatestVersion(this Profile profile, ApplicationContext appContext) {
            JObject versionsList = JObject.Parse(File.ReadAllText(appContext.McVersions + "\\versions.json"));
            return profile.AllowedReleaseTypes != null
                ? profile.AllowedReleaseTypes.Contains("snapshot")
                    ? versionsList["latest"]["snapshot"].ToString()
                    : versionsList["latest"]["release"].ToString()
                : versionsList["latest"]["release"].ToString();
        }
    }
}
