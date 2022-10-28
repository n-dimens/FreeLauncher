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

        private void DisableControls() {
            BlockControls = true;
        }
        
        private void EnableControls() {
            BlockControls = false;
        }
        
        private bool BlockControls {
            set {
                LaunchButton.Enabled = !value;
                DeleteProfileButton.Enabled = !value && (_presenter.ProfileManager.Profiles.Count > 1);
                NicknameDropDownList.Enabled = !value;
            }
        }
       
        public LauncherForm(LauncherFormPresenter presenter) {
            _presenter = presenter;
            _applicationContext = presenter.AppContext;
            InitializeComponent();
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

        private void NicknameDropDownList_SelectedIndexChanged(object sender, PositionChangedEventArgs e) {
            if (NicknameDropDownList.SelectedItem == null) {
                return;
            }

            _presenter.SelectUser(NicknameDropDownList.SelectedItem.Text);
            _presenter.SaveUsers();
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
                // SetProgressVisibility(true);
                _presenter.CheckVersionAvailability();
                _presenter.CheckLibraries();
                _presenter.CheckGameResources();
                // SetProgressVisibility(false);
            };
            bgw.RunWorkerCompleted += (o, args) => {
                // запуск в UI потоке
                Launch();
                EnableControls();
            };
            bgw.RunWorkerAsync();
        }
        
        private void Launch() {
            _presenter.SelectUserForLaunch(NicknameDropDownList.Text);
            _presenter.SaveUsers();
            _presenter.ReloadUserManager();
            UpdateNicknameDropDownList(_presenter.UserManager);
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
            DeleteProfileButton.Enabled = _presenter.ProfileManager.Profiles.Count > 1;
        }

        private void UpdateNicknameDropDownList(UserManager userManager) {
            NicknameDropDownList.Items.Clear();
            NicknameDropDownList.Items.AddRange(userManager.Accounts.Keys);
            NicknameDropDownList.SelectedItem = NicknameDropDownList.FindItemExact(userManager.SelectedUsername, true);
        }
    }
}