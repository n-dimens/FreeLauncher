using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;

namespace dotMCLauncher.Core {
    public class Lib {
        public string Name { get; set; }

        public string Url { get; set; }

        public LibNatives Natives { get; set; }

        public List<Rule> Rules { get; set; }

        public LibDownloads Downloads { get; set; }

        [JsonIgnore]
        public string IsNative => Natives?.Windows?.ToString().Replace("${arch}", IntPtr.Size == 8 ? "64" : "32");

        public bool IsForWindows() {
            if (this.Rules == null) {
                return true;
            }

            bool flag = false;
            foreach (Rule rule in this.Rules) {
                string action = rule.action;
                if (!(action == "allow")) {
                    if (action == "disallow")
                        flag = rule.os == null ? flag : ((object)rule.os["name"]).ToString() != "windows";
                }
                else
                    flag = rule.os == null || ((object)rule.os["name"]).ToString() == "windows";
            }
            return flag;
        }

        public string ToPath() {
            string[] strArray = this.Name.Split(':');
            return string.Format("{0}\\{1}\\{2}\\{1}-{2}" + (!string.IsNullOrEmpty(this.IsNative) ? "-" + this.IsNative : string.Empty) + ".jar", (object)strArray[0].Replace('.', '\\'), (object)strArray[1], (object)strArray[2]);
        }
    }

    public class LibNatives {
        public string Linux { get; set; }

        public string Windows { get; set; }
    }

    public class LibDownloads {
        public Artifact Artifact { get; set; }     
    }
}
