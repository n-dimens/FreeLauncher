using dotMCLauncher.Core;
using Telerik.WinControls.Enumerations;

namespace FreeLauncher.Forms {
    public class MinecraftProcessPageContext {
        public LauncherForm LauncherForm { get; }
        public Profile Profile { get; }
        public ApplicationContext ApplicationContext { get; }
        public bool IsMinecraftLoggingEnabled { get; }
        public bool IsUseGamePrefix { get; }
        public bool IsAutoClosePage { get; }

        public MinecraftProcessPageContext(LauncherForm launcherForm, Profile profile, ApplicationContext appContext) {
            LauncherForm = launcherForm;
            Profile = profile;
            ApplicationContext = appContext;
            IsMinecraftLoggingEnabled = launcherForm.EnableMinecraftLogging.Checked;
            IsUseGamePrefix = launcherForm.UseGamePrefix.ToggleState == ToggleState.On;
            IsAutoClosePage = launcherForm.CloseGameOutput.Checked;
        }
    }
}