using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using dotMCLauncher.Core;
using Ionic.Zip;
using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json.Linq;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Data;
using Version = dotMCLauncher.Core.Version;

namespace FreeLauncher.Forms {
    public partial class LauncherForm : RadForm, ILauncherLogger {
        private readonly LauncherFormPresenter _presenter;
        private readonly ApplicationContext _applicationContext;
        private ProfileManager _profileManager;
        private Profile _selectedProfile;
        private readonly Configuration _cfg;
        private string _versionToLaunch;
        private bool _restoreVersion;

        private int StatusBarMaxValue {
            set => SetStatusBarMaxValue(value);
        }

        public string VersionToLaunch => _versionToLaunch;

        private void DisableControls() {
            BlockControls = true;
        }
        
        private void EnableControls() {
            BlockControls = false;
        }
        
        private bool BlockControls {
            set {
                LaunchButton.Enabled = !value;
                profilesDropDownBox.Enabled = !value;
                DeleteProfileButton.Enabled = !value && (_profileManager.Profiles.Count > 1);
                EditProfile.Enabled = !value;
                AddProfile.Enabled = !value;
                NicknameDropDownList.Enabled = !value;
            }
        }

        private void IncStatusBarValue() {
            SetStatusBarValue(StatusBar.Value1 + 1);
        }

        private void SetStatusBarValue(int i) {
            if (logBox.InvokeRequired) {
                logBox.Invoke(new Action<int>(SetStatusBarValue), i);
            }
            else {
                StatusBar.Value1 = i;
            }
        }

        private void SetStatusBarMaxValue(int value) {
            if (logBox.InvokeRequired) {
                logBox.Invoke(new Action<int>(SetStatusBarMaxValue), value);
            }
            else {
                StatusBar.Maximum = value;
            }
        }
        
        public LauncherForm(ApplicationContext appContext) {
            _presenter = new LauncherFormPresenter(this, appContext);
            _applicationContext = appContext;
            InitializeComponent();
            // Loading configuration
            _cfg = _applicationContext.Configuration;
            EnableMinecraftLogging.Checked = _cfg.EnableGameLogging;
            UseGamePrefix.Checked = _cfg.ShowGamePrefix;
            CloseGameOutput.Checked = _cfg.CloseTabAfterSuccessfulExitCode;
            LoadLocalization();
            //
            Text = ProductName + " " + ProductVersion;
            AboutVersion.Text = ProductVersion;
            AppendLog($"Application: {ProductName} v.{ProductVersion}" +
                      (!_applicationContext.ProgramArguments.NotAStandalone ? "-standalone" : string.Empty));
            AppendLog($"Application language: {_applicationContext.ProgramLocalization.Name}({_applicationContext.ProgramLocalization.LanguageTag})");
            AppendLog("==============");
            AppendLog("System info:");
            AppendLog($"Operating system: {Environment.OSVersion}({new ComputerInfo().OSFullName})");
            AppendLog($"Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}");
            AppendLog($"Java path: \"{Java.JavaInstallationPath}\" ({Java.JavaBitInstallation}-bit)");
            AppendLog("==============");
            if (_applicationContext.LocalizationsList.Count != 0) {
                foreach (KeyValuePair<string, Localization> keyvalue in _applicationContext.LocalizationsList) {
                    LangDropDownList.Items.Add(new RadListDataItem {
                        Text = $"{keyvalue.Value.Name} ({keyvalue.Key})",
                        Tag = keyvalue.Key
                    });
                }

                foreach (RadListDataItem item in LangDropDownList.Items.Where(a => a.Tag.ToString() == _cfg.SelectedLanguage)) {
                    LangDropDownList.SelectedItem = item;
                }
            }
            else {
                LangDropDownList.Enabled = false;
            }

            if (!Directory.Exists(_applicationContext.McDirectory)) {
                Directory.CreateDirectory(_applicationContext.McDirectory);
            }

            if (!Directory.Exists(_applicationContext.McLauncher)) {
                Directory.CreateDirectory(_applicationContext.McLauncher);
            }

            Focus();
            if (!_applicationContext.ProgramArguments.NotAStandalone) {
                UpdateVersions();
            }

            UpdateProfileList();
            UpdateVersionListView();
            UpdateUserList();
        }

