namespace NDimens.Minecraft.FreeLauncher.Presenters;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using dotMCLauncher.Core;
using Version = dotMCLauncher.Core.Version;

using Ionic.Zip;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using dotMCLauncher.Core.Data;
using NDimens.Minecraft.FreeLauncher.Core.Data;
using System.Collections.Generic;
using System.Collections.Immutable;
using NDimens.Minecraft.FreeLauncher.Core;

public class MainFormPresenter {
    private readonly static string CheckingLibrariesMessage = "Выполняется проверка библиотек";
    private readonly ILauncherLogger _logger;
    private readonly IUsersRepository _usersRepository;
    private readonly VersionsService _versionsService;
    private IProgressView _progressView;

    public static readonly string ProductName = "FreeLauncher";

    public GameFileStructure GameFiles { get; }

    public ProfileManager ProfileManager { get; private set; }

    public Profile SelectedProfile { get; private set; }

    public MainFormPresenter(ILauncherLogger viewLogger, GameFileStructure appContext, VersionsService versionsService, IUsersRepository usersRepository) {
        _logger = viewLogger;
        _usersRepository = usersRepository;
        _versionsService = versionsService;
        GameFiles = appContext;
    }

    public void SetProgressView(IProgressView progressView) {
        _progressView = progressView;
    }

    public UserManager GetUserManager() {
        return _usersRepository.Read();
    }

    public User GetUser(string userName) {
        return _usersRepository.Find(userName);
    }

    public void SetSelectedUser(string nickname) {
        var um = _usersRepository.Read();
        um.SelectedUsername = nickname;
        _usersRepository.Save(um);
    }

    public void SelectProfile(string name) {
        SelectedProfile = ProfileManager.Profiles[name];
        ProfileManager.LastUsedProfile = name;
    }

    // TODO: Отделить Restore от Reload
    public void ReloadProfileManager() {
        try {
            if (!File.Exists(GameFiles.McLauncherProfiles) && !File.Exists(GameFiles.LauncherProfiles)) {
                ProfileManager = ProfileManagerUtils.Init(GameFiles.LauncherProfiles);
                return;
            }

            if (File.Exists(GameFiles.McLauncherProfiles) && !File.Exists(GameFiles.LauncherProfiles)) {
                File.Copy(GameFiles.McLauncherProfiles, GameFiles.LauncherProfiles);
            }

            ProfileManager = LauncherExtensions.ParseProfile(GameFiles.LauncherProfiles);
            if (!ProfileManager.Profiles.Any()) {
                _logger.Error("Reading profile list: profiles missing. Creating default.");
                ProfileManager = ProfileManagerUtils.Init(GameFiles.LauncherProfiles);
            }
        }
        catch (Exception ex) {
            _logger.Error("Reading profile list: an exception has occurred\n" + ex.Message + "\nCreating a new one.");

            // save backup
            if (File.Exists(GameFiles.LauncherProfiles)) {
                string fileName = "launcher_profiles-" + DateTime.Now.ToString("hhmmss") + ".broken.json";
                _logger.Info("A copy of broken profiles file has been created: " + fileName);
                File.Move(GameFiles.LauncherProfiles, Path.Combine(GameFiles.McLauncher, fileName));
            }

            ProfileManager = ProfileManagerUtils.Init(GameFiles.LauncherProfiles);
        }
    }

    public void SaveProfiles() {
        ProfileManager.Save(GameFiles.LauncherProfiles);
    }

