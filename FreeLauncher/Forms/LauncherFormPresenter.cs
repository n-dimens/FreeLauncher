using System;
using System.IO;
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

        public UserManager UserManager { get; private set;  }

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
    }
}