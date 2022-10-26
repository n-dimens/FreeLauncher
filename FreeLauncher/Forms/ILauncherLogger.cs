namespace FreeLauncher.Forms {
    public interface ILauncherLogger {
        void LogInfo(string text, string methodName = null);
        
        void LogError(string text, string methodName = null);

        void LogDebug(string text, string methodName = null);
    }
}