    public void CheckVersionAvailability() {
        long state = 0;
        var downloader = new WebClient();
        downloader.DownloadProgressChanged += (sender, e) => {
            _progressView.SetProgressValue(e.ProgressPercentage);
        };
        downloader.DownloadFileCompleted += delegate { state++; };
        _progressView.SetMaxProgressValue(100);
        _progressView.SetProgressValue(0);

        string version = SelectedProfile.SelectedVersion;
        var versionInfo = _versionsService.GetVersionInfo(version);
        _progressView.UpdateStageText($"Выполняется проверка доступности версии '{version}'");
        _logger.Info($"Checking '{version}' version availability...");

        string versionDirectory = Path.Combine(GameFiles.McVersions, version);
        if (!Directory.Exists(versionDirectory)) {
            Directory.CreateDirectory(versionDirectory);
        }

        var versionFile = Path.Combine(versionDirectory, version + ".json");
        if (!File.Exists(versionFile)) {
            _progressView.UpdateStageText("Downloading " + versionFile + "...", new StackFrame().GetMethod().Name);
            downloader.DownloadFileAsync(new Uri(versionInfo.Url), versionFile);
        }
        else {
            state++;
        }
        _progressView.SetProgressValue(0);
        while (state != 1) ;

        var selectedVersion = Version.ParseVersion(new DirectoryInfo(versionDirectory), false);
        if (!File.Exists(Path.Combine(versionDirectory, version + ".jar")) && selectedVersion.InheritsFrom == null) {
            string filename = version + ".jar";
            _progressView.UpdateStageText("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
            downloader.DownloadFileAsync(new Uri(selectedVersion.Downloads.Client.Url), string.Format("{0}/{1}/{1}.jar", GameFiles.McVersions, version));
        }
        else {
            state++;
        }

        while (state != 2) ;
        if (selectedVersion.InheritsFrom == null) {
            _logger.Info("Finished checking version avalability.");
            return;
        }

        // для Forge
        versionDirectory = Path.Combine(GameFiles.McVersions, selectedVersion.InheritsFrom + "\\");
        if (!Directory.Exists(versionDirectory)) {
            Directory.CreateDirectory(versionDirectory);
        }

        if (!File.Exists(string.Format("{0}/{1}/{1}.jar", GameFiles.McVersions, selectedVersion.InheritsFrom))) {
            string filename = selectedVersion.InheritsFrom + ".jar";
            _progressView.UpdateStageText("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
            downloader.DownloadFileAsync(new Uri(string.Format(
                    "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.jar", selectedVersion.InheritsFrom)),
                string.Format("{0}/{1}/{1}.jar", GameFiles.McVersions, selectedVersion.InheritsFrom));
        }
        else {
            state++;
        }

        while (state != 3) ;
        if (!File.Exists(string.Format("{0}/{1}/{1}.json", GameFiles.McVersions, selectedVersion.InheritsFrom))) {
            string filename = selectedVersion.InheritsFrom + ".json";
            _progressView.UpdateStageText("Downloading " + filename + "...");
            downloader.DownloadFileAsync(new Uri(string.Format(
                    "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.json", selectedVersion.InheritsFrom)),
                string.Format("{0}/{1}/{1}.json", GameFiles.McVersions, selectedVersion.InheritsFrom));
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
            new DirectoryInfo(GameFiles.McVersions + SelectedProfile.SelectedVersion));
        _progressView.SetProgressValue(0);
        _progressView.SetMaxProgressValue(selectedVersion.Libraries.Count(a => a.IsForWindows()) + 1);
        _progressView.UpdateStageText(CheckingLibrariesMessage);

        _logger.Info("Checking libraries...");
        foreach (Lib lib in selectedVersion.Libraries.Where(a => a.IsForWindows())) {
            _progressView.IncProgressValue();
            if (!File.Exists(GameFiles.McLibs + lib.ToPath())) {
                _progressView.UpdateStageText("Downloading " + lib.Name + "...");
                _logger.Debug("Url: " + (lib.Url ?? @"https://libraries.minecraft.net/") + lib.ToPath());
                string directory = Path.GetDirectoryName(GameFiles.McLibs + lib.ToPath());
                if (!File.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }

                new WebClient().DownloadFile((lib.Url ?? @"https://libraries.minecraft.net/") + lib.ToPath(),
                    GameFiles.McLibs + lib.ToPath());
            }

            if (lib.IsNative != null) {
                _progressView.UpdateStageText("Unpacking " + lib.Name + "...");
                using (ZipFile zip = ZipFile.Read(GameFiles.McLibs + lib.ToPath())) {
                    foreach (ZipEntry entry in zip.Where(entry => entry.FileName.EndsWith(".dll"))) {
                        _logger.Debug($"Unzipping {entry.FileName}");
                        try {
                            entry.Extract(GameFiles.McNatives, ExtractExistingFileAction.OverwriteSilently);
                        }
                        catch (Exception ex) {
                            _logger.Error(ex.Message);
                        }
                    }
                }
            }
            else {
                libraries += GameFiles.McLibs + lib.ToPath() + ";";
            }

            _progressView.UpdateStageText(CheckingLibrariesMessage);
        }

        libraries += string.Format("{0}{1}\\{1}.jar", GameFiles.McVersions,
            selectedVersion.InheritsFrom ?? SelectedProfile.SelectedVersion);
        GameFiles.Libraries = libraries;
        _logger.Info("Finished checking libraries.");
    }

    public void CheckGameResources() {
        _progressView.UpdateStageText("Checking game assets...");
        Version selectedVersion = Version.ParseVersion(
            new DirectoryInfo(GameFiles.McVersions + SelectedProfile.SelectedVersion));
        string file = string.Format("{0}/indexes/{1}.json", GameFiles.McAssets, selectedVersion.Assets ?? "legacy");
        if (!File.Exists(file)) {
            if (!Directory.Exists(Path.GetDirectoryName(file))) {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            }

            new WebClient().DownloadFile(selectedVersion.AssetIndex.Url, file);
        }

        JObject jo = JObject.Parse(File.ReadAllText(file));
        var something = jo["objects"].Cast<JProperty>()
            .Select(peep => jo["objects"][peep.Name]["hash"].ToString())
            .Select(c => c[0].ToString() + c[1].ToString() + "\\" + c)
            .Where(filename => !File.Exists(GameFiles.McObjectsAssets + filename)).ToList();
        _progressView.SetProgressValue(0);
        _progressView.SetMaxProgressValue(something.Count + 1);
        foreach (string resourseFile in something) {
            string path = GameFiles.McObjectsAssets + resourseFile[0] + resourseFile[1] + "\\";
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            try {
                _logger.Debug("Downloading " + resourseFile + "...");
                new WebClient().DownloadFile(@"http://resources.download.minecraft.net/" + resourseFile,
                    GameFiles.McObjectsAssets + resourseFile);
            }
            catch (Exception ex) {
                _logger.Error(ex.ToString());
            }

            _progressView.IncProgressValue();
        }

        _logger.Info("Finished checking game assets.");
        if (selectedVersion.Assets == null) {
            _progressView.SetProgressValue(0);
            _progressView.SetMaxProgressValue(jo["objects"].Cast<JProperty>()
                .Count(res => !File.Exists(GameFiles.McLegacyAssets + res.Name)) + 1);
            _progressView.UpdateStageText("Converting assets...");
            foreach (JProperty res in jo["objects"].Cast<JProperty>()
                         .Where(res => !File.Exists(GameFiles.McLegacyAssets + res.Name))) {
                try {
                    FileInfo resFile = new FileInfo(GameFiles.McLegacyAssets + res.Name);
                    if (!resFile.Directory.Exists) {
                        resFile.Directory.Create();
                    }

                    _logger.Debug(
                        $"Converting \"{"\\assets\\objects\\" + res.Value["hash"].ToString()[0] + res.Value["hash"].ToString()[1] + "\\" + res.Value["hash"]}\" to \"{"\\assets\\legacy\\" + res.Name}\"");
                    File.Copy(GameFiles.McObjectsAssets + res.Value["hash"].ToString()[0] +
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

    public string GetVersionLabel() {
        var selectedVersion = SelectedProfile.SelectedVersion;
        string versionFilePath = Path.Combine(GameFiles.McVersions, selectedVersion, $"{selectedVersion}.json");
        if (!File.Exists(versionFilePath)) {
            return $"Готов к загрузке версии {selectedVersion}";
        }

        return $"Готов к запуску версии {selectedVersion}";
    }
}