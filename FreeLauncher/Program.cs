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
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        //ApplicationConfiguration.Initialize();
        //Application.Run(new Form1());
        var gameFiles = new GameFileStructure();
        var localization = new Localization();
        ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark";
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        var frmMain = new MainForm(gameFiles, localization);
        Application.Run(frmMain);
    }
}