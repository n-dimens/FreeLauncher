namespace dotMCLauncher.Core {
    using System;

    // TODO: Migrate to Serilog
    public interface ILauncherLogger {
        event EventHandler<string> Changed;

        void Info(string text, string? methodName = null);

        void Error(string text, string? methodName = null);

        void Debug(string text, string? methodName = null);
    }
}