// Decompiled with JetBrains decompiler
// Type: dotMCLauncher.Core.ArgumentCollection`2
// Assembly: dotMCLauncher.Core, Version=0.0.4.84, Culture=neutral, PublicKeyToken=null
// MVID: 3319DB9D-31E6-4AD0-8FD9-640DCB0404A7
// Assembly location: D:\projects\minecraft\FreeLauncher\lib\dotMCLauncher\Core\0.0.4.84\dotMCLauncher.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace dotMCLauncher.Core {
    public class ArgumentCollection<T1, T2> : Dictionary<string, string> {
        private string _argLine;

        public void Parse(string argLine) {
            Regex regex = new Regex("\\-\\-(\\w+) (\\S+)", RegexOptions.IgnoreCase);
            MatchCollection matchCollection = regex.Matches(argLine);
            for (int i = 0; i < matchCollection.Count; ++i) {
                if (!this.ContainsKey(matchCollection[i].Groups[1].Value))
                    base.Add(matchCollection[i].Groups[1].Value, matchCollection[i].Groups[2].Value);
                else
                    this[matchCollection[i].Groups[1].Value] = matchCollection[i].Groups[2].Value;
            }
            this._argLine = regex.Replace(argLine, string.Empty).Trim();
        }

        public new void Add(string key, string value = null) => base.Add(key, value);

        public string ToString(Dictionary<string, string> values = null) {
            Regex regex = new Regex("\\$\\{(\\w+)\\}", RegexOptions.IgnoreCase);
            string str = !string.IsNullOrEmpty(this._argLine) ? regex.Replace(this._argLine, (MatchEvaluator)(match => {
                if (!values.ContainsKey(match.Groups[1].Value))
                    return match.Value;
                return values[match.Groups[1].Value].Contains<char>(' ') ? string.Format("\"{0}\"", (object)values[match.Groups[1].Value]) : values[match.Groups[1].Value];
            })) + " " : string.Empty;

            foreach (string key1 in this.Keys) {
                string key = key1;
                string source = string.Empty;
                if (this[key] != null && values != null && regex.IsMatch(this[key])) {
                    // TODO: reference to a compiler-generated method
                    // source = regex.Replace(this[key], (MatchEvaluator)(match => !values.ContainsKey(match.Groups[1].Value) ? this.\u003C\u003En__0(key) : values[match.Groups[1].Value]));
                    source = regex.Replace(this[key], (MatchEvaluator)(match => !values.ContainsKey(match.Groups[1].Value) ? this[key] : values[match.Groups[1].Value]));
                }
                else if (this[key] != null)
                    source = this[key];
                if (source.Contains<char>(' '))
                    source = string.Format("\"{0}\"", (object)source);
                if (source != string.Empty)
                    source = " " + source;
                str = str + "--" + key + source + " ";
            }
            return str.Substring(0, str.Length - 1);
        }
    }
}
