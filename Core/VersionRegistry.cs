namespace dotMCLauncher.Core {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class VersionRegistry {
        public LatestVersion Latest { get; set; }

        public List<VersionRegistryItem> Versions { get; set; }
    }

    public class LatestVersion {
        public string Release { get; set; }

        public string Snapshot { get; set; }
    }

    public class VersionRegistryItem {
        public string Id { get; set; }

        public DateTime ReleaseTime { get; set; }

        public DateTime Time { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }
    }
}
