namespace NDimens.Minecraft.FreeLauncher; 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using dotMCLauncher.Core;

public partial class UserManagerForm : Form {
    private readonly UsersRepository _usersRepository;
    private readonly UserManager _userManager;

    public UserManagerForm(GameFileStructure gameFiles) {
        _usersRepository = new UsersRepository(gameFiles);
        InitializeComponent();
        _userManager = _usersRepository.Read();
        UpdateUsers();
    }

    private void btnAdd_Click(object sender, EventArgs e) {
        var user = new User {
            Username = txtUsername.Text,
            Type = "offline"
        };
        _userManager.AddUser(user);
        _userManager.SelectedUsername = user.Username;
        _usersRepository.Save(_userManager);
        UpdateUsers();
    }

    private void UpdateUsers() {
        lbUsers.Items.Clear();
        foreach (KeyValuePair<string, User> item in _userManager.Users) {
            lbUsers.Items.Add(item.Key);
        }
    }

    private void btnDelete_Click(object sender, EventArgs e) {
        _userManager.Users.Remove(lbUsers.SelectedItem.ToString());
        _usersRepository.Save(_userManager);
        UpdateUsers();
    }

    private void txtUsername_TextChanged(object sender, EventArgs e) {
        btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtUsername.Text);
    }

    private void lbUsers_SelectedIndexChanged(object sender, EventArgs e) {
        btnDelete.Enabled = lbUsers.SelectedItem != null;
    }
}
