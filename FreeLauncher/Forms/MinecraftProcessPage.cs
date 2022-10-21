using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using dotMCLauncher.Core;
using Telerik.WinControls.UI;

namespace FreeLauncher.Forms {
    /// <summary>
    /// Вкладка для управления процессом.
    /// </summary>
    // TODO: Потенциально вкладка подходит для произвольного процесса, возможно абстрагировать от Minecraft
    internal sealed class MinecraftProcessPage {
        private static Thread _outputReader, _errorReader;
        private readonly MinecraftProcessPageContext _context;
        private readonly Process _minecraftProcess;
        private RadPageViewPage _viewPage;
        private RichTextBox _gameLoggingBox;
        private RadButton _closePageButton;
        private RadButton _killProcessButton;
        public event EventHandler<RadPageViewPage> PageCreated;
        public event EventHandler<RadPageViewPage> PageClosed;
        public event EventHandler ProcessLaunched;
        public event EventHandler ProcessExited;

        public MinecraftProcessPage(Process minecraftProcess, MinecraftProcessPageContext context) {
            _context = context;
            _minecraftProcess = minecraftProcess;
        }

        public void Launch() {
            if (_context.Profile.LauncherVisibilityOnGameClose != Profile.LauncherVisibility.CLOSED) {
                if (_context.IsMinecraftLoggingEnabled) {
                    _outputReader = new Thread(o_reader);
                    _outputReader.Start();
                }

                _errorReader = new Thread(e_reader);
                _errorReader.Start();
                AddNewPage();
                _minecraftProcess.Exited += MinecraftProcess_Exited;
            }

            _minecraftProcess.Start();
            OnProcessLaunched();
        }
        
        private void AddNewPage() {
            _viewPage = new RadPageViewPage {
                Text =
                    string.Format("{0} ({1})", _context.ApplicationContext.ProgramLocalization.GameOutput, _context.Profile.ProfileName)
            };
            
            var panel = new RadPanel {
                Text = _context.Profile.GetSelectedVersion(_context.ApplicationContext),
                Dock = DockStyle.Top,
            };
            panel.Size = new Size(panel.Size.Width, 60);
            
            _closePageButton = new RadButton {
                Text = _context.ApplicationContext.ProgramLocalization.Close,
                Anchor = (AnchorStyles.Right | AnchorStyles.Top),
                Enabled = false
            };
            _closePageButton.Location = new Point(panel.Size.Width - (_closePageButton.Size.Width + 5), 5);
            _closePageButton.Click += ClosePageButtonOnClick;
            
            _killProcessButton = new RadButton {
                Text = _context.ApplicationContext.ProgramLocalization.KillProcess,
                Anchor = (AnchorStyles.Right | AnchorStyles.Top)
            };
            _killProcessButton.Click += KillProcessButton_Click;
            _killProcessButton.Location = new Point(panel.Size.Width - (_killProcessButton.Size.Width + 5),
                _closePageButton.Location.Y + _closePageButton.Size.Height + 5);

            _gameLoggingBox = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true };
            _gameLoggingBox.LinkClicked += (sender, e) => Process.Start(e.LinkText);
            
            panel.Controls.Add(_closePageButton);
            panel.Controls.Add(_killProcessButton);
            
            _viewPage.Controls.Add(_gameLoggingBox);
            _viewPage.Controls.Add(panel);
            
            OnPageCreated(_viewPage);
        }

        private void ClosePageButtonOnClick(object sender, EventArgs e) {
            OnPageClosed(_viewPage);
        }

        private void KillProcessButton_Click(object sender, EventArgs e) {
            _minecraftProcess.Kill();
        }

        private void MinecraftProcess_Exited(object sender, EventArgs e) {
            OnProcessExited();
           
            _outputReader?.Abort();
            _errorReader.Abort();
            AppendLog(GetExitLogMessage(), false);
            _context.LauncherForm.Invoke((MethodInvoker) delegate {
                _closePageButton.Enabled = true;
                if (_context.IsAutoClosePage && (_minecraftProcess.ExitCode == 0 || _minecraftProcess.ExitCode == -1)) {
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
                string line = (_context.IsUseGamePrefix ? "[GAME]" : string.Empty) + text + "\n";
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

        private void OnPageCreated(RadPageViewPage e) {
            PageCreated?.Invoke(this, e);
        }

        private void OnPageClosed(RadPageViewPage e) {
            PageClosed?.Invoke(this, e);
        }

        private void OnProcessLaunched() {
            ProcessLaunched?.Invoke(this, EventArgs.Empty);
        }

        private void OnProcessExited() {
            ProcessExited?.Invoke(this, EventArgs.Empty);
        }
    }
}