using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using dotMCLauncher.Core;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Data;
using Version = dotMCLauncher.Core.Version;

namespace FreeLauncher.Forms {
    public partial class LauncherForm : RadForm {
        private readonly LauncherFormPresenter _presenter;
        private readonly ApplicationContext _applicationContext;
      
        public LauncherForm(LauncherFormPresenter presenter) {
            _presenter = presenter;
            _applicationContext = presenter.AppContext;
            InitializeComponent();
            Text = ProductName + " " + ProductVersion;      
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

        public void LaunchButton_Click(object sender, EventArgs e) {
            if (_presenter.SelectedUser == null) {
                _presenter.LogError("Пользователь не выбран");
                return;
            }

            // DisableControls();
            var bgw = new BackgroundWorker();
            bgw.DoWork += (o, args) => {
                // SetProgressVisibility(true);
                _presenter.CheckVersionAvailability();
                _presenter.CheckLibraries();
                _presenter.CheckGameResources();
                // SetProgressVisibility(false);
            };
            bgw.RunWorkerCompleted += (o, args) => {
                // запуск в UI потоке
                Launch();
                // EnableControls();
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

            CreateMinecraftProcessPage(proc).Launch();
        }

        /// <summary>
        /// Создание вкладки для нового процесса Minecraft
        /// </summary>
        private MinecraftProcessPage CreateMinecraftProcessPage(ProcessStartInfo processInfo) {
            var context = new MinecraftProcessPageContext(this, _presenter.SelectedProfile, _applicationContext);
            var mcProcessPage = new MinecraftProcessPage(new Process {StartInfo = processInfo, EnableRaisingEvents = true}, context);
            mcProcessPage.PageCreated += (o, page) => {
                mainPageView.Pages.Add(page);
                mainPageView.SelectedPage = page;
            };
            mcProcessPage.PageClosed += (o, page) => { mainPageView.Pages.Remove(page); };
            mcProcessPage.ProcessLaunched += (o, args) => {
                if (_presenter.SelectedProfile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.CLOSED) {
                    Close();
                }

                if (_presenter.SelectedProfile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.HIDDEN) {
                    Hide();
                }
            };
            mcProcessPage.ProcessExited += (o, args) => {
                if (_presenter.SelectedProfile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.HIDDEN) {
                    Invoke((MethodInvoker) Show);
                }
            };
            return mcProcessPage;
        }

        private void UpdateProfileList() {
            _presenter.ReloadProfileManager();
            // DeleteProfileButton.Enabled = _presenter.ProfileManager.Profiles.Count > 1;
        }
    }
}