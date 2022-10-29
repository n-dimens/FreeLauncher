﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using CommandLine;

using FreeLauncher.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Telerik.WinControls;

namespace FreeLauncher {
    internal class Program {
        [STAThread]
        public static void Main(string[] args) {
            var applicationContext = new ApplicationContext();
            ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var frmMain = new MainForm(applicationContext);
            Application.Run(frmMain);
        }
    }
}