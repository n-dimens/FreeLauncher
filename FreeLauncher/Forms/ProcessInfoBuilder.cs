using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using dotMCLauncher.Core;
using dotMCLauncher.Core.Auth;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FreeLauncher.Forms {
    public class ProcessInfoBuilder {
        private readonly GameFileStructure _applicationContext;
        private Profile _selectedProfile;
        private User _selectedUser;
        private Version _selectedVersion;

        public ProcessInfoBuilder(GameFileStructure applicationContext) {
            _applicationContext = applicationContext;
        }

        public ProcessInfoBuilder Profile(Profile profile) {
            _selectedProfile = profile;
            return this;
        }
        
        public ProcessInfoBuilder Version(Version version) {
            _selectedVersion = version;
            return this;
        }
        
        public ProcessInfoBuilder User(User user) {
            _selectedUser = user;
            return this;
        }

        public ProcessStartInfo Build() {
            JObject properties = new JObject {
                new JProperty("freelauncher", new JArray("cheeki_breeki_iv_damke"))
            };
            
            string javaArgumentsTemp = _selectedProfile.JavaArguments == null
                ? string.Empty
                : _selectedProfile.JavaArguments;
            
            string userName = _selectedUser.Type == "offline" ? _selectedUser.Username : new Username() { Uuid = _selectedUser.Uuid }.GetUsernameByUuid();
            string userProperties = _selectedVersion.ArgumentCollection.ToString(new Dictionary<string, string> {
                { "auth_player_name", userName },
                { "version_name", _selectedProfile.ProfileName },
                { "game_directory", _selectedProfile.WorkingDirectory ?? _applicationContext.McDirectory },
                { "assets_root", _applicationContext.McAssets },
                { "game_assets", _applicationContext.McLegacyAssets },
                { "assets_index_name", _selectedVersion.AssetsIndex },
                { "auth_session", _selectedUser.AccessToken ?? "sample_token" },
                { "auth_access_token", _selectedUser.SessionToken ?? "sample_token" },
                { "auth_uuid", _selectedUser.Uuid ?? "sample_token" },
                { "user_properties", _selectedUser.UserProperties?.ToString(Formatting.None) ?? properties.ToString(Formatting.None) },
                { "user_type", _selectedUser.Type }
            });
            
            string classPath = _applicationContext.Libraries.Contains(' ') ? "\"" + _applicationContext.Libraries + "\"" : _applicationContext.Libraries;
            string nativeLibraries = _applicationContext.McNatives;
            ProcessStartInfo proc = new ProcessStartInfo {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = _selectedProfile.JavaExecutable ?? JavaUtils.GetJavaExecutable(),
                StandardErrorEncoding = Encoding.UTF8,
                WorkingDirectory = _selectedProfile.WorkingDirectory ?? _applicationContext.McDirectory,
                Arguments = $"{javaArgumentsTemp} -Djava.library.path={nativeLibraries} -cp {classPath} {_selectedVersion.MainClass} {userProperties}"
            };

            return proc;
        }

        public static ProcessInfoBuilder Create(GameFileStructure applicationContext) {
            return new ProcessInfoBuilder(applicationContext);
        }
    }
}