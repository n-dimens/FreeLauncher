namespace dotMCLauncher.Core {
    public class Configuration {
        public bool EnableGameLogging { get; set; } = true;

        public bool ShowGamePrefix { get; set; } = true;

        public bool CloseTabAfterSuccessfulExitCode { get; set; } = false;

        public string InstallationDirectory { get; set; } = string.Empty;
    }
}
