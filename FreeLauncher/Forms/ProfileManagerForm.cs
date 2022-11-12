namespace Launcher.Forms {
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

    using dotMCLauncher.Core;

    public partial class ProfileManagerForm : Form {
        public Profile CurrentProfile;

        public ProfileManagerForm(Profile profile) {
            CurrentProfile = profile;
            InitializeComponent();
            LoadVersionList();

            txtProfileName.Text = CurrentProfile.ProfileName;
            txtFolder.Text = CurrentProfile.WorkingDirectory;
            if (CurrentProfile.SelectedVersion != null) {
                cbVersions.SelectedIndex = cbVersions.Items.IndexOf(CurrentProfile.SelectedVersion);
            }
            else {
                cbVersions.SelectedIndex = 0;
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            CurrentProfile.ProfileName = txtProfileName.Text;
            CurrentProfile.WorkingDirectory = txtFolder.Text;
            CurrentProfile.SelectedVersion = cbVersions.SelectedItem.ToString();
        }

        private void LoadVersionList() {
            cbVersions.Items.Clear();
            cbVersions.Items.Add("1.7.10");
            cbVersions.Items.Add("1.7.10-Forge10.13.4.1614-1.7.10");
        }

        private void btnOpenFolder_Click(object sender, EventArgs e) {
            Process.Start(txtFolder.Text);
        }
    }
}