        private void LauncherForm_FormClosing(object sender, FormClosingEventArgs e) {
            _cfg.EnableGameLogging = EnableMinecraftLogging.Checked;
            _cfg.ShowGamePrefix = UseGamePrefix.Checked;
            _cfg.CloseTabAfterSuccessfulExitCode = CloseGameOutput.Checked;
        }

        private void profilesDropDownBox_SelectedIndexChanged(object sender,
            PositionChangedEventArgs e) {
            if (profilesDropDownBox.SelectedItem == null) {
                return;
            }

            _profileManager.LastUsedProfile = profilesDropDownBox.SelectedItem.Text;
            _selectedProfile = _profileManager.Profiles[profilesDropDownBox.SelectedItem.Text];
            string path = Path.Combine(_applicationContext.McVersions, _selectedProfile.GetSelectedVersion(_applicationContext) + "\\");
            string state = _applicationContext.ProgramLocalization.ReadyToLaunch;
            if (!File.Exists(string.Format("{0}/{1}.json", path, _selectedProfile.GetSelectedVersion(_applicationContext)))) {
                state = _applicationContext.ProgramLocalization.ReadyToDownload;
            }

            SelectedVersion.Text = string.Format(state, _selectedProfile.GetSelectedVersion(_applicationContext));
        }

        private void NicknameDropDownList_SelectedIndexChanged(object sender, PositionChangedEventArgs e) {
            if (NicknameDropDownList.SelectedItem == null) {
                return;
            }

            _presenter.SelectUser(NicknameDropDownList.SelectedItem.Text);
            _presenter.SaveUsers();
        }

        private void EditProfile_Click(object sender, EventArgs e) {
            ProfileForm pf = new ProfileForm(_selectedProfile, _applicationContext) {
                Text = _applicationContext.ProgramLocalization.EditingProfileTitle
            };
            pf.ShowDialog();
            if (pf.DialogResult == DialogResult.OK) {
                _profileManager.Profiles.Remove(_profileManager.LastUsedProfile);
                if (_profileManager.Profiles.ContainsKey(pf.CurrentProfile.ProfileName)) {
                    RadMessageBox.Show(_applicationContext.ProgramLocalization.ProfileAlreadyExistsErrorText,
                        _applicationContext.ProgramLocalization.Error,
                        MessageBoxButtons.OK, RadMessageIcon.Error);
                    UpdateProfileList();
                    return;
                }

                _profileManager.Profiles.Add(pf.CurrentProfile.ProfileName, pf.CurrentProfile);
                _profileManager.LastUsedProfile = pf.CurrentProfile.ProfileName;
            }

            _presenter.SaveProfiles(_profileManager);
            UpdateProfileList();
        }

        private void AddProfile_Click(object sender, EventArgs e) {
            Profile editedProfile = Profile.ParseProfile(_selectedProfile.ToString());
            editedProfile.ProfileName = "Copy of '" + _selectedProfile.ProfileName + "'(" +
                                        DateTime.Now.ToString("HH:mm:ss") + ")";
            ProfileForm pf = new ProfileForm(editedProfile, _applicationContext) {Text = _applicationContext.ProgramLocalization.AddingProfileTitle};
            pf.ShowDialog();
            if (pf.DialogResult == DialogResult.OK) {
                if (_profileManager.Profiles.ContainsKey(editedProfile.ProfileName)) {
                    RadMessageBox.Show(_applicationContext.ProgramLocalization.ProfileAlreadyExistsErrorText,
                        _applicationContext.ProgramLocalization.Error,
                        MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                _profileManager.Profiles.Add(editedProfile.ProfileName, editedProfile);
                _profileManager.LastUsedProfile = pf.CurrentProfile.ProfileName;
            }

            _presenter.SaveProfiles(_profileManager);
            UpdateProfileList();
        }

        private void DeleteProfileButton_Click(object sender, EventArgs e) {
            DialogResult dr =
                RadMessageBox.Show(
                    string.Format(_applicationContext.ProgramLocalization.ProfileDeleteConfirmationText,
                        _profileManager.LastUsedProfile), _applicationContext.ProgramLocalization.DeleteConfirmationTitle,
                    MessageBoxButtons.YesNo, RadMessageIcon.Question);
            if (dr != DialogResult.Yes) {
                return;
            }

            _profileManager.Profiles.Remove(_profileManager.LastUsedProfile);
            _profileManager.LastUsedProfile = _profileManager.Profiles.FirstOrDefault().Key;
            _presenter.SaveProfiles(_profileManager);
            UpdateProfileList();
        }

        private void ManageUsersButton_Click(object sender, EventArgs e) {
            new UsersForm(_applicationContext).ShowDialog();
            UpdateUserList();
        }

        private void LaunchButton_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(NicknameDropDownList.Text)) {
                NicknameDropDownList.Text = $"Player{DateTime.Now:hhmmss}";
            }

            DisableControls();
            var bgw = new BackgroundWorker();
            bgw.DoWork += (o, args) => {
                ShowStatusBar();
                CheckVersionAvailability();
                UpdateVersionListView();
                CheckLibraries();
                CheckGameResources();
                HideStatusBar();
            };
            bgw.RunWorkerCompleted += (o, args) => {
                // запуск в UI потоке
                Launch();
                UpdateVersionListView();
                EnableControls();
                _versionToLaunch = null;
                _restoreVersion = false;
            };
            bgw.RunWorkerAsync();
        }
        
