namespace Launcher.Forms {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using dotMCLauncher.Core;

    public partial class GameSessionForm : Form {
        private Thread _outputReader;
        private Thread _errorReader;
        private readonly Profile _profile;
        private readonly GameFileStructure _gameFiles;
        private readonly Process _minecraftProcess;

        public GameSessionForm(GameFileStructure appContext, Profile profile, ProcessStartInfo processInfo) {
            _profile = profile;
            _gameFiles = appContext;
            InitializeComponent();
            Text = ProductName + ": " + _profile.ProfileName;
            _minecraftProcess = new Process { StartInfo = processInfo, EnableRaisingEvents = true };
        }

        private void btnKillProcess_Click(object sender, EventArgs e) {
            _minecraftProcess.Kill();
        }

        private void GameSessionForm_Shown(object sender, EventArgs e) {
            if (_profile.LauncherVisibilityOnGameClose != Profile.LauncherVisibility.CLOSED) {
                if (_gameFiles.Configuration.EnableGameLogging) {
                    _outputReader = new Thread(o_reader);
                    _outputReader.Start();
                }

                _errorReader = new Thread(e_reader);
                _errorReader.Start();
                _minecraftProcess.Exited += MinecraftProcess_Exited;
            }

            _minecraftProcess.Start();
            if (_profile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.CLOSED) {
                Close();
            }

            if (_profile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.HIDDEN) {
                Hide();
            }
        }

        private void MinecraftProcess_Exited(object sender, EventArgs e) {
            if (_profile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.HIDDEN) {
                Invoke((MethodInvoker)Show);
            }

            _outputReader?.Abort();
            _errorReader.Abort();
            AppendLog(GetExitLogMessage(), false);
            Invoke((MethodInvoker)delegate {
                if (_gameFiles.Configuration.CloseTabAfterSuccessfulExitCode && (_minecraftProcess.ExitCode == 0 || _minecraftProcess.ExitCode == -1)) {
                    Close();
                }

                btnKillProcess.Enabled = false;
            });
        }

        private void o_reader() {
            while (true) {
                while (IsRunning(_minecraftProcess)) {
                    string line = _minecraftProcess.StandardOutput.ReadLine();
                    if (string.IsNullOrEmpty(line)) {
                        continue;
                    }

                    AppendLog(line, false);
                }

                if (txtGameLog == null || !txtGameLog.IsDisposed) {
                    continue;
                }

                _minecraftProcess.EnableRaisingEvents = false;
                _outputReader.Abort();
                _errorReader.Abort();
            }
        }

        private void e_reader() {
            while (true) {
                while (IsRunning(_minecraftProcess)) {
                    string line = _minecraftProcess.StandardError.ReadLine();
                    if (string.IsNullOrEmpty(line)) {
                        continue;
                    }

                    AppendLog(line, true);
                }

                if (txtGameLog == null || !txtGameLog.IsDisposed) {
                    continue;
                }

                _minecraftProcess.EnableRaisingEvents = false;
                _outputReader.Abort();
                _errorReader.Abort();
            }
        }

        private static bool IsRunning(Process process) {
            try {
                Process.GetProcessById(process.Id);
            }
            catch {
                return false;
            }

            return true;
        }

        private void AppendLog(string text, bool iserror) {
            if (txtGameLog.InvokeRequired) {
                txtGameLog.Invoke(new Action<string, bool>(AppendLog), text, iserror);
            }
            else {
                Color color = iserror ? Color.Red : Color.DarkSlateGray;
                string line = (_gameFiles.Configuration.ShowGamePrefix ? "[GAME]" : string.Empty) + text + "\n";
                int start = txtGameLog.TextLength;
                txtGameLog.AppendText(line);
                int end = txtGameLog.TextLength;
                txtGameLog.Select(start, end - start);
                txtGameLog.SelectionColor = color;
                txtGameLog.SelectionLength = 0;
                txtGameLog.ScrollToCaret();
            }
        }

        private string GetExitLogMessage() {
            return string.Format("Process exited with error code {0}({1}). Session since {2}({3} total)",
                _minecraftProcess.ExitCode,
                GetProcessExitDescription(_minecraftProcess.ExitCode),
                _minecraftProcess.StartTime.ToString("HH:mm:ss"),
                GetElapsedTime(_minecraftProcess.StartTime));
        }

        private string GetProcessExitDescription(int exitCode) {
            switch (exitCode) {
                case 0:
                    return "Stable";
                case -1:
                    return "Killed";
                default:
                    return "There could be problems";
            }
        }

        private string GetElapsedTime(DateTime startTime) {
            return Math.Round(startTime.Subtract(DateTime.Now).TotalMinutes, 2)
                .ToString(CultureInfo.InvariantCulture)
                .Replace('-', ' ') + " min.";
        }

        public static void Launch(GameFileStructure appContext, Profile selectedProfile, ProcessStartInfo processInfo) {
            new GameSessionForm(appContext, selectedProfile, processInfo).Show();
        }
    }
}
