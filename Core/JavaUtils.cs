using System;
using System.IO;

namespace FreeLauncher {
    public static class JavaUtils {
        /// <summary>
        /// Проверка переменных окружения в порядке JAVA_HOME, JRE_HOME, JDK_HOME
        /// </summary>
        /// <returns></returns>
        public static string GetJavaInstallationPath() {
            var javaHome = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrEmpty(javaHome)) {
                return javaHome;
            }

            var jreHome = Environment.GetEnvironmentVariable("JRE_HOME");
            if (!string.IsNullOrEmpty(jreHome)) {
                return jreHome;
            }

            var jdkHome = Environment.GetEnvironmentVariable("JDK_HOME");
            if (!string.IsNullOrEmpty(jdkHome)) {
                return jdkHome;
            }

            return string.Empty;
        }

        public static string GetJavaExecutable() {
            var javaDir = GetJavaInstallationPath();
            if (string.IsNullOrEmpty(javaDir)) {
                return string.Empty;
            }

            return Path.Combine(javaDir, "bin", "java.exe");
        }
    }
}