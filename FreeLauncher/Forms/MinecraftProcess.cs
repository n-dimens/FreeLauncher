using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using dotMCLauncher.Core;

using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace FreeLauncher.Forms {
    internal class MinecraftProcess {
        private readonly LauncherForm _launcherForm;
        private readonly Profile _profile;
        private readonly Process _minecraftProcess;
        private RichTextBox _gameLoggingBox;
        private RadButton _closePageButton, _killProcessButton;
        private static Thread _outputReader, _errorReader;

        public MinecraftProcess(Process minecraftProcess, LauncherForm launcherForm, Profile profile) {
            _launcherForm = launcherForm;
            _profile = profile;
            _minecraftProcess = minecraftProcess;
        }

        public void Launch() {
            if (_profile.LauncherVisibilityOnGameClose != Profile.LauncherVisibility.CLOSED) {
                if (_launcherForm.EnableMinecraftLogging.Checked) {
                    _outputReader = new Thread(o_reader);
                    _outputReader.Start();
                }
                _errorReader = new Thread(e_reader);
                _errorReader.Start();
                object[] obj = _launcherForm.AddNewPage();
                _gameLoggingBox = (RichTextBox)obj[0];
                _closePageButton = (RadButton)obj[2];
                _killProcessButton = (RadButton)obj[1];
                _killProcessButton.Click += KillProcessButton_Click;
                _minecraftProcess.Exited += MinecraftProcess_Exited;
            }
            _minecraftProcess.Start();
            if (_profile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.CLOSED) {
                _launcherForm.Close();
            }
            if (_profile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.HIDDEN) {
                _launcherForm.Hide();
            }
        }

        private void KillProcessButton_Click(object sender, EventArgs e) {
            _minecraftProcess.Kill();
        }

        private void MinecraftProcess_Exited(object sender, EventArgs e) {
            if (_profile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.HIDDEN) {
                _launcherForm.Invoke((MethodInvoker)(() => _launcherForm.Show()));
            }
            _outputReader?.Abort();
            _errorReader.Abort();
            AppendLog(GetExitLogMessage(), false);
            _launcherForm.Invoke((MethodInvoker)delegate {
                _closePageButton.Enabled = true;
                if (_launcherForm.CloseGameOutput.Checked &&
                    (_minecraftProcess.ExitCode == 0 || _minecraftProcess.ExitCode == -1)) {
                    _closePageButton.PerformClick();
                }
                _killProcessButton.Enabled = false;
            });
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

        private void AppendLog(string text, bool iserror) {
            if (_gameLoggingBox.IsDisposed) {
                return;
            }
            if (_gameLoggingBox.InvokeRequired) {
                _gameLoggingBox.Invoke(new Action<string, bool>(AppendLog), text, iserror);
            }
            else {
                Color color = iserror ? Color.Red : Color.DarkSlateGray;
                string line = (_launcherForm.UseGamePrefix.ToggleState == ToggleState.On ? "[GAME]" : string.Empty) + text + "\n";
                int start = _gameLoggingBox.TextLength;
                _gameLoggingBox.AppendText(line);
                int end = _gameLoggingBox.TextLength;
                _gameLoggingBox.Select(start, end - start);
                _gameLoggingBox.SelectionColor = color;
                _gameLoggingBox.SelectionLength = 0;
                _gameLoggingBox.ScrollToCaret();
            }
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
                if (_gameLoggingBox == null || !_gameLoggingBox.IsDisposed) {
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
                if (_gameLoggingBox == null || !_gameLoggingBox.IsDisposed) {
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
    }
}