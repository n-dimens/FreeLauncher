using System;
using System.IO;
using System.Net;
using dotMCLauncher.Core;
using dotMCLauncher.YaDra4il;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FreeLauncher.Forms {
    public class LauncherFormPresenter {
        private readonly ILauncherLogger _logger;
        private readonly ApplicationContext _applicationContext;

        // TODO: Выпилить свойства, раскрывающие объекты
        public User SelectedUser { get; private set; }

        public UserManager UserManager { get; private set; }

        public LauncherFormPresenter(ILauncherLogger viewLogger, ApplicationContext applicationContext) {
            _logger = viewLogger;
            _applicationContext = applicationContext;
        }

        public void ReloadUserManager() {
            try {
                UserManager = File.Exists(_applicationContext.McLauncher + "users.json")
                    ? JsonConvert.DeserializeObject<UserManager>(File.ReadAllText(_applicationContext.McLauncher + "users.json"))
                    : new UserManager();
            }
            catch (Exception ex) {
                _logger.AppendException("Reading user list: an exception has occurred\n" + ex.Message);
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
                        _logger.AppendException("Session token is not valid. Please, head up to user manager and re-add your account.");
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

        public void SaveProfiles(ProfileManager manager) {
            File.WriteAllText(_applicationContext.McDirectory + "launcher_profiles.json", manager.ToJson());
        }

        public void SaveUsers() {
            File.WriteAllText(_applicationContext.McLauncher + "users.json",
                JsonConvert.SerializeObject(UserManager, Formatting.Indented,
                    new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore}));
        }

        /// <summary>
        /// Обновление локального файла versions.json при сравнении с файлом в облаке
        /// </summary>
        public void UpdateVersions() {
            _logger.AppendLog("Checking version.json...");
            if (!Directory.Exists(_applicationContext.McVersions)) {
                Directory.CreateDirectory(_applicationContext.McVersions);
            }
            
            // Скачиваем новый файл
            var jsonVersionList = new WebClient().DownloadString(new Uri(ApplicationContext.VersionsFileUrl));
            
            // Если локального файла не существует, сохраняем и выходим
            if (!File.Exists(_applicationContext.McVersionsFile)) {
                File.WriteAllText(_applicationContext.McVersionsFile, jsonVersionList);
                _logger.AppendLog("File downloaded and saved.");
                return;
            }

            // Если локальный файл существует, сравниваем со скаченным
            var newVersionsData = JObject.Parse(jsonVersionList);
            string remoteSnapshotVersion = newVersionsData["latest"]["snapshot"].ToString();
            string remoteReleaseVersion = newVersionsData["latest"]["release"].ToString();
            _logger.AppendLog("Latest snapshot: " + remoteSnapshotVersion);
            _logger.AppendLog("Latest release: " + remoteReleaseVersion);

            JObject ver = JObject.Parse(File.ReadAllText(_applicationContext.McVersionsFile));
            string localSnapshotVersion = ver["latest"]["snapshot"].ToString();
            string localReleaseVersion = ver["latest"]["release"].ToString();

            bool isVersionsCountEqual = ((JArray) newVersionsData["versions"]).Count == ((JArray) ver["versions"]).Count;
            bool isEqualVersions = remoteReleaseVersion == localReleaseVersion && remoteSnapshotVersion == localSnapshotVersion;
            _logger.AppendLog("Local versions: " + ((JArray) newVersionsData["versions"]).Count + ". Remote versions: " + ((JArray) ver["versions"]).Count);
            
            if (isVersionsCountEqual && isEqualVersions) {
                // Изменений нет, выходим
                _logger.AppendLog("No update found.");
                return;
            }

            // Найдены изменения, обновляем лоакльный файл
            _logger.AppendLog("Writting new list... ");
            File.WriteAllText(_applicationContext.McVersionsFile, jsonVersionList);
        }
    }
}