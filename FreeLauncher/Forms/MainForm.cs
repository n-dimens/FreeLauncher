namespace FreeLauncher.Forms {
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

    public partial class MainForm : Form, ILauncherLogger {
        private readonly FreeLauncher.ApplicationContext _applicationContext;
        private readonly LauncherFormPresenter _presenter;
        private readonly LauncherForm frmLauncher;

        public MainForm(FreeLauncher.ApplicationContext appContext) {
            _applicationContext = appContext;
            _presenter = new LauncherFormPresenter(this, _applicationContext);
            InitializeComponent();
            frmLauncher = new LauncherForm(_presenter);
            frmLauncher.Show();
            LoadProfilesList();
        }

        private void LoadProfilesList() {
            cbProfiles.Items.Clear();
            cbProfiles.Items.AddRange(frmLauncher.Presenter.ProfileManager.Profiles.Select(p => p.Key).ToArray());
            cbProfiles.SelectedItem = frmLauncher.Presenter.ProfileManager.LastUsedProfile;
        }

        private void btnLaunch_Click(object sender, EventArgs e) {
            frmLauncher.LaunchButton.PerformClick();
        }

        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbProfiles.SelectedItem == null) {
                return;
            }

            frmLauncher.profilesDropDownBox.SelectedItem = frmLauncher.profilesDropDownBox.FindItemExact(cbProfiles.SelectedItem.ToString(), true);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
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
    }
}
