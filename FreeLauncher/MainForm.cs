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
using NDimens.Minecraft.FreeLauncher.Core;

public partial class MainForm : Form, IProgressView {
    private readonly FormFactory _formFactory;
    private readonly ILauncherLogger _logger;
    private readonly Localization _localization;
    private readonly GameFileStructure _gameFiles;
    private readonly MainFormPresenter _presenter;

    internal MainForm(
        MainFormPresenter presenter,
        FormFactory formFactory,
        GameFileStructure appContext, 
        Localization localization, 
        ILauncherLogger logger) {
        _formFactory = formFactory;
        _gameFiles = appContext;
        _localization = localization;
        _logger = logger;
        _presenter = presenter;
        _presenter.SetProgressView(this);
        logger.Changed += Logger_Changed;
        InitializeComponent();

        PrintAppInfo();           

        LoadConfiguration();
        LoadProfilesList(_presenter.GetProfileManager());
        LoadUsersList(_presenter.GetUserManager());
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
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
        SaveConfiguration();
        _presenter.SaveState();
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

    private void btnAddProfile_Click(object sender, EventArgs e) {
        var pf = _formFactory.CreateNewProfileManagerForm();
        pf.Text = _localization.AddingProfileTitle;
        pf.ShowDialog();
        if (pf.DialogResult == DialogResult.OK) {
            LoadProfilesList(_presenter.GetProfileManager());
        }
    }

    private void btnEditProfile_Click(object sender, EventArgs e) {
        var pf = _formFactory.CreateEditProfileManagerForm(_presenter.SelectedProfile);
        pf.Text = _localization.EditingProfileTitle;
        pf.ShowDialog();
        if (pf.DialogResult == DialogResult.OK) {
            LoadProfilesList(_presenter.GetProfileManager());
        }
    }

    private void btnUsers_Click(object sender, EventArgs e) {
        _formFactory.CreateUserManagerForm().ShowDialog();
        LoadUsersList(_presenter.GetUserManager());
    }

    private void cbUsers_SelectedIndexChanged(object sender, EventArgs e) {
        if (cbUsers.SelectedItem == null) {
            return;
        }

        _presenter.SelectUser(cbUsers.SelectedItem.ToString());
    }

    public void btnLaunch_Click(object sender, EventArgs e) {
        if (cbUsers.SelectedItem == null) {
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
            Launch(cbUsers.SelectedItem.ToString());            
            EnableControls();
        };
        bgw.RunWorkerAsync();
    }

    private void Launch(string selectedUserName) {
        var selectedVersion = Version.ParseVersion(
            new DirectoryInfo(_presenter.GameFiles.McVersions + _presenter.SelectedProfile.SelectedVersion));

        if (_presenter.SelectedProfile.WorkingDirectory != null && !Directory.Exists(_presenter.SelectedProfile.WorkingDirectory)) {
            Directory.CreateDirectory(_presenter.SelectedProfile.WorkingDirectory);
        }

        var proc = ProcessInfoBuilder.Create(_presenter.GameFiles)
            .Profile(_presenter.SelectedProfile)
            .User(_presenter.GetUser(selectedUserName))
            .Version(selectedVersion)
            .Build();

        _logger.Info($"Command line: \"{proc.FileName}\" {proc.Arguments}");
        _logger.Info($"Version {selectedVersion.Id} successfuly launched.");

        GameSessionForm.Launch(_presenter.GameFiles, _presenter.SelectedProfile, proc);
    }

    private void LoadProfilesList(ProfileManager pm) {
        cbProfiles.Items.Clear();
        if (pm.Profiles.Count == 0) {
            return;
        }

        cbProfiles.Items.AddRange(pm.Profiles.Keys.ToArray());
        var itemIndex = cbProfiles.FindStringExact(pm.LastUsedProfile);
        if (itemIndex == -1) {
            cbProfiles.SelectedIndex = 0;
        }
        else {
            cbProfiles.SelectedIndex = itemIndex;
        }
    }

    // TODO: Почему разная логика?
    private void LoadUsersList(UserManager um) {
        cbUsers.Items.Clear();
        if (um.Users.Count == 0) {
            return;
        }

        cbUsers.Items.AddRange(um.Users.Keys.ToArray());
        var itemIndex = cbUsers.FindStringExact(um.LastUserName);
        if (itemIndex == -1) {
            cbUsers.SelectedIndex = 0;
        }
        else {
            cbUsers.SelectedIndex = itemIndex;
        }
    }
}