        private void ShowStatusBar() {
            SetStatusBarVisibility(true);
        }
        
        private void HideStatusBar() {
            SetStatusBarVisibility(false);
        }
        
        private void CheckVersionAvailability() {
            long state = 0;
            WebClient downloader = new WebClient();
            downloader.DownloadProgressChanged += (sender, e) => { SetStatusBarValue(e.ProgressPercentage); };
            downloader.DownloadFileCompleted += delegate { state++; };
            SetStatusBarMaxValue(100);
            SetStatusBarValue(0);
            string version = _versionToLaunch ?? _selectedProfile.GetSelectedVersion(_applicationContext);
            UpdateStatusBarText(string.Format(_applicationContext.ProgramLocalization.CheckingVersionAvailability, version));
            AppendLog($"Checking '{version}' version availability...");
            string path = Path.Combine(_applicationContext.McVersions, version + "\\");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(path + "/" + version + ".json") || _restoreVersion) {
                string filename = version + ".json";
                UpdateStatusBarAndLog("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.json", version)),
                    string.Format("{0}/{1}/{1}.json", _applicationContext.McVersions, version));
            }
            else {
                state++;
            }
            
            SetStatusBarValue(0);
            while (state != 1) ;
            Version selectedVersion = Version.ParseVersion(new DirectoryInfo(_applicationContext.McVersions + version), false);
            if ((!File.Exists(path + "/" + version + ".jar") || _restoreVersion) &&
                selectedVersion.InheritsFrom == null) {
                string filename = version + ".jar";
                UpdateStatusBarAndLog("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.jar", version)),
                    string.Format("{0}/{1}/{1}.jar", _applicationContext.McVersions, version));
            }
            else {
                state++;
            }

            while (state != 2) ;
            if (selectedVersion.InheritsFrom == null) {
                AppendLog("Finished checking version avalability.");
                return;
            }

            path = Path.Combine(_applicationContext.McVersions, selectedVersion.InheritsFrom + "\\");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(string.Format("{0}/{1}/{1}.jar", _applicationContext.McVersions, selectedVersion.InheritsFrom)) || _restoreVersion) {
                string filename = selectedVersion.InheritsFrom + ".jar";
                UpdateStatusBarAndLog("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.jar", selectedVersion.InheritsFrom)),
                    string.Format("{0}/{1}/{1}.jar", _applicationContext.McVersions, selectedVersion.InheritsFrom));
            }
            else {
                state++;
            }

            while (state != 3) ;
            if (!File.Exists(string.Format("{0}/{1}/{1}.json", _applicationContext.McVersions, selectedVersion.InheritsFrom)) || _restoreVersion) {
                string filename = selectedVersion.InheritsFrom + ".json";
                UpdateStatusBarAndLog("Downloading " + filename + "...");
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.json", selectedVersion.InheritsFrom)),
                    string.Format("{0}/{1}/{1}.json", _applicationContext.McVersions, selectedVersion.InheritsFrom));
            }
            else {
                state++;
            }

            while (state != 4) ;
            AppendLog("Finished checking version avalability.");
        }

        private void UpdateVersionListView() {
            if (logBox.InvokeRequired) {
                logBox.Invoke(new Action(UpdateVersionListView));
            }
            else {
                versionsListView.Items.Clear();
                foreach (Version version in Directory.GetDirectories(_applicationContext.McVersions)
                        .Select(versionDir => new DirectoryInfo(versionDir))
                        .Select(info => Version.ParseVersion(info, false))) {
                    versionsListView.Items.Add(version.VersionId, version.ReleaseType,
                        version.InheritsFrom ?? _applicationContext.ProgramLocalization.Independent);
                }
            }
        }
        
        private void CheckLibraries() {
            string libraries = string.Empty;
            Version selectedVersion = Version.ParseVersion(
                new DirectoryInfo(_applicationContext.McVersions +
                                  (_versionToLaunch ?? _selectedProfile.GetSelectedVersion(_applicationContext))));
            SetStatusBarValue(0);
            SetStatusBarMaxValue(selectedVersion.Libs.Count(a => a.IsForWindows()) + 1);
            UpdateStatusBarText(_applicationContext.ProgramLocalization.CheckingLibraries);
            AppendLog("Checking libraries...");
            foreach (Lib lib in selectedVersion.Libs.Where(a => a.IsForWindows())) {
                IncStatusBarValue();
                if (!File.Exists(_applicationContext.McLibs + lib.ToPath()) || _restoreVersion) {
                    UpdateStatusBarAndLog("Downloading " + lib.Name + "...");
                    AppendDebug("Url: " + (lib.Url ?? @"https://libraries.minecraft.net/") + lib.ToPath());
                    string directory = Path.GetDirectoryName(_applicationContext.McLibs + lib.ToPath());
                    if (!File.Exists(directory)) {
                        Directory.CreateDirectory(directory);
                    }

                    new WebClient().DownloadFile((lib.Url ?? @"https://libraries.minecraft.net/") + lib.ToPath(),
                        _applicationContext.McLibs + lib.ToPath());
                }

                if (lib.IsNative != null) {
                    UpdateStatusBarAndLog("Unpacking " + lib.Name + "...");
                    using (ZipFile zip = ZipFile.Read(_applicationContext.McLibs + lib.ToPath())) {
                        foreach (ZipEntry entry in zip.Where(entry => entry.FileName.EndsWith(".dll"))) {
                            AppendDebug($"Unzipping {entry.FileName}");
                            try {
                                entry.Extract(_applicationContext.McNatives, ExtractExistingFileAction.OverwriteSilently);
                            }
                            catch (Exception ex) {
                                AppendException(ex.Message);
                            }
                        }
                    }
                }
                else {
                    libraries += _applicationContext.McLibs + lib.ToPath() + ";";
                }

                UpdateStatusBarText(_applicationContext.ProgramLocalization.CheckingLibraries);
            }

            libraries += string.Format("{0}{1}\\{1}.jar", _applicationContext.McVersions,
                selectedVersion.InheritsFrom ??
                (_versionToLaunch ?? _selectedProfile.GetSelectedVersion(_applicationContext)));
            _applicationContext.Libraries = libraries;
            AppendLog("Finished checking libraries.");
        }
        
        private void CheckGameResources() {
            UpdateStatusBarAndLog("Checking game assets...");
            Version selectedVersion = Version.ParseVersion(
                new DirectoryInfo(_applicationContext.McVersions +
                                  (_versionToLaunch ?? _selectedProfile.GetSelectedVersion(_applicationContext))));
            string file = string.Format("{0}/indexes/{1}.json", _applicationContext.McAssets, selectedVersion.AssetsIndex ?? "legacy");
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
                .Select(c => c[0] + c[1] + "\\" + c)
                .Where(filename => !File.Exists(_applicationContext.McObjectsAssets + filename) || _restoreVersion).ToList();
            SetStatusBarValue(0);
            SetStatusBarMaxValue(something.Count + 1);
            foreach (string resourseFile in something) {
                string path = _applicationContext.McObjectsAssets + resourseFile[0] + resourseFile[1] + "\\";
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                try {
                    AppendDebug("Downloading " + resourseFile + "...");
                    new WebClient().DownloadFile(@"http://resources.download.minecraft.net/" + resourseFile,
                        _applicationContext.McObjectsAssets + resourseFile);
                }
                catch (Exception ex) {
                    AppendException(ex.ToString());
                }

                IncStatusBarValue();
            }

            AppendLog("Finished checking game assets.");
            if (selectedVersion.AssetsIndex == null) {
                SetStatusBarValue(0);
                StatusBarMaxValue = jo["objects"].Cast<JProperty>()
                    .Count(res => !File.Exists(_applicationContext.McLegacyAssets + res.Name)) + 1;
                UpdateStatusBarAndLog("Converting assets...");
                foreach (JProperty res in jo["objects"].Cast<JProperty>()
                             .Where(res => !File.Exists(_applicationContext.McLegacyAssets + res.Name) || _restoreVersion)) {
                    try {
                        FileInfo resFile = new FileInfo(_applicationContext.McLegacyAssets + res.Name);
                        if (!resFile.Directory.Exists) {
                            resFile.Directory.Create();
                        }

                        AppendDebug(
                            $"Converting \"{"\\assets\\objects\\" + res.Value["hash"].ToString()[0] + res.Value["hash"].ToString()[1] + "\\" + res.Value["hash"]}\" to \"{"\\assets\\legacy\\" + res.Name}\"");
                        File.Copy(_applicationContext.McObjectsAssets + res.Value["hash"].ToString()[0] +
                                  res.Value["hash"].ToString()[1] + "\\" + res.Value["hash"],
                            resFile.FullName);
                    }
                    catch (Exception ex) {
                        AppendLog(ex.ToString());
                    }

                    IncStatusBarValue();
                }

                AppendLog("Finished converting assets.");
            }
        }
        
        private void Launch() {
            _presenter.SelectUserForLaunch(NicknameDropDownList.Text);
            _presenter.SaveUsers();
            UpdateUserList();
            var selectedVersion = Version.ParseVersion(
                new DirectoryInfo(_applicationContext.McVersions + (_versionToLaunch ?? _selectedProfile.GetSelectedVersion(_applicationContext))));

            if (_selectedProfile.FastConnectionSettigs != null) {
                selectedVersion.ArgumentCollection.Add("server", _selectedProfile.FastConnectionSettigs.ServerIP);
                selectedVersion.ArgumentCollection.Add("port", _selectedProfile.FastConnectionSettigs.ServerPort.ToString());
            }

            if (_selectedProfile.WorkingDirectory != null && !Directory.Exists(_selectedProfile.WorkingDirectory)) {
                Directory.CreateDirectory(_selectedProfile.WorkingDirectory);
            }

            var proc = ProcessInfoBuilder.Create(_applicationContext)
                .Profile(_selectedProfile)
                .User(_presenter.SelectedUser)
                .Version(selectedVersion)
                .OfflineNickname(NicknameDropDownList.Text)
                .Build();

            AppendLog($"Command line: \"{proc.FileName}\" {proc.Arguments}");
            AppendLog($"Version {selectedVersion.VersionId} successfuly launched.");

            CreateMinecraftProcessPage(proc).Launch();
        }

        /// <summary>
        /// Создание вкладки для нового процесса Minecraft
        /// </summary>
        private MinecraftProcessPage CreateMinecraftProcessPage(ProcessStartInfo processInfo) {
            var context = new MinecraftProcessPageContext(this, _selectedProfile, _applicationContext);
            var mcProcessPage = new MinecraftProcessPage(new Process {StartInfo = processInfo, EnableRaisingEvents = true}, context);
            mcProcessPage.PageCreated += (o, page) => {
                mainPageView.Pages.Add(page);
                mainPageView.SelectedPage = page;
            };
            mcProcessPage.PageClosed += (o, page) => { mainPageView.Pages.Remove(page); };
            mcProcessPage.ProcessLaunched += (o, args) => {
                if (_selectedProfile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.CLOSED) {
                    Close();
                }

                if (_selectedProfile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.HIDDEN) {
                    Hide();
                }
            };
            mcProcessPage.ProcessExited += (o, args) => {
                if (_selectedProfile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.HIDDEN) {
                    Invoke((MethodInvoker) Show);
                }
            };
            return mcProcessPage;
        }

        private void logBox_TextChanged(object sender, EventArgs e) {
            logBox.SelectionStart = logBox.Text.Length;
            logBox.ScrollToCaret();
        }

        private void versionsListView_ItemMouseClick(object sender, ListViewItemEventArgs e) {
            versionsListView.SelectedItem = e.Item;
            Version ver =
                Version.ParseVersion(
                    new DirectoryInfo(Path.Combine(_applicationContext.McVersions, versionsListView.SelectedItem[0].ToString() + "\\")), false);
            RadMenuItem launchVerButton = new RadMenuItem {Text = _applicationContext.ProgramLocalization.Launch};
            launchVerButton.Click += delegate {
                if (versionsListView.SelectedItem == null) {
                    return;
                }

                _versionToLaunch = versionsListView.SelectedItem[0].ToString();
                LaunchButton.PerformClick();
            };
            bool enableRestoreButton = false;
            switch (ver.ReleaseType) {
                case "release":
                case "snapshot":
                case "old_beta":
                case "old_alpha":
                    enableRestoreButton = true;
                    break;
            }

            RadMenuItem restoreVerButton = new RadMenuItem {Text = "Restore and launch", Enabled = enableRestoreButton};
            restoreVerButton.Click += delegate {
                _restoreVersion = true;
                _versionToLaunch = versionsListView.SelectedItem[0].ToString();
                LaunchButton.PerformClick();
            };
            RadMenuItem openVerButton = new RadMenuItem {Text = _applicationContext.ProgramLocalization.OpenLocation};
            openVerButton.Click += delegate {
                if (versionsListView.SelectedItem == null) {
                    return;
                }

                Process.Start(Path.Combine(_applicationContext.McVersions, versionsListView.SelectedItem[0].ToString() + "\\"));
            };
            RadMenuItem delVerButton = new RadMenuItem {Text = _applicationContext.ProgramLocalization.DeleteVersion};
            delVerButton.Click += delegate {
                if (versionsListView.SelectedItem == null) {
                    return;
                }

                DialogResult dr =
                    RadMessageBox.Show(
                        string.Format(_applicationContext.ProgramLocalization.DeleteConfirmationText,
                            versionsListView.SelectedItem[0].ToString()),
                        _applicationContext.ProgramLocalization.DeleteConfirmationTitle,
                        MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr != DialogResult.Yes) {
                    return;
                }

                try {
                    Directory.Delete(
                        Path.Combine(_applicationContext.McVersions, versionsListView.SelectedItem[0].ToString() + "\\"), true);
                    AppendLog($"Version '{versionsListView.SelectedItem[0].ToString()}' has been deleted successfuly.");
                    UpdateVersionListView();
                }
                catch (Exception ex) {
                    AppendException($"An error has occurred during version deletion: {ex.ToString()}");
                }

                string path = Path.Combine(_applicationContext.McVersions, _selectedProfile.GetSelectedVersion(_applicationContext) + "\\");
                string state = _applicationContext.ProgramLocalization.ReadyToLaunch;
                if (!File.Exists(string.Format("{0}/{1}.json", path, _selectedProfile.GetSelectedVersion(_applicationContext)))) {
                    state = _applicationContext.ProgramLocalization.ReadyToDownload;
                }

                SelectedVersion.Text = string.Format(state, _selectedProfile.GetSelectedVersion(_applicationContext));
            };
            RadContextMenu verListMenuStrip = new RadContextMenu();
            verListMenuStrip.Items.Add(launchVerButton);
            verListMenuStrip.Items.Add(new RadMenuSeparatorItem());
            verListMenuStrip.Items.Add(restoreVerButton);
            verListMenuStrip.Items.Add(new RadMenuSeparatorItem());
            verListMenuStrip.Items.Add(openVerButton);
            verListMenuStrip.Items.Add(delVerButton);
            new RadContextMenuManager().SetRadContextMenu(versionsListView, verListMenuStrip);
        }

        private void SetToClipboardButton_Click(object sender, EventArgs e) {
            Clipboard.SetText(logBox.Text);
        }

        private void urlLabel_Click(object sender, EventArgs e) {
            Process.Start((sender as Label).Text);
        }

        private void LangDropDownList_SelectedIndexChanged(object sender, PositionChangedEventArgs e) {
            if (LangDropDownList.SelectedItem.Tag.ToString() == _cfg.SelectedLanguage) {
                return;
            }

            var selectedLocalization = LangDropDownList.SelectedItem.Tag;
            if (LangDropDownList.SelectedIndex == 0)
                _applicationContext.SetLocalization(string.Empty);
            else
                _applicationContext.SetLocalization(selectedLocalization.ToString());

            _cfg.SelectedLanguage = selectedLocalization.ToString();
            AppendLog($"Application language changed to {selectedLocalization}");
            LoadLocalization();
        }

        private void UpdateVersions() {
            AppendLog("Checking version.json...");
            string jsonVersionList = new WebClient().DownloadString(
                new Uri("https://s3.amazonaws.com/Minecraft.Download/versions/versions.json"));
            if (!Directory.Exists(_applicationContext.McVersions)) {
                Directory.CreateDirectory(_applicationContext.McVersions);
            }

            if (!File.Exists(_applicationContext.McVersions + "\\versions.json")) {
                File.WriteAllText(_applicationContext.McVersions + "\\versions.json", jsonVersionList);
                return;
            }

            JObject jb =
                JObject.Parse(jsonVersionList);
            string remoteSnapshotVersion = jb["latest"]["snapshot"].ToString(),
                remoteReleaseVersion = jb["latest"]["release"].ToString();
            AppendLog("Latest snapshot: " + remoteSnapshotVersion);
            AppendLog("Latest release: " + remoteReleaseVersion);
            JObject ver = JObject.Parse(File.ReadAllText(_applicationContext.McVersions + "/versions.json"));
            string localSnapshotVersion = ver["latest"]["snapshot"].ToString(),
                localReleaseVersion = ver["latest"]["release"].ToString();
            AppendLog("Local versions: " + ((JArray) jb["versions"]).Count + ". Remote versions: " +
                      ((JArray) ver["versions"]).Count);
            if (((JArray) jb["versions"]).Count == ((JArray) ver["versions"]).Count &&
                remoteReleaseVersion == localReleaseVersion && remoteSnapshotVersion == localSnapshotVersion) {
                AppendLog("No update found.");
                return;
            }

            AppendLog("Writting new list... ");
            File.WriteAllText(_applicationContext.McVersions + "\\versions.json", jsonVersionList);
        }

        private void UpdateProfileList() {
            profilesDropDownBox.Items.Clear();
            try {
                _profileManager = LauncherExtensions.ParseProfile(_applicationContext.McDirectory + "/launcher_profiles.json");
                if (!_profileManager.Profiles.Any()) {
                    throw new Exception("Someone broke my profiles>:(");
                }
            }
            catch (Exception ex) {
                AppendException("Reading profile list: an exception has occurred\n" + ex.Message + "\nCreating a new one.");

                // save backup
                if (File.Exists(_applicationContext.LauncherProfiles)) {
                    string fileName = "launcher_profiles-" + DateTime.Now.ToString("hhmmss") + ".bak.json";
                    AppendLog("A copy of old profile list has been created: " + fileName);
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

                _profileManager = LauncherExtensions.ParseProfile(_applicationContext.LauncherProfiles);
                _presenter.SaveProfiles(_profileManager);
            }

            DeleteProfileButton.Enabled = _profileManager.Profiles.Count > 1;
            profilesDropDownBox.Items.AddRange(_profileManager.Profiles.Keys);
            profilesDropDownBox.SelectedItem = profilesDropDownBox.FindItemExact(_profileManager.LastUsedProfile, true);
        }

        private void UpdateUserList() {
            NicknameDropDownList.Items.Clear();
            _presenter.ReloadUserManager();
            NicknameDropDownList.Items.AddRange(_presenter.UserManager.Accounts.Keys);
            NicknameDropDownList.SelectedItem = NicknameDropDownList.FindItemExact(_presenter.UserManager.SelectedUsername, true);
        }

        private void LoadLocalization() {
            ConsolePage.Text = _applicationContext.ProgramLocalization.ConsoleTabText;
            EditVersions.Text = _applicationContext.ProgramLocalization.ManageVersionsTabText;
            AboutPage.Text = _applicationContext.ProgramLocalization.AboutTabText;
            SettingsPage.Text = _applicationContext.ProgramLocalization.SettingsTabText;

            LaunchButton.Text = _applicationContext.ProgramLocalization.LaunchButtonText;
            AddProfile.Text = _applicationContext.ProgramLocalization.AddProfileButtonText;
            EditProfile.Text = _applicationContext.ProgramLocalization.EditProfileButtonText;

            DevInfoLabel.Text = _applicationContext.ProgramLocalization.DevInfo;
            GratitudesLabel.Text = _applicationContext.ProgramLocalization.GratitudesText;
            GratitudesDescLabel.Text = _applicationContext.ProgramLocalization.GratitudesDescription;
            PartnersLabel.Text = _applicationContext.ProgramLocalization.PartnersText;
            MCofflineDescLabel.Text = _applicationContext.ProgramLocalization.MCofflineDescription;
            CopyrightInfoLabel.Text = _applicationContext.ProgramLocalization.CopyrightInfo;

            EnableMinecraftUpdateAlerts.Text = _applicationContext.ProgramLocalization.EnableMinecraftUpdateAlertsText;
            EnableMinecraftLogging.Text = _applicationContext.ProgramLocalization.EnableMinecraftLoggingText;
            UseGamePrefix.Text = _applicationContext.ProgramLocalization.UseGamePrefixText;
            CloseGameOutput.Text = _applicationContext.ProgramLocalization.CloseGameOutputText;
        }

        private void UpdateStatusBarText(string text) {
            if (logBox.InvokeRequired) {
                logBox.Invoke(new Action<string>(UpdateStatusBarText), text);
            }
            else {
                StatusBar.Text = text;
            }
        }

        private void UpdateStatusBarAndLog(string text, string methodName = null) {
            if (logBox.InvokeRequired) {
                logBox.Invoke(new Action<string, string>(UpdateStatusBarAndLog), text,
                    new StackFrame(1).GetMethod().Name);
            }
            else {
                StatusBar.Text = text;
                AppendLog(text, methodName);
            }
        }

        public void AppendLog(string text, string methodName = null) {
            if (logBox.InvokeRequired) {
                logBox.Invoke(new Action<string, string>(AppendLog), text, new StackFrame(1).GetMethod().Name);
            }
            else {
                logBox.AppendText(string.Format(
                    string.IsNullOrEmpty(logBox.Text) ? "[{0}][{1}][{2}] {3}" : "\n[{0}][{1}][{2}] {3}",
                    DateTime.Now.ToString("dd-MM-yy HH:mm:ss"), "INFO",
                    methodName ?? new StackFrame(1, false).GetMethod().Name, text));
            }
        }

        public void AppendException(string text, string methodName = null) {
            if (logBox.InvokeRequired) {
                logBox.Invoke(new Action<string, string>(AppendException), text, new StackFrame(1).GetMethod().Name);
            }
            else {
                logBox.AppendText(string.Format(
                    string.IsNullOrEmpty(logBox.Text) ? "[{0}][{1}][{2}] {3}" : "\n[{0}][{1}][{2}] {3}",
                    DateTime.Now.ToString("dd-MM-yy HH:mm:ss"), "ERR",
                    methodName ?? new StackFrame(1, false).GetMethod().Name, text));
            }
        }

        public void AppendDebug(string text, string methodName = null) {
            if (!DebugModeButton.IsChecked) {
                return;
            }

            if (logBox.InvokeRequired) {
                logBox.Invoke(new Action<string, string>(AppendDebug), text, new StackFrame(1).GetMethod().Name);
            }
            else {
                logBox.AppendText(string.Format(
                    string.IsNullOrEmpty(logBox.Text) ? "[{0}][{1}][{2}] {3}" : "\n[{0}][{1}][{2}] {3}",
                    DateTime.Now.ToString("dd-MM-yy HH:mm:ss"), "DEBUG",
                    methodName ?? new StackFrame(1, false).GetMethod().Name, text));
            }
        }
        
        private void SetStatusBarVisibility(bool b) {
            if (logBox.InvokeRequired) {
                logBox.Invoke(new Action<bool>(SetStatusBarVisibility), b);
            }
            else {
                StatusBar.Visible = b;
            }
        }
    }
}