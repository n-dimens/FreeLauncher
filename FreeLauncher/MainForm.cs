namespace NDimens.Minecraft.FreeLauncher; 

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
using dotMCLauncher.Core.Data;
using Launcher.Forms;
using NDimens.Minecraft.FreeLauncher.Presenters;
using global::FreeLauncher;

public partial class MainForm : Form, IProgressView {
    private readonly ILauncherLogger _logger;
    private readonly Localization _localization;
    private readonly GameFileStructure _gameFiles;
    private readonly MainFormPresenter _presenter;

    public MainForm(GameFileStructure appContext, 
        Localization localization, 
        ILauncherLogger logger,
        VersionsService versionsService) {
        _gameFiles = appContext;
        _localization = localization;
        _logger = logger;
        _presenter = new MainFormPresenter(logger, this, _gameFiles, versionsService);
        logger.Changed += Logger_Changed;
        InitializeComponent();

        PrintAppInfo();           

        LoadConfiguration();

        _presenter.ReloadProfileManager();
        LoadProfilesList();

        _presenter.ReloadUserManager();
        LoadUsersList(_presenter.UserManager);
    }

    private void Logger_Changed(object sender, string message) {
        Log(message);
    }

    private void PrintAppInfo() {
        _logger.Info("=============< New Session >=============");
        _logger.Info($"Application: {ProductName} v.{ProductVersion}");
        _logger.Info($"Application language: {_localization.Name}({_localization.LanguageTag})");
        // _presenter.LogInfo($"Operating system: {Environment.OSVersion}({ComputerInfo.OSFullName})");
        _logger.Info($"Operating system: {Environment.OSVersion}");
        _logger.Info($"Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}");
        _logger.Info($"Java path: {JavaUtils.GetJavaInstallationPath()}");
    }

    private void LoadConfiguration() {
        chbEnableGameLogging.Checked = _gameFiles.Configuration.EnableGameLogging;
        chbUseLogPrefix.Checked = _gameFiles.Configuration.ShowGamePrefix;
        chbCloseOutput.Checked = _gameFiles.Configuration.CloseTabAfterSuccessfulExitCode;
        txtInstallationDir.Text = _gameFiles.Configuration.InstallationDirectory;
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
        // frmGameProcess.Close();
    }

    private void Log(string message) {
        if (txtLog.InvokeRequired) {
            txtLog.Invoke(new Action<string>(Log), message);
        }
        else {
            txtLog.AppendText(message + "\n");
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
        _gameFiles.Configuration.EnableGameLogging = chbEnableGameLogging.Checked;
        _gameFiles.Configuration.ShowGamePrefix = chbUseLogPrefix.Checked;
        _gameFiles.Configuration.CloseTabAfterSuccessfulExitCode = chbCloseOutput.Checked;
        _gameFiles.Configuration.InstallationDirectory = txtInstallationDir.Text;
        _gameFiles.SaveConfiguration();
    }

    public void UpdateStageText(string text, string methodName = null) {
        if (progressBar.InvokeRequired) {
            progressBar.Invoke(new Action<string, string>(UpdateStageText), text, methodName);
        }
        else {
            // progressBar.Text = text;
            // TODO: need custom progress bar; https://stackoverflow.com/questions/3529928/how-do-i-put-text-on-progressbar
            _logger.Info(text, methodName);
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
        var pf = new ProfileManagerForm(editedProfile) { 
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
        var pf = new ProfileManagerForm(_presenter.SelectedProfile) {
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
        new UserManagerForm(_gameFiles).ShowDialog();
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
            _logger.Error("Пользователь не выбран");
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
            new DirectoryInfo(_gameFiles.McVersions + _presenter.SelectedProfile.SelectedVersion));

        if (_presenter.SelectedProfile.WorkingDirectory != null && !Directory.Exists(_presenter.SelectedProfile.WorkingDirectory)) {
            Directory.CreateDirectory(_presenter.SelectedProfile.WorkingDirectory);
        }

        var proc = ProcessInfoBuilder.Create(_gameFiles)
            .Profile(_presenter.SelectedProfile)
            .User(_presenter.SelectedUser)
            .Version(selectedVersion)
            .Build();

        _logger.Info($"Command line: \"{proc.FileName}\" {proc.Arguments}");
        _logger.Info($"Version {selectedVersion.Id} successfuly launched.");

        GameSessionForm.Launch(_presenter.AppContext, _presenter.SelectedProfile, proc);
    }

    private void LoadUsersList(UserManager userManager) {
        cbUsers.Items.Clear();
        if (userManager.Users.Count == 0) {
            return;
        }

        cbUsers.Items.AddRange(userManager.Users.Keys.ToArray());
        var itemIndex = cbUsers.FindStringExact(userManager.SelectedUsername);
        if (itemIndex == -1) {
            cbUsers.SelectedIndex = 0;
        }
        else {
            cbUsers.SelectedIndex = itemIndex;
        }
    }
}
