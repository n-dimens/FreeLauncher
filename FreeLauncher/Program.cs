namespace NDimens.Minecraft.FreeLauncher;

using System;
using System.Windows.Forms;

using dotMCLauncher.Core;
using dotMCLauncher.Core.Data;

static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
        var logger = new FileLogger();
        var gameFiles = new GameFileStructure();
        var localization = new Localization();
        var versionsService = new VersionsService(logger, gameFiles);

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        var frmMain = new MainForm(gameFiles, localization, logger, versionsService);
        Application.Run(frmMain);
    }
}