using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using dotMCLauncher.Core;
using Telerik.WinControls.UI;

namespace FreeLauncher.Forms {
    public partial class GameProcessForm : RadForm {
        private readonly ApplicationContext _applicationContext;
      
        public GameProcessForm(ApplicationContext appContext) {
            _applicationContext = appContext;
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Text = ProductName + " " + ProductVersion;      
        }

        /// <summary>
        /// Создание вкладки для нового процесса Minecraft
        /// </summary>
        internal MinecraftProcessPage CreateMinecraftProcessPage(Profile selectedProfile, ProcessStartInfo processInfo) {
            var context = new MinecraftProcessPageContext(this, selectedProfile, _applicationContext);
            var mcProcessPage = new MinecraftProcessPage(new Process {StartInfo = processInfo, EnableRaisingEvents = true}, context);
            mcProcessPage.PageCreated += (o, page) => {
                mainPageView.Pages.Add(page);
                mainPageView.SelectedPage = page;
            };
            mcProcessPage.PageClosed += (o, page) => { mainPageView.Pages.Remove(page); };
            mcProcessPage.ProcessLaunched += (o, args) => {
                if (selectedProfile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.CLOSED) {
                    Close();
                }

                if (selectedProfile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.HIDDEN) {
                    Hide();
                }
            };
            mcProcessPage.ProcessExited += (o, args) => {
                if (selectedProfile.LauncherVisibilityOnGameClose == Profile.LauncherVisibility.HIDDEN) {
                    Invoke((MethodInvoker) Show);
                }
            };
            return mcProcessPage;
        }
    }
}