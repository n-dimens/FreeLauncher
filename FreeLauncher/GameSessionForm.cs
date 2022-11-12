namespace NDimens.Minecraft.FreeLauncher;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using dotMCLauncher.Core;

public partial class GameSessionForm : Form {
    private readonly Profile _profile;
    private readonly GameFileStructure _gameFiles;
    private readonly Process _minecraftProcess;

    public GameSessionForm(GameFileStructure appContext, Profile profile, ProcessStartInfo processInfo) {
        _profile = profile;
        _gameFiles = appContext;
        InitializeComponent();
        Text = ProductName + ": " + _profile.ProfileName;
        _minecraftProcess = new Process { 
            StartInfo = processInfo, 
            EnableRaisingEvents = true 
        };

        _minecraftProcess.StartInfo.RedirectStandardOutput = true;
        _minecraftProcess.OutputDataReceived += _minecraftProcess_OutputDataReceived;
        _minecraftProcess.StartInfo.RedirectStandardError = true;
        _minecraftProcess.ErrorDataReceived += _minecraftProcess_ErrorDataReceived;
        _minecraftProcess.Exited += MinecraftProcess_Exited;
    }

    private void _minecraftProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
        AppendLog(e.Data, true);
    }

    private void _minecraftProcess_OutputDataReceived(object sender, DataReceivedEventArgs e) {
        AppendLog(e.Data, false);
    }

    private void btnKillProcess_Click(object sender, EventArgs e) {
        _minecraftProcess.Kill();
    }

    private void GameSessionForm_Shown(object sender, EventArgs e) {
        _minecraftProcess.Start();
        _minecraftProcess.BeginOutputReadLine();
        _minecraftProcess.BeginErrorReadLine();

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

        AppendLog(GetExitLogMessage(), false);
        Invoke((MethodInvoker)delegate {
            if (_gameFiles.Configuration.CloseTabAfterSuccessfulExitCode && (_minecraftProcess.ExitCode == 0 || _minecraftProcess.ExitCode == -1)) {
                Close();
            }

            btnKillProcess.Enabled = false;
        });
    }

    private void AppendLog(string text, bool iserror) {
        if (txtGameLog.InvokeRequired) {
            txtGameLog.Invoke(new Action<string, bool>(AppendLog), text, iserror);
        }
        else {
            Color color = iserror ? Color.Red : Color.Black;
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
        return exitCode switch {
            0 => "Stable",
            -1 => "Killed",
            _ => "There could be problems",
        };
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
