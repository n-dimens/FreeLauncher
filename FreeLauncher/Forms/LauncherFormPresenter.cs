using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using dotMCLauncher.Core;
using Version = dotMCLauncher.Core.Version;
using Core = dotMCLauncher.Core;

using Ionic.Zip;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FreeLauncher.Forms {
    public class MainFormPresenter {
        private readonly static string CheckingLibrariesMessage = "Выполняется проверка библиотек";
        // TODO: Выпилить свойства, раскрывающие объекты
        private readonly ILauncherLogger _logger;
        private readonly IProgressView _progressView;
        private readonly UsersRepository _usersRepository;

        public static readonly string ProductName = "FreeLauncher";

        public Core.GameFileStructure AppContext { get; }

        public User SelectedUser { get; private set; }

        public UserManager UserManager { get; private set; }

        public ProfileManager ProfileManager { get; private set; }

        public Profile SelectedProfile { get; private set; }

        public MainFormPresenter(ILauncherLogger viewLogger, IProgressView progressView, Core.GameFileStructure appContext) {
            _logger = viewLogger;
            _progressView = progressView;
            _usersRepository = new UsersRepository(appContext);
            AppContext = appContext;
        }

        public void ReloadUserManager() {
            try {
                UserManager = _usersRepository.Read();
            }
            catch (Exception ex) {
                _logger.Error("Reading user list: an exception has occurred\n" + ex.Message);
                UserManager = new UserManager();
                _usersRepository.Save(UserManager);
            }
        }

        public void SelectUser(string nickname) {
            SelectedUser = UserManager.Accounts[nickname];
            UserManager.SelectedUsername = nickname;
        }

        public void SelectProfile(string name) {
            SelectedProfile = ProfileManager.Profiles[name];
            ProfileManager.LastUsedProfile = name;
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
                    _logger.Error("Reading profile list: profiles missing. Creating default.");
                    ProfileManager = ProfileManagerUtils.Init(AppContext.LauncherProfiles);
                }
            }
            catch (Exception ex) {
                _logger.Error("Reading profile list: an exception has occurred\n" + ex.Message + "\nCreating a new one.");

                // save backup
                if (File.Exists(AppContext.LauncherProfiles)) {
                    string fileName = "launcher_profiles-" + DateTime.Now.ToString("hhmmss") + ".broken.json";
                    _logger.Info("A copy of broken profiles file has been created: " + fileName);
                    File.Move(AppContext.LauncherProfiles, Path.Combine(AppContext.McLauncher, fileName));
                }

                ProfileManager = ProfileManagerUtils.Init(AppContext.LauncherProfiles);
            }
        }

        public void SaveProfiles() {
            ProfileManager.Save(AppContext.LauncherProfiles);
        }

        /// <summary>
        /// Обновление локального файла versions.json при сравнении с файлом в облаке
        /// </summary>
        public void UpdateVersionsList() {
            _logger.Info("Checking version.json...");
            if (!Directory.Exists(AppContext.McVersions)) {
                Directory.CreateDirectory(AppContext.McVersions);
            }

            // Скачиваем новый файл
            var jsonVersionList = new WebClient().DownloadString(new Uri(GameFileStructure.VersionsFileUrl));

            // Если локального файла не существует, сохраняем и выходим
            if (!File.Exists(AppContext.McVersionsFile)) {
                File.WriteAllText(AppContext.McVersionsFile, jsonVersionList);
                _logger.Info("File downloaded and saved.");
                return;
            }

            // Если локальный файл существует, сравниваем со скаченным
            var newVersionsData = JObject.Parse(jsonVersionList);
            string remoteSnapshotVersion = newVersionsData["latest"]["snapshot"].ToString();
            string remoteReleaseVersion = newVersionsData["latest"]["release"].ToString();
            _logger.Info("Latest snapshot: " + remoteSnapshotVersion);
            _logger.Info("Latest release: " + remoteReleaseVersion);

            JObject ver = JObject.Parse(File.ReadAllText(AppContext.McVersionsFile));
            string localSnapshotVersion = ver["latest"]["snapshot"].ToString();
            string localReleaseVersion = ver["latest"]["release"].ToString();

            bool isVersionsCountEqual = ((JArray)newVersionsData["versions"]).Count == ((JArray)ver["versions"]).Count;
            bool isEqualVersions = remoteReleaseVersion == localReleaseVersion && remoteSnapshotVersion == localSnapshotVersion;
            _logger.Info("Local versions: " + ((JArray)newVersionsData["versions"]).Count + ". Remote versions: " + ((JArray)ver["versions"]).Count);

            if (isVersionsCountEqual && isEqualVersions) {
                // Изменений нет, выходим
                _logger.Info("No update found.");
                return;
            }

            // Найдены изменения, обновляем лоакльный файл
            _logger.Info("Writting new list... ");
            File.WriteAllText(AppContext.McVersionsFile, jsonVersionList);
        }

        public void CheckVersionAvailability() {
            long state = 0;
            WebClient downloader = new WebClient();
            downloader.DownloadProgressChanged += (sender, e) => {
                _progressView.SetProgressValue(e.ProgressPercentage);
            };
            downloader.DownloadFileCompleted += delegate { state++; };
            _progressView.SetMaxProgressValue(100);
            _progressView.SetProgressValue(0);
            string version = SelectedProfile.GetSelectedVersion(AppContext);
            _progressView.UpdateStageText($"Выполняется проверка доступности версии '{version}'");
            _logger.Info($"Checking '{version}' version availability...");
            string path = Path.Combine(AppContext.McVersions, version + "\\");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(path + "/" + version + ".json")) {
                string filename = version + ".json";
                _progressView.UpdateStageText("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.json", version)),
                    string.Format("{0}/{1}/{1}.json", AppContext.McVersions, version));
            }
            else {
                state++;
            }

            _progressView.SetProgressValue(0);
            while (state != 1) ;
            var selectedVersion = dotMCLauncher.Core.Version.ParseVersion(new DirectoryInfo(AppContext.McVersions + version), false);
            if ((!File.Exists(path + "/" + version + ".jar")) &&
                selectedVersion.InheritsFrom == null) {
                string filename = version + ".jar";
                _progressView.UpdateStageText("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.jar", version)),
                    string.Format("{0}/{1}/{1}.jar", AppContext.McVersions, version));
            }
            else {
                state++;
            }

            while (state != 2) ;
            if (selectedVersion.InheritsFrom == null) {
                _logger.Info("Finished checking version avalability.");
                return;
            }

            path = Path.Combine(AppContext.McVersions, selectedVersion.InheritsFrom + "\\");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(string.Format("{0}/{1}/{1}.jar", AppContext.McVersions, selectedVersion.InheritsFrom))) {
                string filename = selectedVersion.InheritsFrom + ".jar";
                _progressView.UpdateStageText("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.jar", selectedVersion.InheritsFrom)),
                    string.Format("{0}/{1}/{1}.jar", AppContext.McVersions, selectedVersion.InheritsFrom));
            }
            else {
                state++;
            }

            while (state != 3) ;
            if (!File.Exists(string.Format("{0}/{1}/{1}.json", AppContext.McVersions, selectedVersion.InheritsFrom))) {
                string filename = selectedVersion.InheritsFrom + ".json";
                _progressView.UpdateStageText("Downloading " + filename + "...");
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.json", selectedVersion.InheritsFrom)),
                    string.Format("{0}/{1}/{1}.json", AppContext.McVersions, selectedVersion.InheritsFrom));
            }
            else {
                state++;
            }

            while (state != 4) ;
            _logger.Info("Finished checking version avalability.");
        }

        public void CheckLibraries() {
            string libraries = string.Empty;
            var selectedVersion = Version.ParseVersion(
                new DirectoryInfo(AppContext.McVersions + SelectedProfile.GetSelectedVersion(AppContext)));
            _progressView.SetProgressValue(0);
            _progressView.SetMaxProgressValue(selectedVersion.Libs.Count(a => a.IsForWindows()) + 1);
            _progressView.UpdateStageText(CheckingLibrariesMessage);
            _logger.Info("Checking libraries...");
            foreach (Lib lib in selectedVersion.Libs.Where(a => a.IsForWindows())) {
                _progressView.IncProgressValue();
                if (!File.Exists(AppContext.McLibs + lib.ToPath())) {
                    _progressView.UpdateStageText("Downloading " + lib.Name + "...");
                    _logger.Debug("Url: " + (lib.Url ?? @"https://libraries.minecraft.net/") + lib.ToPath());
                    string directory = Path.GetDirectoryName(AppContext.McLibs + lib.ToPath());
                    if (!File.Exists(directory)) {
                        Directory.CreateDirectory(directory);
                    }

                    new WebClient().DownloadFile((lib.Url ?? @"https://libraries.minecraft.net/") + lib.ToPath(),
                        AppContext.McLibs + lib.ToPath());
                }

                if (lib.IsNative != null) {
                    _progressView.UpdateStageText("Unpacking " + lib.Name + "...");
                    using (ZipFile zip = ZipFile.Read(AppContext.McLibs + lib.ToPath())) {
                        foreach (ZipEntry entry in zip.Where(entry => entry.FileName.EndsWith(".dll"))) {
                            _logger.Debug($"Unzipping {entry.FileName}");
                            try {
                                entry.Extract(AppContext.McNatives, ExtractExistingFileAction.OverwriteSilently);
                            }
                            catch (Exception ex) {
                                _logger.Error(ex.Message);
                            }
                        }
                    }
                }
                else {
                    libraries += AppContext.McLibs + lib.ToPath() + ";";
                }

                _progressView.UpdateStageText(CheckingLibrariesMessage);
            }

            libraries += string.Format("{0}{1}\\{1}.jar", AppContext.McVersions,
                selectedVersion.InheritsFrom ?? SelectedProfile.GetSelectedVersion(AppContext));
            AppContext.Libraries = libraries;
            _logger.Info("Finished checking libraries.");
        }

        public void CheckGameResources() {
            _progressView.UpdateStageText("Checking game assets...");
            Version selectedVersion = Version.ParseVersion(
                new DirectoryInfo(AppContext.McVersions + SelectedProfile.GetSelectedVersion(AppContext)));
            string file = string.Format("{0}/indexes/{1}.json", AppContext.McAssets, selectedVersion.AssetsIndex ?? "legacy");
            if (!File.Exists(file)) {
                if (!Directory.Exists(Path.GetDirectoryName(file))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(file));
                }

                new WebClient().DownloadFile(
                    string.Format("https://s3.amazonaws.com/Minecraft.Download/indexes/{0}.json",
                        selectedVersion.AssetsIndex ?? "legacy"), file);
            }

            JObject jo = JObject.Parse(File.ReadAllText(file));
            var something = jo["objects"].Cast<JProperty>()
                .Select(peep => jo["objects"][peep.Name]["hash"].ToString())
                .Select(c => c[0].ToString() + c[1].ToString() + "\\" + c)
                .Where(filename => !File.Exists(AppContext.McObjectsAssets + filename)).ToList();
            _progressView.SetProgressValue(0);
            _progressView.SetMaxProgressValue(something.Count + 1);
            foreach (string resourseFile in something) {
                string path = AppContext.McObjectsAssets + resourseFile[0] + resourseFile[1] + "\\";
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                try {
                    _logger.Debug("Downloading " + resourseFile + "...");
                    new WebClient().DownloadFile(@"http://resources.download.minecraft.net/" + resourseFile,
                        AppContext.McObjectsAssets + resourseFile);
                }
                catch (Exception ex) {
                    _logger.Error(ex.ToString());
                }

                _progressView.IncProgressValue();
            }

            _logger.Info("Finished checking game assets.");
            if (selectedVersion.AssetsIndex == null) {
                _progressView.SetProgressValue(0);
                _progressView.SetMaxProgressValue(jo["objects"].Cast<JProperty>()
                    .Count(res => !File.Exists(AppContext.McLegacyAssets + res.Name)) + 1);
                _progressView.UpdateStageText("Converting assets...");
                foreach (JProperty res in jo["objects"].Cast<JProperty>()
                             .Where(res => !File.Exists(AppContext.McLegacyAssets + res.Name))) {
                    try {
                        FileInfo resFile = new FileInfo(AppContext.McLegacyAssets + res.Name);
                        if (!resFile.Directory.Exists) {
                            resFile.Directory.Create();
                        }

                        _logger.Debug(
                            $"Converting \"{"\\assets\\objects\\" + res.Value["hash"].ToString()[0] + res.Value["hash"].ToString()[1] + "\\" + res.Value["hash"]}\" to \"{"\\assets\\legacy\\" + res.Name}\"");
                        File.Copy(AppContext.McObjectsAssets + res.Value["hash"].ToString()[0] +
                                  res.Value["hash"].ToString()[1] + "\\" + res.Value["hash"],
                            resFile.FullName);
                    }
                    catch (Exception ex) {
                        _logger.Error(ex.ToString());
                    }

                    _progressView.IncProgressValue();
                }

                _logger.Info("Finished converting assets.");
            }
        }

        //public void LogInfo(string text, string methodName = null) {
        //    _logger.LogInfo(text, methodName);
        //}

        public string GetVersionLabel() {
            var selectedVersion = SelectedProfile.GetSelectedVersion(AppContext);
            string versionFilePath = Path.Combine(AppContext.McVersions, selectedVersion, $"{selectedVersion}.json");
            if (!File.Exists(versionFilePath)) {
                return $"Готов к загрузке версии {selectedVersion}";
            }

            return $"Готов к запуску версии {selectedVersion}";
        }
    }
}