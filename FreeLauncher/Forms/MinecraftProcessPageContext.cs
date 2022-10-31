using dotMCLauncher.Core;
using Telerik.WinControls.Enumerations;

namespace FreeLauncher.Forms {
    public class MinecraftProcessPageContext {
        public GameProcessForm LauncherForm { get; }
        public Profile Profile { get; }
        public GameFileStructure ApplicationContext { get; }
        public bool IsMinecraftLoggingEnabled { get; }
        public bool IsUseGamePrefix { get; }
        public bool IsAutoClosePage { get; }

        public MinecraftProcessPageContext(GameProcessForm launcherForm, Profile profile, GameFileStructure appContext) {
            LauncherForm = launcherForm;
            Profile = profile;
            ApplicationContext = appContext;
            IsMinecraftLoggingEnabled = appContext.Configuration.EnableGameLogging;
            IsUseGamePrefix = appContext.Configuration.ShowGamePrefix;
            IsAutoClosePage = appContext.Configuration.CloseTabAfterSuccessfulExitCode;
        }
    }
}