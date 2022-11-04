namespace dotMCLauncher.Core {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Artifact {
        public string Sha1 { get; set; }

        public string Url { get; set; }

        public long Size { get; set; }
    }
}
