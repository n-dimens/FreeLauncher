namespace Launcher;

using System;
using System.Windows.Forms;

using dotMCLauncher.Core;

using FreeLauncher;
using FreeLauncher.Forms;

using Telerik.WinControls;

static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
        var logger = new FileLogger();
        var gameFiles = new GameFileStructure();
        var localization = new Localization();
        ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark";
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        var frmMain = new MainForm(gameFiles, localization, logger);
        Application.Run(frmMain);
    }
}