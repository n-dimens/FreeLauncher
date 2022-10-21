using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace FreeLauncher.Forms
{
    partial class LauncherForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // Telerik.WinControls.UI.ListViewDetailColumn listViewDetailColumn1 = new Telerik.WinControls.UI.ListViewDetailColumn("Column 0", "Версия");
            // Telerik.WinControls.UI.ListViewDetailColumn listViewDetailColumn2 = new Telerik.WinControls.UI.ListViewDetailColumn("Column 1", "Тип");
            // Telerik.WinControls.UI.ListViewDetailColumn listViewDetailColumn3 = new Telerik.WinControls.UI.ListViewDetailColumn("Column 2", "Зависимость");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherForm));
            this.vs12theme = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.mainPageView = new Telerik.WinControls.UI.RadPageView();
            this.ConsolePage = new Telerik.WinControls.UI.RadPageViewPage();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.ConsoleOptionsPanel = new Telerik.WinControls.UI.RadPanel();
            this.SetToClipboardButton = new Telerik.WinControls.UI.RadButton();
            this.DebugModeButton = new Telerik.WinControls.UI.RadToggleButton();
            // this.EditVersions = new Telerik.WinControls.UI.RadPageViewPage();
            // this.versionsListView = new Telerik.WinControls.UI.RadListView();
            this.AboutPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.AboutPageView = new Telerik.WinControls.UI.RadPageView();
            this.AboutVersion = new Telerik.WinControls.UI.RadLabel();
            this.SettingsPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.radScrollablePanel1 = new Telerik.WinControls.UI.RadScrollablePanel();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.CloseGameOutput = new Telerik.WinControls.UI.RadCheckBox();
            this.UseGamePrefix = new Telerik.WinControls.UI.RadCheckBox();
            this.EnableMinecraftLogging = new Telerik.WinControls.UI.RadCheckBox();
            this.StatusBar = new Telerik.WinControls.UI.RadProgressBar();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.DeleteProfileButton = new Telerik.WinControls.UI.RadButton();
            this.ManageUsersButton = new Telerik.WinControls.UI.RadButton();
            this.NicknameDropDownList = new Telerik.WinControls.UI.RadDropDownList();
            this.SelectedVersion = new System.Windows.Forms.Label();
            this.LogoBox = new System.Windows.Forms.PictureBox();
            this.LaunchButton = new Telerik.WinControls.UI.RadButton();
            this.profilesDropDownBox = new Telerik.WinControls.UI.RadDropDownList();
            this.EditProfile = new Telerik.WinControls.UI.RadButton();
            this.AddProfile = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.mainPageView)).BeginInit();
            this.mainPageView.SuspendLayout();
            this.ConsolePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConsoleOptionsPanel)).BeginInit();
            this.ConsoleOptionsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SetToClipboardButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DebugModeButton)).BeginInit();
            // this.EditVersions.SuspendLayout();
            // ((System.ComponentModel.ISupportInitialize)(this.versionsListView)).BeginInit();
            this.AboutPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AboutPageView)).BeginInit();
            this.AboutPageView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AboutVersion)).BeginInit();
            this.SettingsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).BeginInit();
            this.radScrollablePanel1.PanelContainer.SuspendLayout();
            this.radScrollablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CloseGameOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UseGamePrefix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnableMinecraftLogging)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeleteProfileButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ManageUsersButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NicknameDropDownList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LaunchButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.profilesDropDownBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPageView
            // 
            this.mainPageView.Controls.Add(this.ConsolePage);
            // this.mainPageView.Controls.Add(this.EditVersions);
            this.mainPageView.Controls.Add(this.AboutPage);
            this.mainPageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPageView.Location = new System.Drawing.Point(0, 0);
            this.mainPageView.Name = "mainPageView";
            // 
            // 
            // 
            this.mainPageView.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.mainPageView.RootElement.AngleTransform = 0F;
            this.mainPageView.RootElement.FlipText = false;
            this.mainPageView.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.mainPageView.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.mainPageView.SelectedPage = this.ConsolePage;
            this.mainPageView.Size = new System.Drawing.Size(858, 363);
            this.mainPageView.TabIndex = 2;
            this.mainPageView.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.mainPageView.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            // 
            // ConsolePage
            // 
            this.ConsolePage.Controls.Add(this.logBox);
            this.ConsolePage.Controls.Add(this.ConsoleOptionsPanel);
            this.ConsolePage.ItemSize = new System.Drawing.SizeF(65F, 24F);
            this.ConsolePage.Location = new System.Drawing.Point(5, 30);
            this.ConsolePage.Name = "ConsolePage";
            this.ConsolePage.Size = new System.Drawing.Size(848, 328);
            this.ConsolePage.Text = "КОНСОЛЬ";
            // 
            // logBox
            // 
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.logBox.Location = new System.Drawing.Point(0, 0);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(848, 299);
            this.logBox.TabIndex = 1;
            this.logBox.Text = "";
            this.logBox.TextChanged += new System.EventHandler(this.logBox_TextChanged);
            // 
            // ConsoleOptionsPanel
            // 
            this.ConsoleOptionsPanel.Controls.Add(this.SetToClipboardButton);
            this.ConsoleOptionsPanel.Controls.Add(this.DebugModeButton);
            this.ConsoleOptionsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ConsoleOptionsPanel.Location = new System.Drawing.Point(0, 299);
            this.ConsoleOptionsPanel.Name = "ConsoleOptionsPanel";
            this.ConsoleOptionsPanel.Size = new System.Drawing.Size(848, 29);
            this.ConsoleOptionsPanel.TabIndex = 2;
            this.ConsoleOptionsPanel.ThemeName = "VisualStudio2012Dark";
            // 
            // SetToClipboardButton
            // 
            this.SetToClipboardButton.Location = new System.Drawing.Point(7, 3);
            this.SetToClipboardButton.Name = "SetToClipboardButton";
            this.SetToClipboardButton.Size = new System.Drawing.Size(131, 23);
            this.SetToClipboardButton.TabIndex = 1;
            this.SetToClipboardButton.Text = "Скопировать в буфер";
            this.SetToClipboardButton.ThemeName = "VisualStudio2012Dark";
            this.SetToClipboardButton.Click += new System.EventHandler(this.SetToClipboardButton_Click);
            // 
            // DebugModeButton
            // 
            this.DebugModeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugModeButton.Location = new System.Drawing.Point(710, 3);
            this.DebugModeButton.Name = "DebugModeButton";
            this.DebugModeButton.Size = new System.Drawing.Size(131, 23);
            this.DebugModeButton.TabIndex = 0;
            this.DebugModeButton.Text = "Debug Mode";
            this.DebugModeButton.ThemeName = "VisualStudio2012Dark";
            // 
            // EditVersions
            // 
            // this.EditVersions.Controls.Add(this.versionsListView);
            // this.EditVersions.ItemSize = new System.Drawing.SizeF(145F, 24F);
            // this.EditVersions.Location = new System.Drawing.Point(5, 30);
            // this.EditVersions.Name = "EditVersions";
            // this.EditVersions.Size = new System.Drawing.Size(848, 328);
            // this.EditVersions.Text = "УПРАВЛЕНИЕ ВЕРСИЯМИ";
            // 
            // versionsListView
            // 
            // this.versionsListView.AllowColumnReorder = false;
            // this.versionsListView.AllowColumnResize = false;
            // this.versionsListView.AllowEdit = false;
            // this.versionsListView.AllowRemove = false;
            // this.versionsListView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            // this.versionsListView.CheckOnClickMode = Telerik.WinControls.UI.CheckOnClickMode.FirstClick;
            // listViewDetailColumn1.HeaderText = "Версия";
            // listViewDetailColumn2.HeaderText = "Тип";
            // listViewDetailColumn2.Width = 100F;
            // listViewDetailColumn3.HeaderText = "Зависимость";
            // listViewDetailColumn3.Width = 100F;
            // this.versionsListView.Columns.AddRange(new Telerik.WinControls.UI.ListViewDetailColumn[] {
            // listViewDetailColumn1,
            // listViewDetailColumn2,
            // listViewDetailColumn3});
            // this.versionsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            // this.versionsListView.EnableColumnSort = true;
            // this.versionsListView.EnableFiltering = true;
            // this.versionsListView.EnableSorting = true;
            // this.versionsListView.HorizontalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysHide;
            // this.versionsListView.ItemSpacing = -1;
            // this.versionsListView.Location = new System.Drawing.Point(0, 0);
            // this.versionsListView.Name = "versionsListView";
            // 
            // 
            // 
            // this.versionsListView.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            // this.versionsListView.RootElement.AngleTransform = 0F;
            // this.versionsListView.RootElement.FlipText = false;
            // this.versionsListView.RootElement.Margin = new System.Windows.Forms.Padding(0);
            // this.versionsListView.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            // this.versionsListView.SelectLastAddedItem = false;
            // this.versionsListView.ShowItemToolTips = false;
            // this.versionsListView.Size = new System.Drawing.Size(848, 328);
            // this.versionsListView.TabIndex = 0;
            // this.versionsListView.ThemeName = "VisualStudio2012Dark";
            // this.versionsListView.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            // this.versionsListView.ViewType = Telerik.WinControls.UI.ListViewType.DetailsView;
            // this.versionsListView.ItemMouseClick += new Telerik.WinControls.UI.ListViewItemEventHandler(this.versionsListView_ItemMouseClick);
            // 
            // AboutPage
            // 
            this.AboutPage.Controls.Add(this.AboutPageView);
            this.AboutPage.ItemSize = new System.Drawing.SizeF(79F, 24F);
            this.AboutPage.Location = new System.Drawing.Point(5, 30);
            this.AboutPage.Name = "AboutPage";
            this.AboutPage.Size = new System.Drawing.Size(848, 328);
            this.AboutPage.Text = "О ЛАУНЧЕРЕ";
            // 
            // AboutPageView
            // 
            this.AboutPageView.Controls.Add(this.SettingsPage);
            this.AboutPageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutPageView.Location = new System.Drawing.Point(0, 0);
            this.AboutPageView.Name = "AboutPageView";
            // 
            // 
            // 
            this.AboutPageView.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.AboutPageView.RootElement.AngleTransform = 0F;
            this.AboutPageView.RootElement.FlipText = false;
            this.AboutPageView.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.AboutPageView.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.AboutPageView.SelectedPage = this.SettingsPage;
            this.AboutPageView.Size = new System.Drawing.Size(848, 328);
            this.AboutPageView.TabIndex = 9;
            this.AboutPageView.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.AboutPageView.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.AboutPageView.GetChildAt(0))).ItemAlignment = Telerik.WinControls.UI.StripViewItemAlignment.Center;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.AboutPageView.GetChildAt(0))).ItemFitMode = Telerik.WinControls.UI.StripViewItemFitMode.Fill;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.AboutPageView.GetChildAt(0))).StripAlignment = Telerik.WinControls.UI.StripViewAlignment.Bottom;
            // 
            // AboutVersion
            // 
            this.AboutVersion.BackColor = System.Drawing.Color.Transparent;
            this.AboutVersion.ForeColor = System.Drawing.Color.DimGray;
            this.AboutVersion.Location = new System.Drawing.Point(122, 34);
            this.AboutVersion.MinimumSize = new System.Drawing.Size(58, 18);
            this.AboutVersion.Name = "AboutVersion";
            // 
            // 
            // 
            this.AboutVersion.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.AboutVersion.RootElement.AngleTransform = 0F;
            this.AboutVersion.RootElement.FlipText = false;
            this.AboutVersion.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.AboutVersion.RootElement.MinSize = new System.Drawing.Size(58, 18);
            this.AboutVersion.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.AboutVersion.Size = new System.Drawing.Size(58, 18);
            this.AboutVersion.TabIndex = 1;
            this.AboutVersion.Text = "0.0.0.000";
            this.AboutVersion.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.AboutVersion.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadLabelElement)(this.AboutVersion.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadLabelElement)(this.AboutVersion.GetChildAt(0))).Text = "0.0.0.000";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.AboutVersion.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // SettingsPage
            // 
            this.SettingsPage.Controls.Add(this.radScrollablePanel1);
            this.SettingsPage.Location = new System.Drawing.Point(5, 5);
            this.SettingsPage.Name = "SettingsPage";
            this.SettingsPage.Size = new System.Drawing.Size(838, 293);
            this.SettingsPage.Text = "НАСТРОЙКИ";
            // 
            // radScrollablePanel1
            // 
            this.radScrollablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radScrollablePanel1.Location = new System.Drawing.Point(0, 0);
            this.radScrollablePanel1.Name = "radScrollablePanel1";
            // 
            // radScrollablePanel1.PanelContainer
            // 
            this.radScrollablePanel1.PanelContainer.Controls.Add(this.radGroupBox2);
            this.radScrollablePanel1.PanelContainer.Size = new System.Drawing.Size(836, 291);
            // 
            // 
            // 
            this.radScrollablePanel1.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.radScrollablePanel1.RootElement.AngleTransform = 0F;
            this.radScrollablePanel1.RootElement.FlipText = false;
            this.radScrollablePanel1.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.radScrollablePanel1.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.radScrollablePanel1.Size = new System.Drawing.Size(838, 293);
            this.radScrollablePanel1.TabIndex = 1;
            this.radScrollablePanel1.Text = "radScrollablePanel1";
            this.radScrollablePanel1.ThemeName = "VisualStudio2012Dark";
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.radGroupBox2.Controls.Add(this.CloseGameOutput);
            this.radGroupBox2.Controls.Add(this.UseGamePrefix);
            this.radGroupBox2.Controls.Add(this.EnableMinecraftLogging);
            this.radGroupBox2.HeaderText = "Логирование";
            this.radGroupBox2.Location = new System.Drawing.Point(22, 5);
            this.radGroupBox2.Name = "radGroupBox2";
            // 
            // 
            // 
            this.radGroupBox2.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.radGroupBox2.RootElement.AngleTransform = 0F;
            this.radGroupBox2.RootElement.FlipText = false;
            this.radGroupBox2.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.radGroupBox2.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.radGroupBox2.Size = new System.Drawing.Size(357, 121);
            this.radGroupBox2.TabIndex = 1;
            this.radGroupBox2.Text = "Логирование";
            this.radGroupBox2.ThemeName = "VisualStudio2012Dark";
            // 
            // CloseGameOutput
            // 
            this.CloseGameOutput.Location = new System.Drawing.Point(5, 69);
            this.CloseGameOutput.Name = "CloseGameOutput";
            // 
            // 
            // 
            this.CloseGameOutput.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.CloseGameOutput.RootElement.AngleTransform = 0F;
            this.CloseGameOutput.RootElement.FlipText = false;
            this.CloseGameOutput.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.CloseGameOutput.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.CloseGameOutput.Size = new System.Drawing.Size(327, 18);
            this.CloseGameOutput.TabIndex = 2;
            this.CloseGameOutput.Text = "Закрывать вкладку, если завершение прошло без ошибок";
            this.CloseGameOutput.ThemeName = "VisualStudio2012Dark";
            // 
            // UseGamePrefix
            // 
            this.UseGamePrefix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseGamePrefix.Location = new System.Drawing.Point(5, 45);
            this.UseGamePrefix.Name = "UseGamePrefix";
            // 
            // 
            // 
            this.UseGamePrefix.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.UseGamePrefix.RootElement.AngleTransform = 0F;
            this.UseGamePrefix.RootElement.FlipText = false;
            this.UseGamePrefix.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.UseGamePrefix.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.UseGamePrefix.Size = new System.Drawing.Size(288, 18);
            this.UseGamePrefix.TabIndex = 1;
            this.UseGamePrefix.Text = "Использовать префикс [GAME] для логов Minecraft";
            this.UseGamePrefix.ThemeName = "VisualStudio2012Dark";
            this.UseGamePrefix.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // EnableMinecraftLogging
            // 
            this.EnableMinecraftLogging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableMinecraftLogging.Location = new System.Drawing.Point(5, 21);
            this.EnableMinecraftLogging.Name = "EnableMinecraftLogging";
            // 
            // 
            // 
            this.EnableMinecraftLogging.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.EnableMinecraftLogging.RootElement.AngleTransform = 0F;
            this.EnableMinecraftLogging.RootElement.FlipText = false;
            this.EnableMinecraftLogging.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.EnableMinecraftLogging.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.EnableMinecraftLogging.Size = new System.Drawing.Size(177, 18);
            this.EnableMinecraftLogging.TabIndex = 0;
            this.EnableMinecraftLogging.Text = "Выводить лог игры в консоль";
            this.EnableMinecraftLogging.ThemeName = "VisualStudio2012Dark";
            this.EnableMinecraftLogging.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // StatusBar
            // 
            this.StatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StatusBar.Location = new System.Drawing.Point(0, 363);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(858, 24);
            this.StatusBar.TabIndex = 4;
            this.StatusBar.Text = "StatusBar";
            this.StatusBar.ThemeName = "VisualStudio2012Dark";
            this.StatusBar.Visible = false;
            // 
            // radPanel1
            // 
            this.radPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radPanel1.BackgroundImage")));
            this.radPanel1.Controls.Add(this.DeleteProfileButton);
            this.radPanel1.Controls.Add(this.ManageUsersButton);
            this.radPanel1.Controls.Add(this.NicknameDropDownList);
            this.radPanel1.Controls.Add(this.SelectedVersion);
            this.radPanel1.Controls.Add(this.LogoBox);
            this.radPanel1.Controls.Add(this.LaunchButton);
            this.radPanel1.Controls.Add(this.profilesDropDownBox);
            this.radPanel1.Controls.Add(this.EditProfile);
            this.radPanel1.Controls.Add(this.AddProfile);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radPanel1.Location = new System.Drawing.Point(0, 387);
            this.radPanel1.Name = "radPanel1";
            // 
            // 
            // 
            this.radPanel1.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.radPanel1.RootElement.AngleTransform = 0F;
            this.radPanel1.RootElement.FlipText = false;
            this.radPanel1.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.radPanel1.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.radPanel1.Size = new System.Drawing.Size(858, 59);
            this.radPanel1.TabIndex = 3;
            this.radPanel1.ThemeName = "VisualStudio2012Dark";
            // 
            // DeleteProfileButton
            // 
            this.DeleteProfileButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteProfileButton.Image")));
            this.DeleteProfileButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.DeleteProfileButton.Location = new System.Drawing.Point(6, 6);
            this.DeleteProfileButton.Name = "DeleteProfileButton";
            this.DeleteProfileButton.Size = new System.Drawing.Size(32, 24);
            this.DeleteProfileButton.TabIndex = 8;
            this.DeleteProfileButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.DeleteProfileButton.ThemeName = "VisualStudio2012Dark";
            this.DeleteProfileButton.Click += new System.EventHandler(this.DeleteProfileButton_Click);
            // 
            // ManageUsersButton
            // 
            this.ManageUsersButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ManageUsersButton.Image = global::FreeLauncher.Properties.Resources.edit;
            this.ManageUsersButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.ManageUsersButton.Location = new System.Drawing.Point(513, 6);
            this.ManageUsersButton.Name = "ManageUsersButton";
            // 
            // 
            // 
            this.ManageUsersButton.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.ManageUsersButton.RootElement.AngleTransform = 0F;
            this.ManageUsersButton.RootElement.FlipText = false;
            this.ManageUsersButton.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.ManageUsersButton.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.ManageUsersButton.Size = new System.Drawing.Size(32, 24);
            this.ManageUsersButton.TabIndex = 7;
            this.ManageUsersButton.ThemeName = "VisualStudio2012Dark";
            this.ManageUsersButton.Click += new System.EventHandler(this.ManageUsersButton_Click);
            // 
            // NicknameDropDownList
            // 
            this.NicknameDropDownList.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.NicknameDropDownList.AutoCompleteDisplayMember = null;
            this.NicknameDropDownList.AutoCompleteValueMember = null;
            this.NicknameDropDownList.Location = new System.Drawing.Point(314, 6);
            this.NicknameDropDownList.Name = "NicknameDropDownList";
            this.NicknameDropDownList.NullText = "Ник";
            // 
            // 
            // 
            this.NicknameDropDownList.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.NicknameDropDownList.RootElement.AngleTransform = 0F;
            this.NicknameDropDownList.RootElement.FlipText = false;
            this.NicknameDropDownList.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.NicknameDropDownList.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.NicknameDropDownList.Size = new System.Drawing.Size(196, 24);
            this.NicknameDropDownList.TabIndex = 3;
            this.NicknameDropDownList.ThemeName = "VisualStudio2012Dark";
            this.NicknameDropDownList.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.NicknameDropDownList_SelectedIndexChanged);
            // 
            // SelectedVersion
            // 
            this.SelectedVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedVersion.AutoSize = true;
            this.SelectedVersion.BackColor = System.Drawing.Color.Transparent;
            this.SelectedVersion.ForeColor = System.Drawing.Color.White;
            this.SelectedVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SelectedVersion.Location = new System.Drawing.Point(631, 42);
            this.SelectedVersion.MinimumSize = new System.Drawing.Size(220, 13);
            this.SelectedVersion.Name = "SelectedVersion";
            this.SelectedVersion.Size = new System.Drawing.Size(220, 13);
            this.SelectedVersion.TabIndex = 6;
            this.SelectedVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LogoBox
            // 
            this.LogoBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LogoBox.BackColor = System.Drawing.Color.Transparent;
            this.LogoBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.LogoBox.Image = ((System.Drawing.Image)(resources.GetObject("LogoBox.Image")));
            this.LogoBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LogoBox.Location = new System.Drawing.Point(651, -11);
            this.LogoBox.Name = "LogoBox";
            this.LogoBox.Size = new System.Drawing.Size(181, 84);
            this.LogoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LogoBox.TabIndex = 5;
            this.LogoBox.TabStop = false;
            // 
            // LaunchButton
            // 
            this.LaunchButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LaunchButton.Location = new System.Drawing.Point(314, 33);
            this.LaunchButton.Name = "LaunchButton";
            // 
            // 
            // 
            this.LaunchButton.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.LaunchButton.RootElement.AngleTransform = 0F;
            this.LaunchButton.RootElement.FlipText = false;
            this.LaunchButton.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.LaunchButton.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.LaunchButton.Size = new System.Drawing.Size(231, 22);
            this.LaunchButton.TabIndex = 4;
            this.LaunchButton.Text = "Запуск игры";
            this.LaunchButton.ThemeName = "VisualStudio2012Dark";
            this.LaunchButton.Click += new System.EventHandler(this.LaunchButton_Click);
            // 
            // profilesDropDownBox
            // 
            this.profilesDropDownBox.AutoCompleteDisplayMember = null;
            this.profilesDropDownBox.AutoCompleteValueMember = null;
            this.profilesDropDownBox.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.profilesDropDownBox.Location = new System.Drawing.Point(41, 6);
            this.profilesDropDownBox.Name = "profilesDropDownBox";
            // 
            // 
            // 
            this.profilesDropDownBox.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.profilesDropDownBox.RootElement.AngleTransform = 0F;
            this.profilesDropDownBox.RootElement.FlipText = false;
            this.profilesDropDownBox.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.profilesDropDownBox.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.profilesDropDownBox.Size = new System.Drawing.Size(191, 24);
            this.profilesDropDownBox.TabIndex = 2;
            this.profilesDropDownBox.ThemeName = "VisualStudio2012Dark";
            this.profilesDropDownBox.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.profilesDropDownBox_SelectedIndexChanged);
            // 
            // EditProfile
            // 
            this.EditProfile.Location = new System.Drawing.Point(122, 33);
            this.EditProfile.Name = "EditProfile";
            // 
            // 
            // 
            this.EditProfile.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.EditProfile.RootElement.AngleTransform = 0F;
            this.EditProfile.RootElement.FlipText = false;
            this.EditProfile.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.EditProfile.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.EditProfile.Size = new System.Drawing.Size(110, 22);
            this.EditProfile.TabIndex = 1;
            this.EditProfile.Text = "Изменить профиль";
            this.EditProfile.TextWrap = true;
            this.EditProfile.ThemeName = "VisualStudio2012Dark";
            this.EditProfile.Click += new System.EventHandler(this.EditProfile_Click);
            // 
            // AddProfile
            // 
            this.AddProfile.Location = new System.Drawing.Point(6, 33);
            this.AddProfile.Name = "AddProfile";
            // 
            // 
            // 
            this.AddProfile.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.AddProfile.RootElement.AngleTransform = 0F;
            this.AddProfile.RootElement.FlipText = false;
            this.AddProfile.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.AddProfile.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.AddProfile.Size = new System.Drawing.Size(110, 22);
            this.AddProfile.TabIndex = 0;
            this.AddProfile.Text = "Добавить профиль";
            this.AddProfile.TextWrap = true;
            this.AddProfile.ThemeName = "VisualStudio2012Dark";
            this.AddProfile.Click += new System.EventHandler(this.AddProfile_Click);
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 446);
            this.Controls.Add(this.mainPageView);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.radPanel1);
            this.MinimumSize = new System.Drawing.Size(712, 446);
            this.Name = "LauncherForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "FreeLauncher";
            this.ThemeName = "VisualStudio2012Dark";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LauncherForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.mainPageView)).EndInit();
            this.mainPageView.ResumeLayout(false);
            this.ConsolePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConsoleOptionsPanel)).EndInit();
            this.ConsoleOptionsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SetToClipboardButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DebugModeButton)).EndInit();
            // this.EditVersions.ResumeLayout(false);
            // ((System.ComponentModel.ISupportInitialize)(this.versionsListView)).EndInit();
            this.AboutPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AboutPageView)).EndInit();
            this.AboutPageView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AboutVersion)).EndInit();
            this.SettingsPage.ResumeLayout(false);
            this.radScrollablePanel1.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).EndInit();
            this.radScrollablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            this.radGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CloseGameOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UseGamePrefix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnableMinecraftLogging)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeleteProfileButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ManageUsersButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NicknameDropDownList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LaunchButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.profilesDropDownBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EditProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VisualStudio2012DarkTheme vs12theme;
        private RadPageView mainPageView;
        private RadPageViewPage ConsolePage;
        public RichTextBox logBox;
        // private RadPageViewPage EditVersions;
        // private RadListView versionsListView;
        private RadPageViewPage AboutPage;
        private RadPageView AboutPageView;
        private RadLabel AboutVersion;
        private RadPageViewPage SettingsPage;
        private RadScrollablePanel radScrollablePanel1;
        private RadGroupBox radGroupBox2;
        public RadCheckBox UseGamePrefix;
        public RadCheckBox EnableMinecraftLogging;
        private RadPanel radPanel1;
        private RadButton ManageUsersButton;
        public RadDropDownList NicknameDropDownList;
        private Label SelectedVersion;
        private PictureBox LogoBox;
        private RadButton LaunchButton;
        public RadDropDownList profilesDropDownBox;
        private RadButton EditProfile;
        private RadButton AddProfile;
        private RadProgressBar StatusBar;
        private RadPanel ConsoleOptionsPanel;
        public RadCheckBox CloseGameOutput;
        private RadToggleButton DebugModeButton;
        private RadButton DeleteProfileButton;
        private RadButton SetToClipboardButton;
    }
}
