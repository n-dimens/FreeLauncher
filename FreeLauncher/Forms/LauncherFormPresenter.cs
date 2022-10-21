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
        public static readonly string ProductName = "FreeLauncher";
        private readonly ILauncherLogger _logger;
        private readonly ApplicationContext _applicationContext;

        // TODO: Выпилить свойства, раскрывающие объекты
        public User SelectedUser { get; private set; }

        public UserManager UserManager { get; private set; }
        
        public ProfileManager ProfileManager { get; private set; }

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

        public void ReloadProfileManager() {
            try {
                ProfileManager = LauncherExtensions.ParseProfile(_applicationContext.McDirectory + "/launcher_profiles.json");
                if (!ProfileManager.Profiles.Any()) {
                    throw new Exception("Someone broke my profiles>:(");
                }
            }
            catch (Exception ex) {
                _logger.AppendException("Reading profile list: an exception has occurred\n" + ex.Message + "\nCreating a new one.");

                // save backup
                if (File.Exists(_applicationContext.LauncherProfiles)) {
                    string fileName = "launcher_profiles-" + DateTime.Now.ToString("hhmmss") + ".bak.json";
                    _logger.AppendLog("A copy of old profile list has been created: " + fileName);
                    File.Move(_applicationContext.LauncherProfiles, _applicationContext.McDirectory + "/" + fileName);
                }

                // write default content file
                File.WriteAllText(_applicationContext.LauncherProfiles, new JObject {
                    {
                        "profiles", new JObject {
                            {
                                ProductName, new JObject {
                                    {"name", ProductName}, {
                                        "allowedReleaseTypes", new JArray {
                                            "release",
                                            "other"
                                        }
                                    },
                                    {"launcherVisibilityOnGameClose", "keep the launcher open"}
                                }
                            }
                        }
                    },
                    {"selectedProfile", ProductName}
                }.ToString());

                ProfileManager = LauncherExtensions.ParseProfile(_applicationContext.LauncherProfiles);
                SaveProfiles();
            }
        }

        public void SaveProfiles() {
            File.WriteAllText(_applicationContext.McDirectory + "launcher_profiles.json", ProfileManager.ToJson());
        }

        public void SaveUsers() {
            File.WriteAllText(_applicationContext.McLauncher + "users.json",
                JsonConvert.SerializeObject(UserManager, Formatting.Indented,
                    new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore}));
        }

        /// <summary>
        /// Обновление локального файла versions.json при сравнении с файлом в облаке
        /// </summary>
        public void UpdateVersionsList() {
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