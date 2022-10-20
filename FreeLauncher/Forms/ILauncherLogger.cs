namespace FreeLauncher.Forms {
    public interface ILauncherLogger {
        void AppendLog(string text, string methodName = null);
        
        void AppendException(string text, string methodName = null);

        void AppendDebug(string text, string methodName = null);
    }
}