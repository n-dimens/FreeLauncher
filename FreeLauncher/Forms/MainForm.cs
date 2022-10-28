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

    using dotMCLauncher.Core;

    using Microsoft.VisualBasic.Devices;

    public partial class MainForm : Form, ILauncherLogger, IProgressView {
        private readonly FreeLauncher.ApplicationContext _applicationContext;
        private readonly LauncherFormPresenter _presenter;
        private readonly LauncherForm frmLauncher;

        public MainForm(FreeLauncher.ApplicationContext appContext) {
            _applicationContext = appContext;
            _presenter = new LauncherFormPresenter(this, this, _applicationContext);
            InitializeComponent();
            PrintAppInfo();
            frmLauncher = new LauncherForm(_presenter);
            frmLauncher.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            frmLauncher.Show();

            chbEnableGameLogging.Checked = appContext.Configuration.EnableGameLogging;
            chbUseLogPrefix.Checked = appContext.Configuration.ShowGamePrefix;
            chbCloseOutput.Checked = appContext.Configuration.CloseTabAfterSuccessfulExitCode;

            UpdateProfileList();
        }

        private void PrintAppInfo() {
            _presenter.LogInfo($"Application: {ProductName} v.{ProductVersion}");
            _presenter.LogInfo($"Application language: {_applicationContext.ProgramLocalization.Name}({_applicationContext.ProgramLocalization.LanguageTag})");
            _presenter.LogInfo("==============");
            _presenter.LogInfo("System info:");
            _presenter.LogInfo($"Operating system: {Environment.OSVersion}({new ComputerInfo().OSFullName})");
            _presenter.LogInfo($"Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}");
            _presenter.LogInfo($"Java path: \"{Java.JavaInstallationPath}\" ({Java.JavaBitInstallation}-bit)");
            _presenter.LogInfo("==============");
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
            // NicknameDropDownList.Enabled = !value;
        }

        private void btnLaunch_Click(object sender, EventArgs e) {
            frmLauncher.LaunchButton.PerformClick();
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
            frmLauncher.Close();
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
            ProfileForm pf = new ProfileForm(editedProfile, _applicationContext) { Text = _applicationContext.ProgramLocalization.AddingProfileTitle };
            pf.ShowDialog();
            if (pf.DialogResult == DialogResult.OK) {
                if (_presenter.ProfileManager.Profiles.ContainsKey(editedProfile.ProfileName)) {
                    MessageBox.Show(_applicationContext.ProgramLocalization.ProfileAlreadyExistsErrorText,
                        _applicationContext.ProgramLocalization.Error,
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
            ProfileForm pf = new ProfileForm(_presenter.SelectedProfile, _applicationContext) {
                Text = _applicationContext.ProgramLocalization.EditingProfileTitle
            };
            pf.ShowDialog();
            if (pf.DialogResult == DialogResult.OK) {
                _presenter.ProfileManager.Profiles.Remove(_presenter.ProfileManager.LastUsedProfile);
                if (_presenter.ProfileManager.Profiles.ContainsKey(pf.CurrentProfile.ProfileName)) {
                    MessageBox.Show(_applicationContext.ProgramLocalization.ProfileAlreadyExistsErrorText,
                        _applicationContext.ProgramLocalization.Error,
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
    }
}
