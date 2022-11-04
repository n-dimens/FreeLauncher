namespace dotMCLauncher.Core {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class FileLogger : ILauncherLogger {
        private readonly static DirectoryInfo LogDir = new DirectoryInfo("logs");

        public event EventHandler<string>? Changed;

        public FileLogger() {
            if (!LogDir.Exists) {
                LogDir.Create();
            }
        }

        public void Debug(string text, string? methodName = null) {
            Log("DEBUG", text);
        }

        public void Error(string text, string? methodName = null) {
            Log("ERROR", text);
        }

        public void Info(string text, string? methodName = null) {
            Log("INFO", text);
        }

        private void Log(string level, string text) {
            var logFile = Path.Combine(LogDir.FullName, GetCurrentLogName());
            using (var writer = File.AppendText(logFile)) {
                var message = string.Format("{0:dd-MM-yyyy} [{1}] {2}: {3}", DateTime.Now, Thread.CurrentThread.ManagedThreadId, level, text);
                writer.WriteLine(message);
                OnChanged(message);
            }
        }

        private static string GetCurrentLogName() {
            return $"mauncher-{DateTime.Today:dd-MM-yyyy}.txt";
        }

        private void OnChanged(string message) {
            Changed?.Invoke(this, message);
        }
    }
}
