namespace NDimens.Minecraft.FreeLauncher;

using System;
using System.IO;
using System.Windows.Forms;

using dotMCLauncher.Core;
using dotMCLauncher.Core.Data;

using NDimens.Minecraft.FreeLauncher.Core.Data;
using NDimens.Minecraft.FreeLauncher.Presenters;

static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
        // init services
        var logger = new FileLogger();
        var gameFiles = new GameFileStructure();
        var localization = new Localization();
        var versionsService = new VersionsService(logger, gameFiles);
        var usersRepository = new UsersRepository(new FileInfo(gameFiles.LauncherUsers));
        var profilesRepository = new ProfilesRepository(new FileInfo(gameFiles.LauncherProfiles));
        var formFactory = new FormFactory(usersRepository, profilesRepository);

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var mainPresenter = new MainFormPresenter(logger, gameFiles, versionsService, usersRepository, profilesRepository);
        var frmMain = new MainForm(mainPresenter, formFactory, gameFiles, localization, logger);
        Application.Run(frmMain);
    }
}