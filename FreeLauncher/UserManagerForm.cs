namespace NDimens.Minecraft.FreeLauncher; 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NDimens.Minecraft.FreeLauncher.Core.Data;
using NDimens.Minecraft.FreeLauncher.Presenters;

public partial class UserManagerForm : Form {
    private readonly UserManagerFormPresenter _presenter;

    internal UserManagerForm(UserManagerFormPresenter presenter) {
        _presenter = presenter;
        InitializeComponent();
        ReloadUserList();
    }

    private void btnAdd_Click(object sender, EventArgs e) {
        _presenter.AddUser(txtUsername.Text);
        ReloadUserList();
    }

    private void btnDelete_Click(object sender, EventArgs e) {
        _presenter.DeleteUser(lbUsers.SelectedItem.ToString());
        ReloadUserList();
    }

    private void txtUsername_TextChanged(object sender, EventArgs e) {
        btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtUsername.Text);
    }

    private void lbUsers_SelectedIndexChanged(object sender, EventArgs e) {
        btnDelete.Enabled = lbUsers.SelectedItem != null;
    }

    private void ReloadUserList() {
        lbUsers.Items.Clear();
        foreach (var username in _presenter.GetUserNames()) {
            lbUsers.Items.Add(username);
        }
    }
}
