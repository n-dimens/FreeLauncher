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
    public partial class LauncherForm : RadForm {
        private readonly LauncherFormPresenter _presenter;
        private readonly ApplicationContext _applicationContext;
        private Profile _selectedProfile;

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
                DeleteProfileButton.Enabled = !value && (_presenter.ProfileManager.Profiles.Count > 1);
                EditProfile.Enabled = !value;
                AddProfile.Enabled = !value;
                NicknameDropDownList.Enabled = !value;
            }
        }
       
        public LauncherForm(LauncherFormPresenter presenter) {
            _presenter = presenter;
            _applicationContext = presenter.AppContext;
            InitializeComponent();
            LoadLocalization();
            //
            Text = ProductName + " " + ProductVersion;

            if (!Directory.Exists(_applicationContext.McDirectory)) {
                Directory.CreateDirectory(_applicationContext.McDirectory);
            }

            if (!Directory.Exists(_applicationContext.McLauncher)) {
                Directory.CreateDirectory(_applicationContext.McLauncher);
            }

            Focus();
            
            _presenter.UpdateVersionsList();
            UpdateProfileList();
            
            _presenter.ReloadUserManager();
            UpdateNicknameDropDownList(_presenter.UserManager);
        }

        private void profilesDropDownBox_SelectedIndexChanged(object sender, PositionChangedEventArgs e) {
            if (profilesDropDownBox.SelectedItem == null) {
                return;
            }

            _presenter.ProfileManager.LastUsedProfile = profilesDropDownBox.SelectedItem.Text;
            _selectedProfile = _presenter.ProfileManager.Profiles[profilesDropDownBox.SelectedItem.Text];
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
                _presenter.ProfileManager.Profiles.Remove(_presenter.ProfileManager.LastUsedProfile);
                if (_presenter.ProfileManager.Profiles.ContainsKey(pf.CurrentProfile.ProfileName)) {
                    RadMessageBox.Show(_applicationContext.ProgramLocalization.ProfileAlreadyExistsErrorText,
                        _applicationContext.ProgramLocalization.Error,
                        MessageBoxButtons.OK, RadMessageIcon.Error);
                    UpdateProfileList();
                    return;
                }

                _presenter.ProfileManager.Profiles.Add(pf.CurrentProfile.ProfileName, pf.CurrentProfile);
                _presenter.ProfileManager.LastUsedProfile = pf.CurrentProfile.ProfileName;
            }

            _presenter.SaveProfiles();
            UpdateProfileList();
        }

        private void AddProfile_Click(object sender, EventArgs e) {
            Profile editedProfile = Profile.ParseProfile(_selectedProfile.ToString());
            editedProfile.ProfileName = "Copy of '" + _selectedProfile.ProfileName + "'(" +
                                        DateTime.Now.ToString("HH:mm:ss") + ")";
            ProfileForm pf = new ProfileForm(editedProfile, _applicationContext) {Text = _applicationContext.ProgramLocalization.AddingProfileTitle};
            pf.ShowDialog();
            if (pf.DialogResult == DialogResult.OK) {
                if (_presenter.ProfileManager.Profiles.ContainsKey(editedProfile.ProfileName)) {
                    RadMessageBox.Show(_applicationContext.ProgramLocalization.ProfileAlreadyExistsErrorText,
                        _applicationContext.ProgramLocalization.Error,
                        MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                _presenter.ProfileManager.Profiles.Add(editedProfile.ProfileName, editedProfile);
                _presenter.ProfileManager.LastUsedProfile = pf.CurrentProfile.ProfileName;
            }

            _presenter.SaveProfiles();
            UpdateProfileList();
        }

        private void DeleteProfileButton_Click(object sender, EventArgs e) {
            DialogResult dr =
                RadMessageBox.Show(
                    string.Format(_applicationContext.ProgramLocalization.ProfileDeleteConfirmationText,
                        _presenter.ProfileManager.LastUsedProfile), _applicationContext.ProgramLocalization.DeleteConfirmationTitle,
                    MessageBoxButtons.YesNo, RadMessageIcon.Question);
            if (dr != DialogResult.Yes) {
                return;
            }

            _presenter.ProfileManager.Profiles.Remove(_presenter.ProfileManager.LastUsedProfile);
            _presenter.ProfileManager.LastUsedProfile = _presenter.ProfileManager.Profiles.FirstOrDefault().Key;
            _presenter.SaveProfiles();
            UpdateProfileList();
        }

        private void ManageUsersButton_Click(object sender, EventArgs e) {
            new UsersForm(_applicationContext).ShowDialog();
            _presenter.ReloadUserManager();
            UpdateNicknameDropDownList(_presenter.UserManager);
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
                CheckLibraries();
                CheckGameResources();
                HideStatusBar();
            };
            bgw.RunWorkerCompleted += (o, args) => {
                // запуск в UI потоке
                Launch();
                EnableControls();
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
            string version = _selectedProfile.GetSelectedVersion(_applicationContext);
            UpdateStatusBarText(string.Format(_applicationContext.ProgramLocalization.CheckingVersionAvailability, version));
            _presenter.LogInfo($"Checking '{version}' version availability...");
            string path = Path.Combine(_applicationContext.McVersions, version + "\\");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(path + "/" + version + ".json")) {
                string filename = version + ".json";
                UpdateStatusBarText("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
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
            if ((!File.Exists(path + "/" + version + ".jar")) &&
                selectedVersion.InheritsFrom == null) {
                string filename = version + ".jar";
                UpdateStatusBarText("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.jar", version)),
                    string.Format("{0}/{1}/{1}.jar", _applicationContext.McVersions, version));
            }
            else {
                state++;
            }

            while (state != 2) ;
            if (selectedVersion.InheritsFrom == null) {
                _presenter.LogInfo("Finished checking version avalability.");
                return;
            }

            path = Path.Combine(_applicationContext.McVersions, selectedVersion.InheritsFrom + "\\");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(string.Format("{0}/{1}/{1}.jar", _applicationContext.McVersions, selectedVersion.InheritsFrom))) {
                string filename = selectedVersion.InheritsFrom + ".jar";
                UpdateStatusBarText("Downloading " + filename + "...", new StackFrame().GetMethod().Name);
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.jar", selectedVersion.InheritsFrom)),
                    string.Format("{0}/{1}/{1}.jar", _applicationContext.McVersions, selectedVersion.InheritsFrom));
            }
            else {
                state++;
            }

            while (state != 3) ;
            if (!File.Exists(string.Format("{0}/{1}/{1}.json", _applicationContext.McVersions, selectedVersion.InheritsFrom))) {
                string filename = selectedVersion.InheritsFrom + ".json";
                UpdateStatusBarText("Downloading " + filename + "...");
                downloader.DownloadFileAsync(new Uri(string.Format(
                        "https://s3.amazonaws.com/Minecraft.Download/versions/{0}/{0}.json", selectedVersion.InheritsFrom)),
                    string.Format("{0}/{1}/{1}.json", _applicationContext.McVersions, selectedVersion.InheritsFrom));
            }
            else {
                state++;
            }

            while (state != 4) ;
            _presenter.LogInfo("Finished checking version avalability.");
        }
        
        private void CheckLibraries() {
            string libraries = string.Empty;
            Version selectedVersion = Version.ParseVersion(
                new DirectoryInfo(_applicationContext.McVersions + _selectedProfile.GetSelectedVersion(_applicationContext)));
            SetStatusBarValue(0);
            SetStatusBarMaxValue(selectedVersion.Libs.Count(a => a.IsForWindows()) + 1);
            UpdateStatusBarText(_applicationContext.ProgramLocalization.CheckingLibraries);
            _presenter.LogInfo("Checking libraries...");
            foreach (Lib lib in selectedVersion.Libs.Where(a => a.IsForWindows())) {
                IncStatusBarValue();
                if (!File.Exists(_applicationContext.McLibs + lib.ToPath())) {
                    UpdateStatusBarText("Downloading " + lib.Name + "...");
                    _presenter.LogDebug("Url: " + (lib.Url ?? @"https://libraries.minecraft.net/") + lib.ToPath());
                    string directory = Path.GetDirectoryName(_applicationContext.McLibs + lib.ToPath());
                    if (!File.Exists(directory)) {
                        Directory.CreateDirectory(directory);
                    }

                    new WebClient().DownloadFile((lib.Url ?? @"https://libraries.minecraft.net/") + lib.ToPath(),
                        _applicationContext.McLibs + lib.ToPath());
                }

                if (lib.IsNative != null) {
                    UpdateStatusBarText("Unpacking " + lib.Name + "...");
                    using (ZipFile zip = ZipFile.Read(_applicationContext.McLibs + lib.ToPath())) {
                        foreach (ZipEntry entry in zip.Where(entry => entry.FileName.EndsWith(".dll"))) {
                            _presenter.LogDebug($"Unzipping {entry.FileName}");
                            try {
                                entry.Extract(_applicationContext.McNatives, ExtractExistingFileAction.OverwriteSilently);
                            }
                            catch (Exception ex) {
                                _presenter.LogError(ex.Message);
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
                selectedVersion.InheritsFrom ?? _selectedProfile.GetSelectedVersion(_applicationContext));
            _applicationContext.Libraries = libraries;
            _presenter.LogInfo("Finished checking libraries.");
        }
        
        private void CheckGameResources() {
            UpdateStatusBarText("Checking game assets...");
            Version selectedVersion = Version.ParseVersion(
                new DirectoryInfo(_applicationContext.McVersions + _selectedProfile.GetSelectedVersion(_applicationContext)));
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
                .Select(c => c[0].ToString() + c[1].ToString() + "\\" + c)
                .Where(filename => !File.Exists(_applicationContext.McObjectsAssets + filename)).ToList();
            SetStatusBarValue(0);
            SetStatusBarMaxValue(something.Count + 1);
            foreach (string resourseFile in something) {
                string path = _applicationContext.McObjectsAssets + resourseFile[0] + resourseFile[1] + "\\";
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                try {
                    _presenter.LogDebug("Downloading " + resourseFile + "...");
                    new WebClient().DownloadFile(@"http://resources.download.minecraft.net/" + resourseFile,
                        _applicationContext.McObjectsAssets + resourseFile);
                }
                catch (Exception ex) {
                    _presenter.LogError(ex.ToString());
                }

                IncStatusBarValue();
            }

            _presenter.LogInfo("Finished checking game assets.");
            if (selectedVersion.AssetsIndex == null) {
                SetStatusBarValue(0);
                StatusBarMaxValue = jo["objects"].Cast<JProperty>()
                    .Count(res => !File.Exists(_applicationContext.McLegacyAssets + res.Name)) + 1;
                UpdateStatusBarText("Converting assets...");
                foreach (JProperty res in jo["objects"].Cast<JProperty>()
                             .Where(res => !File.Exists(_applicationContext.McLegacyAssets + res.Name))) {
                    try {
                        FileInfo resFile = new FileInfo(_applicationContext.McLegacyAssets + res.Name);
                        if (!resFile.Directory.Exists) {
                            resFile.Directory.Create();
                        }

                        _presenter.LogDebug(
                            $"Converting \"{"\\assets\\objects\\" + res.Value["hash"].ToString()[0] + res.Value["hash"].ToString()[1] + "\\" + res.Value["hash"]}\" to \"{"\\assets\\legacy\\" + res.Name}\"");
                        File.Copy(_applicationContext.McObjectsAssets + res.Value["hash"].ToString()[0] +
                                  res.Value["hash"].ToString()[1] + "\\" + res.Value["hash"],
                            resFile.FullName);
                    }
                    catch (Exception ex) {
                        _presenter.LogError(ex.ToString());
                    }

                    IncStatusBarValue();
                }

                _presenter.LogInfo("Finished converting assets.");
            }
        }
        
        private void Launch() {
            _presenter.SelectUserForLaunch(NicknameDropDownList.Text);
            _presenter.SaveUsers();
            _presenter.ReloadUserManager();
            UpdateNicknameDropDownList(_presenter.UserManager);
            var selectedVersion = Version.ParseVersion(
                new DirectoryInfo(_applicationContext.McVersions + _selectedProfile.GetSelectedVersion(_applicationContext)));

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

            _presenter.LogInfo($"Command line: \"{proc.FileName}\" {proc.Arguments}");
            _presenter.LogInfo($"Version {selectedVersion.VersionId} successfuly launched.");

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

        private void UpdateProfileList() {
            _presenter.ReloadProfileManager();
            profilesDropDownBox.Items.Clear();
            DeleteProfileButton.Enabled = _presenter.ProfileManager.Profiles.Count > 1;
            profilesDropDownBox.Items.AddRange(_presenter.ProfileManager.Profiles.Keys);
            profilesDropDownBox.SelectedItem = profilesDropDownBox.FindItemExact(_presenter.ProfileManager.LastUsedProfile, true);
        }

        private void UpdateNicknameDropDownList(UserManager userManager) {
            NicknameDropDownList.Items.Clear();
            NicknameDropDownList.Items.AddRange(userManager.Accounts.Keys);
            NicknameDropDownList.SelectedItem = NicknameDropDownList.FindItemExact(userManager.SelectedUsername, true);
        }

        private void LoadLocalization() {
            // EditVersions.Text = _applicationContext.ProgramLocalization.ManageVersionsTabText;
            LaunchButton.Text = _applicationContext.ProgramLocalization.LaunchButtonText;
            AddProfile.Text = _applicationContext.ProgramLocalization.AddProfileButtonText;
            EditProfile.Text = _applicationContext.ProgramLocalization.EditProfileButtonText;           
        }

        private void UpdateStatusBarText(string text, string methodName = null) {
            if (StatusBar.InvokeRequired) {
                StatusBar.Invoke(new Action<string, string>(UpdateStatusBarText), text, methodName);
            }
            else {
                StatusBar.Text = text;
                _presenter.LogInfo(text, methodName);
            }
        }

        private int StatusBarMaxValue {
            set => SetStatusBarMaxValue(value);
        }

        private void SetStatusBarVisibility(bool b) {
            if (StatusBar.InvokeRequired) {
                StatusBar.Invoke(new Action<bool>(SetStatusBarVisibility), b);
            }
            else {
                StatusBar.Visible = b;
            }
        }

        // TODO: to event
        private void IncStatusBarValue() {
            SetStatusBarValue(StatusBar.Value1 + 1);
        }

        // TODO: to event
        private void SetStatusBarValue(int i) {
            if (StatusBar.InvokeRequired) {
                StatusBar.Invoke(new Action<int>(SetStatusBarValue), i);
            }
            else {
                StatusBar.Value1 = i;
            }
        }

        // TODO: to event
        private void SetStatusBarMaxValue(int value) {
            if (StatusBar.InvokeRequired) {
                StatusBar.Invoke(new Action<int>(SetStatusBarMaxValue), value);
            }
            else {
                StatusBar.Maximum = value;
            }
        }
    }
}