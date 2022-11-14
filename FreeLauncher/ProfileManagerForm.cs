namespace Launcher.Forms;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NDimens.Minecraft.FreeLauncher.Presenters;

public partial class ProfileManagerForm : Form {
    private readonly CreateProfileFormPresenter _presenter;

    internal ProfileManagerForm(CreateProfileFormPresenter presenter) {
        _presenter = presenter;
        InitializeComponent();
        LoadVersionList();
        txtProfileName.Text = _presenter.Profile.ProfileName;
        txtFolder.Text = _presenter.Profile.WorkingDirectory;
        if (_presenter.Profile.SelectedVersion != null) {
            cbVersions.SelectedIndex = cbVersions.Items.IndexOf(_presenter.Profile.SelectedVersion);
        }
        else {
            cbVersions.SelectedIndex = 0;
        }
    }

    private void btnSave_Click(object sender, EventArgs e) {
        _presenter.Profile.ProfileName = txtProfileName.Text;
        _presenter.Profile.WorkingDirectory = txtFolder.Text;
        _presenter.Profile.SelectedVersion = cbVersions.SelectedItem.ToString();
        var error = _presenter.Save();
        if (!string.IsNullOrEmpty(error)) {
            MessageBox.Show(error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else {
            DialogResult = DialogResult.OK;
        }
    }

    private void LoadVersionList() {
        cbVersions.Items.Clear();
        foreach (var v in _presenter.VersionList) {
            cbVersions.Items.Add(v);
        }
    }

    private void btnOpenFolder_Click(object sender, EventArgs e) {
        // TODO: Linux ?
        Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", txtFolder.Text);
    }
}
