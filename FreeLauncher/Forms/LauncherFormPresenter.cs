using System;
using System.IO;
using System.Linq;
using System.Net;
using dotMCLauncher.Core;
using dotMCLauncher.YaDra4il;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FreeLauncher.Forms {
    public class LauncherFormPresenter {
        private readonly ILauncherLogger _logger;

        public static readonly string ProductName = "FreeLauncher";

        public ApplicationContext AppContext { get; }

        // TODO: Выпилить свойства, раскрывающие объекты
        public User SelectedUser { get; private set; }

        public UserManager UserManager { get; private set; }
        
        public ProfileManager ProfileManager { get; private set; }

        public LauncherFormPresenter(ILauncherLogger viewLogger, ApplicationContext applicationContext) {
            _logger = viewLogger;
            AppContext = applicationContext;
        }

        public void ReloadUserManager() {
            try {
                UserManager = File.Exists(AppContext.McLauncher + "users.json")
                    ? JsonConvert.DeserializeObject<UserManager>(File.ReadAllText(AppContext.McLauncher + "users.json"))
                    : new UserManager();
            }
            catch (Exception ex) {
                LogError("Reading user list: an exception has occurred\n" + ex.Message);
                UserManager = new UserManager();
                SaveUsers();
            }
        }

        public void SelectUser(string nickname) {
            SelectedUser = UserManager.Accounts[nickname];
            UserManager.SelectedUsername = nickname;
        }

        public void SelectUserForLaunch(string nickname) {
            if (!UserManager.Accounts.ContainsKey(nickname)) {
                User user = new User {
                    Username = nickname,
                    Type = "offline"
                };
                UserManager.Accounts.Add(user.Username, user);
                SelectedUser = user;
            }
            else {
                SelectedUser = UserManager.Accounts[nickname];
                if (SelectedUser.Type != "offline") {
                    AuthManager am = new AuthManager {
                        SessionToken = SelectedUser.SessionToken,
                        Uuid = SelectedUser.Uuid
                    };
                    bool check = am.CheckSessionToken();
                    if (!check) {
                        LogError("Session token is not valid. Please, head up to user manager and re-add your account.");
                        User user = new User {
                            Username = nickname,
                            Type = "offline"
                        };
                        SelectedUser = user;
                    }
                    else {
                        Refresh refresh = new Refresh(SelectedUser.SessionToken, SelectedUser.AccessToken);
                        SelectedUser.UserProperties = (JArray) refresh.user["properties"];
                        SelectedUser.SessionToken = refresh.accessToken;
                        UserManager.Accounts[nickname] = SelectedUser;
                    }
                }
            }

            UserManager.SelectedUsername = SelectedUser.Username;
        }

        // TODO: Отделить Restore от Reload
        public void ReloadProfileManager() {
            try {
                if (!File.Exists(AppContext.McLauncherProfiles) && !File.Exists(AppContext.LauncherProfiles)) {
                    ProfileManager = ProfileManagerUtils.Init(AppContext.LauncherProfiles);
                    return;
                } 
                
                if (File.Exists(AppContext.McLauncherProfiles) && !File.Exists(AppContext.LauncherProfiles)) {
                    File.Copy(AppContext.McLauncherProfiles, AppContext.LauncherProfiles);
                }

                ProfileManager = LauncherExtensions.ParseProfile(AppContext.LauncherProfiles);
                if (!ProfileManager.Profiles.Any()) {
                    LogError("Reading profile list: profiles missing. Creating default.");
                    ProfileManager = ProfileManagerUtils.Init(AppContext.LauncherProfiles);
                }
            }
            catch (Exception ex) {
                LogError("Reading profile list: an exception has occurred\n" + ex.Message + "\nCreating a new one.");

                // save backup
                if (File.Exists(AppContext.LauncherProfiles)) {
                    string fileName = "launcher_profiles-" + DateTime.Now.ToString("hhmmss") + ".broken.json";
                    LogInfo("A copy of broken profiles file has been created: " + fileName);
                    File.Move(AppContext.LauncherProfiles, Path.Combine(AppContext.McLauncher, fileName));
                }

                ProfileManager = ProfileManagerUtils.Init(AppContext.LauncherProfiles);
            }
        }

        public void SaveProfiles() {
            ProfileManager.Save(AppContext.LauncherProfiles);
        }

        public void SaveUsers() {
            File.WriteAllText(AppContext.McLauncher + "users.json",
                JsonConvert.SerializeObject(UserManager, Formatting.Indented,
                    new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore}));
        }

        /// <summary>
        /// Обновление локального файла versions.json при сравнении с файлом в облаке
        /// </summary>
        public void UpdateVersionsList() {
            LogInfo("Checking version.json...");
            if (!Directory.Exists(AppContext.McVersions)) {
                Directory.CreateDirectory(AppContext.McVersions);
            }
            
            // Скачиваем новый файл
            var jsonVersionList = new WebClient().DownloadString(new Uri(ApplicationContext.VersionsFileUrl));
            
            // Если локального файла не существует, сохраняем и выходим
            if (!File.Exists(AppContext.McVersionsFile)) {
                File.WriteAllText(AppContext.McVersionsFile, jsonVersionList);
                LogInfo("File downloaded and saved.");
                return;
            }

            // Если локальный файл существует, сравниваем со скаченным
            var newVersionsData = JObject.Parse(jsonVersionList);
            string remoteSnapshotVersion = newVersionsData["latest"]["snapshot"].ToString();
            string remoteReleaseVersion = newVersionsData["latest"]["release"].ToString();
            LogInfo("Latest snapshot: " + remoteSnapshotVersion);
            LogInfo("Latest release: " + remoteReleaseVersion);

            JObject ver = JObject.Parse(File.ReadAllText(AppContext.McVersionsFile));
            string localSnapshotVersion = ver["latest"]["snapshot"].ToString();
            string localReleaseVersion = ver["latest"]["release"].ToString();

            bool isVersionsCountEqual = ((JArray) newVersionsData["versions"]).Count == ((JArray) ver["versions"]).Count;
            bool isEqualVersions = remoteReleaseVersion == localReleaseVersion && remoteSnapshotVersion == localSnapshotVersion;
            LogInfo("Local versions: " + ((JArray) newVersionsData["versions"]).Count + ". Remote versions: " + ((JArray) ver["versions"]).Count);
            
            if (isVersionsCountEqual && isEqualVersions) {
                // Изменений нет, выходим
                LogInfo("No update found.");
                return;
            }

            // Найдены изменения, обновляем лоакльный файл
            LogInfo("Writting new list... ");
            File.WriteAllText(AppContext.McVersionsFile, jsonVersionList);
        }

        public void LogDebug(string text, string methodName = null) {
            _logger.LogDebug(text, methodName);
        }

        public void LogError(string text, string methodName = null) {
            _logger.LogError(text, methodName);
        }

        public void LogInfo(string text, string methodName = null) {
            _logger.LogInfo(text, methodName);
        }
    }
}