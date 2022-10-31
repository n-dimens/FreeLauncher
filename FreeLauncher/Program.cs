using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using CommandLine;

using dotMCLauncher.Core;

using FreeLauncher.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Telerik.WinControls;

namespace FreeLauncher {
    internal class Program {
        [STAThread]
        public static void Main(string[] args) {
            var gameFiles = new GameFileStructure();
            var localization = new Localization();
            ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var frmMain = new MainForm(gameFiles, localization);
            Application.Run(frmMain);
        }
    }
}