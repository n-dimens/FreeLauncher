using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

using Core = dotMCLauncher.Core;

using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Data;
using dotMCLauncher.Core;
using dotMCLauncher.Core.Auth;

namespace FreeLauncher.Forms {
    public partial class UsersForm : RadForm {
        private readonly Localization _localization;
        private readonly UsersRepository _usersRepository;
        private readonly UserManager _userManager;

        public UsersForm(Core.GameFileStructure appContext, Localization localization) {
            _localization = localization;
            _usersRepository = new UsersRepository(appContext);
            InitializeComponent();
            LoadLocalization();
            _userManager = _usersRepository.Read();
            UpdateUsers();
        }

        private void IsLicense_ValueChanged(object sender, EventArgs e) {
            PasswordTextBox.Enabled = swIsLicense.Value;
            AddUserButton.Enabled = AllowAddUser(UsernameTextBox.Text, PasswordTextBox.Text, swIsLicense.Value);
        }

        private void AddUserButton_Click(object sender, EventArgs e) {
            if (!swIsLicense.Value) {
                User user = new User {
                    Username = UsernameTextBox.Text,
                    Type = "offline"
                };
                _userManager.AddUser(user);
                _userManager.SelectedUsername = user.Username;
                _usersRepository.Save(_userManager);
                UpdateUsers();
                return;
            }

            DisableControls();
            AddUserButton.Text = _localization.PleaseWait;
            BackgroundWorker bgw = new BackgroundWorker();
            // TODO: Task ?
            bgw.DoWork += delegate {
                var user = AuthService.Login(UsernameTextBox.Text, PasswordTextBox.Text);
                if (user != null) {
                    _userManager.AddUser(user);
                    _userManager.SelectedUsername = user.Username;
                }
                else {
                    RadMessageBox.Show(_localization.IncorrectLoginOrPassword, _localization.Error, MessageBoxButtons.OK,
                                RadMessageIcon.Error);
                }

            };
            bgw.RunWorkerCompleted += delegate {
                _usersRepository.Save(_userManager);
                UpdateUsers();
                UsernameTextBox.Clear();
                PasswordTextBox.Clear();
                EnableControls();
                AddUserButton.Text = _localization.AddNewUserButton;
                IsLicense_ValueChanged(this, EventArgs.Empty);
            };
            bgw.RunWorkerAsync();
        }

        private void DisableControls() {
            AddUserButton.Enabled = false;
            UsernameTextBox.Enabled = false;
            PasswordTextBox.Enabled = false;
            swIsLicense.Enabled = false;
            ControlBox = false;
        }

        private void EnableControls() {
            UsernameTextBox.Enabled = true;
            swIsLicense.Enabled = true;
            ControlBox = true;
        }

        private void DeleteUserButton_Click(object sender, EventArgs e) {
            _userManager.Users.Remove(UsersListControl.SelectedItem.Tag.ToString());
            _usersRepository.Save(_userManager);
            UpdateUsers();
        }

        private void UpdateUsers() {
            UsersListControl.Items.Clear();
            foreach (KeyValuePair<string, User> item in _userManager.Users) {
                UsersListControl.Items.Add(new RadListDataItem($"{item.Key} [{_userManager.Users[item.Key].Type}]") {
                    Tag = item.Key
                });
            }
        }

        private void UsersListControl_SelectedIndexChanged(object sender, PositionChangedEventArgs e) {
            DeleteUserButton.Enabled = UsersListControl.SelectedItem != null;
        }

        private void UsernameTextBox_TextChanged(object sender, EventArgs e) {
            AddUserButton.Enabled = AllowAddUser(UsernameTextBox.Text, PasswordTextBox.Text, swIsLicense.Value);
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e) {
            AddUserButton.Enabled = AllowAddUser(UsernameTextBox.Text, PasswordTextBox.Text, swIsLicense.Value);
        }

        private bool AllowAddUser(string username, string password, bool isLicense) {
            if (string.IsNullOrWhiteSpace(username)) {
                return false;
            }

            if (!isLicense) {
                return true;
            }

            if (isLicense) {
                return !string.IsNullOrWhiteSpace(password);
            }

            return false;
        }

        private void LoadLocalization() {
            DeleteUserButton.Text = _localization.RemoveSelectedUser;
            AddNewUserBox.Text = _localization.AddNewUserBox;
            NicknameLabel.Text = _localization.Nickname;
            LicenseQuestionLabel.Text = _localization.LicenseQuestion;
            PasswordLabel.Text = _localization.Password;
            AddUserButton.Text = _localization.AddNewUserButton;
        }
    }
}
