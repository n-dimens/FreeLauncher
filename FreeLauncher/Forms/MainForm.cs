namespace FreeLauncher.Forms {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Version = dotMCLauncher.Core.Version;
    using dotMCLauncher.Core;

    // using Microsoft.VisualBasic.Devices;
    using Microsoft.TeamFoundation.Common;

    public partial class MainForm : Form, ILauncherLogger, IProgressView {
        private readonly Localization _localization;
        private readonly GameFileStructure _applicationContext;
        private readonly MainFormPresenter _presenter;
        private readonly GameProcessForm frmGameProcess;

        public MainForm(GameFileStructure appContext, Localization localization) {
            _applicationContext = appContext;
            _localization = localization;
            _presenter = new MainFormPresenter(this, this, _applicationContext);
            InitializeComponent();

            PrintAppInfo();
            frmGameProcess = new GameProcessForm(_presenter.AppContext);

            LoadConfiguration();

            _presenter.ReloadProfileManager();
            LoadProfilesList();

            _presenter.ReloadUserManager();
            LoadUsersList(_presenter.UserManager);
        }

        private void PrintAppInfo() {
            _presenter.LogInfo($"Application: {ProductName} v.{ProductVersion}");
            _presenter.LogInfo($"Application language: {_localization.Name}({_localization.LanguageTag})");
            _presenter.LogInfo("==============");
            _presenter.LogInfo("System info:");
            // _presenter.LogInfo($"Operating system: {Environment.OSVersion}({ComputerInfo.OSFullName})");
            _presenter.LogInfo($"Operating system: {Environment.OSVersion}");
            _presenter.LogInfo($"Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}");
            _presenter.LogInfo($"Java path: {JavaUtils.GetJavaInstallationPath()}");
            _presenter.LogInfo("==============");
        }

        private void LoadConfiguration() {
            chbEnableGameLogging.Checked = _applicationContext.Configuration.EnableGameLogging;
            chbUseLogPrefix.Checked = _applicationContext.Configuration.ShowGamePrefix;
            chbCloseOutput.Checked = _applicationContext.Configuration.CloseTabAfterSuccessfulExitCode;
            txtInstallationDir.Text = _applicationContext.Configuration.InstallationDirectory;
        }

        private void LoadProfilesList() {
            cbProfiles.Items.Clear();
            cbProfiles.Items.AddRange(_presenter.ProfileManager.Profiles.Select(p => p.Key).ToArray());
            cbProfiles.SelectedItem = _presenter.ProfileManager.LastUsedProfile;
        }

        private void DisableControls() {
            BlockControls(true);
        }

        private void EnableControls() {
            BlockControls(false);
        }

        private void BlockControls(bool value) {
            btnLaunch.Enabled = !value;
            cbProfiles.Enabled = !value;
            // DeleteProfileButton.Enabled = !value && (_presenter.ProfileManager.Profiles.Count > 1);
            btnEditProfile.Enabled = !value;
            btnAddProfile.Enabled = !value;
            cbUsers.Enabled = !value;
            btnUsers.Enabled = !value;
        }

        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbProfiles.SelectedItem == null) {
                return;
            }

            _presenter.SelectProfile(cbProfiles.SelectedItem.ToString());
            lblSelectedVersion.Text = _presenter.GetVersionLabel();
            // frmLauncher.profilesDropDownBox.SelectedItem = frmLauncher.profilesDropDownBox.FindItemExact(cbProfiles.SelectedItem.ToString(), true);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            SaveConfiguration();
            frmGameProcess.Close();
        }

        public void LogDebug(string text, string methodName) {
            Log("DEBUG", text, methodName);
        }

        public void LogError(string text, string methodName) {
            Log("ERROR", text, methodName);
        }

        public void LogInfo(string text, string methodName) {
            Log("INFO", text, methodName);
        }

        private void Log(string level, string text, string methodName) {
            if (txtLog.InvokeRequired) {
                txtLog.Invoke(new Action<string, string, string>(Log), level, text, new StackFrame(1).GetMethod().Name);
            }
            else {
                txtLog.AppendText(string.Format(
                    string.IsNullOrEmpty(txtLog.Text) ? "[{0}] [{1}] [{2}] {3}" : "\n[{0}] [{1}] [{2}] {3}",
                    DateTime.Now.ToString("dd-MM-yy HH:mm:ss"), level,
                    methodName ?? new StackFrame(1, false).GetMethod().Name, text));
            }
        }

        private void txtLog_TextChanged(object sender, EventArgs e) {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e) {
            SaveConfiguration();
        }

        private void SaveConfiguration() {
            _applicationContext.Configuration.EnableGameLogging = chbEnableGameLogging.Checked;
            _applicationContext.Configuration.ShowGamePrefix = chbUseLogPrefix.Checked;
            _applicationContext.Configuration.CloseTabAfterSuccessfulExitCode = chbCloseOutput.Checked;
            _applicationContext.Configuration.InstallationDirectory = txtInstallationDir.Text;
            _applicationContext.SaveConfiguration();
        }

        public void UpdateStageText(string text, string methodName = null) {
            if (progressBar.InvokeRequired) {
                progressBar.Invoke(new Action<string, string>(UpdateStageText), text, methodName);
            }
            else {
                // progressBar.Text = text;
                // TODO: need custom progress bar; https://stackoverflow.com/questions/3529928/how-do-i-put-text-on-progressbar
                _presenter.LogInfo(text, methodName);
            }
        }

        public void SetProgressVisibility(bool b) {
            if (progressBar.InvokeRequired) {
                progressBar.Invoke(new Action<bool>(SetProgressVisibility), b);
            }
            else {
                progressBar.Visible = b;
            }
        }

        public void IncProgressValue() {
            SetProgressValue(progressBar.Value + 1);
        }

        public void SetProgressValue(int value) {
            if (progressBar.InvokeRequired) {
                progressBar.Invoke(new Action<int>(SetProgressValue), value);
            }
            else {
                progressBar.Value = value;
            }
        }

        public void SetMaxProgressValue(int value) {
            if (progressBar.InvokeRequired) {
                progressBar.Invoke(new Action<int>(SetMaxProgressValue), value);
            }
            else {
                progressBar.Maximum = value;
            }
        }

        // TODO: Текстовая вкладка с редактированием Json?
        private void btnAddProfile_Click(object sender, EventArgs e) {
            Profile editedProfile = Profile.ParseProfile(_presenter.SelectedProfile.ToString());
            editedProfile.ProfileName = "Copy of '" + _presenter.SelectedProfile.ProfileName + "'(" +
                                        DateTime.Now.ToString("HH:mm:ss") + ")";
            var pf = new ProfileForm(editedProfile, _applicationContext, _localization) { 
                Text = _localization.AddingProfileTitle 
            };
            pf.ShowDialog();
            if (pf.DialogResult == DialogResult.OK) {
                if (_presenter.ProfileManager.Profiles.ContainsKey(editedProfile.ProfileName)) {
                    MessageBox.Show(_localization.ProfileAlreadyExistsErrorText, _localization.Error,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _presenter.ProfileManager.Profiles.Add(editedProfile.ProfileName, editedProfile);
                _presenter.ProfileManager.LastUsedProfile = pf.CurrentProfile.ProfileName;
            }

            _presenter.SaveProfiles();
            UpdateProfileList();
        }

        private void btnEditProfile_Click(object sender, EventArgs e) {
            ProfileForm pf = new ProfileForm(_presenter.SelectedProfile, _applicationContext, _localization) {
                Text = _localization.EditingProfileTitle
            };
            pf.ShowDialog();
            if (pf.DialogResult == DialogResult.OK) {
                _presenter.ProfileManager.Profiles.Remove(_presenter.ProfileManager.LastUsedProfile);
                if (_presenter.ProfileManager.Profiles.ContainsKey(pf.CurrentProfile.ProfileName)) {
                    MessageBox.Show(_localization.ProfileAlreadyExistsErrorText, _localization.Error,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateProfileList();
                    return;
                }

                _presenter.ProfileManager.Profiles.Add(pf.CurrentProfile.ProfileName, pf.CurrentProfile);
                _presenter.ProfileManager.LastUsedProfile = pf.CurrentProfile.ProfileName;
            }

            _presenter.SaveProfiles();
            UpdateProfileList();
        }

        private void UpdateProfileList() {
            _presenter.ReloadProfileManager();
            // DeleteProfileButton.Enabled = _presenter.ProfileManager.Profiles.Count > 1;
            LoadProfilesList();
        }

        //private void DeleteProfileButton_Click(object sender, EventArgs e) {
        //    DialogResult dr =
        //        RadMessageBox.Show(
        //            string.Format(_applicationContext.ProgramLocalization.ProfileDeleteConfirmationText,
        //                _presenter.ProfileManager.LastUsedProfile), _applicationContext.ProgramLocalization.DeleteConfirmationTitle,
        //            MessageBoxButtons.YesNo, RadMessageIcon.Question);
        //    if (dr != DialogResult.Yes) {
        //        return;
        //    }

        //    _presenter.ProfileManager.Profiles.Remove(_presenter.ProfileManager.LastUsedProfile);
        //    _presenter.ProfileManager.LastUsedProfile = _presenter.ProfileManager.Profiles.FirstOrDefault().Key;
        //    _presenter.SaveProfiles();
        //    UpdateProfileList();
        //}

        private void btnUsers_Click(object sender, EventArgs e) {
            new UsersForm(_applicationContext, _localization).ShowDialog();
            _presenter.ReloadUserManager();
            LoadUsersList(_presenter.UserManager);
        }

        private void cbUsers_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbUsers.SelectedItem == null) {
                return;
            }

            _presenter.SelectUser(cbUsers.SelectedItem.ToString());
        }

        public void btnLaunch_Click(object sender, EventArgs e) {
            if (_presenter.SelectedUser == null) {
                _presenter.LogError("Пользователь не выбран");
                return;
            }

            DisableControls();
            var bgw = new BackgroundWorker();
            bgw.DoWork += (o, args) => {
                SetProgressVisibility(true);
                _presenter.CheckVersionAvailability();
                _presenter.CheckLibraries();
                _presenter.CheckGameResources();
                SetProgressVisibility(false);
            };
            bgw.RunWorkerCompleted += (o, args) => {
                // запуск в UI потоке
                Launch();
                EnableControls();
            };
            bgw.RunWorkerAsync();
        }

        private void Launch() {
            var selectedVersion = Version.ParseVersion(
                new DirectoryInfo(_applicationContext.McVersions + _presenter.SelectedProfile.GetSelectedVersion(_applicationContext)));

            if (_presenter.SelectedProfile.FastConnectionSettigs != null) {
                selectedVersion.ArgumentCollection.Add("server", _presenter.SelectedProfile.FastConnectionSettigs.ServerIP);
                selectedVersion.ArgumentCollection.Add("port", _presenter.SelectedProfile.FastConnectionSettigs.ServerPort.ToString());
            }

            if (_presenter.SelectedProfile.WorkingDirectory != null && !Directory.Exists(_presenter.SelectedProfile.WorkingDirectory)) {
                Directory.CreateDirectory(_presenter.SelectedProfile.WorkingDirectory);
            }

            var proc = ProcessInfoBuilder.Create(_applicationContext)
                .Profile(_presenter.SelectedProfile)
                .User(_presenter.SelectedUser)
                .Version(selectedVersion)
                .Build();

            _presenter.LogInfo($"Command line: \"{proc.FileName}\" {proc.Arguments}");
            _presenter.LogInfo($"Version {selectedVersion.VersionId} successfuly launched.");

            if (frmGameProcess.Visible == false) {
                frmGameProcess.Show();
            }

            frmGameProcess.CreateMinecraftProcessPage(_presenter.SelectedProfile, proc).Launch();
        }

        private void LoadUsersList(UserManager userManager) {
            cbUsers.Items.Clear();
            if (userManager.Accounts.Count == 0) {
                return;
            }

            cbUsers.Items.AddRange(userManager.Accounts.Keys.ToArray());
            var itemIndex = cbUsers.FindStringExact(userManager.SelectedUsername);
            if (itemIndex == -1) {
                cbUsers.SelectedIndex = 0;
            }
            else {
                cbUsers.SelectedIndex = itemIndex;
            }
        }
    }
}
