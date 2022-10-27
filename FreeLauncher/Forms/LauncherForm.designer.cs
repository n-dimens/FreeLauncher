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
            // this.EditVersions = new Telerik.WinControls.UI.RadPageViewPage();
            // this.versionsListView = new Telerik.WinControls.UI.RadListView();
            this.BottomToolbarPanel = new Telerik.WinControls.UI.RadPanel();
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
            // this.EditVersions.SuspendLayout();
            // ((System.ComponentModel.ISupportInitialize)(this.versionsListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomToolbarPanel)).BeginInit();
            this.BottomToolbarPanel.SuspendLayout();
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
            // this.mainPageView.Controls.Add(this.EditVersions);
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
            this.mainPageView.Size = new System.Drawing.Size(858, 363);
            this.mainPageView.TabIndex = 2;
            this.mainPageView.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.mainPageView.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
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
            // radPanel1
            // 
            this.BottomToolbarPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radPanel1.BackgroundImage")));
            this.BottomToolbarPanel.Controls.Add(this.DeleteProfileButton);
            this.BottomToolbarPanel.Controls.Add(this.ManageUsersButton);
            this.BottomToolbarPanel.Controls.Add(this.NicknameDropDownList);
            this.BottomToolbarPanel.Controls.Add(this.SelectedVersion);
            this.BottomToolbarPanel.Controls.Add(this.LogoBox);
            this.BottomToolbarPanel.Controls.Add(this.LaunchButton);
            this.BottomToolbarPanel.Controls.Add(this.profilesDropDownBox);
            this.BottomToolbarPanel.Controls.Add(this.EditProfile);
            this.BottomToolbarPanel.Controls.Add(this.AddProfile);
            this.BottomToolbarPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomToolbarPanel.Location = new System.Drawing.Point(0, 387);
            this.BottomToolbarPanel.Name = "radPanel1";
            // 
            // 
            // 
            this.BottomToolbarPanel.RootElement.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.BottomToolbarPanel.RootElement.AngleTransform = 0F;
            this.BottomToolbarPanel.RootElement.FlipText = false;
            this.BottomToolbarPanel.RootElement.Margin = new System.Windows.Forms.Padding(0);
            this.BottomToolbarPanel.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolbarPanel.Size = new System.Drawing.Size(858, 59);
            this.BottomToolbarPanel.TabIndex = 3;
            this.BottomToolbarPanel.ThemeName = "VisualStudio2012Dark";
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
            this.Controls.Add(this.BottomToolbarPanel);
            this.MinimumSize = new System.Drawing.Size(712, 446);
            this.Name = "LauncherForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "FreeLauncher";
            this.ThemeName = "VisualStudio2012Dark";
            ((System.ComponentModel.ISupportInitialize)(this.mainPageView)).EndInit();
            this.mainPageView.ResumeLayout(false);
            // this.EditVersions.ResumeLayout(false);
            // ((System.ComponentModel.ISupportInitialize)(this.versionsListView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomToolbarPanel)).EndInit();
            this.BottomToolbarPanel.ResumeLayout(false);
            this.BottomToolbarPanel.PerformLayout();
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
        // private RadPageViewPage EditVersions;
        // private RadListView versionsListView;

        private RadPanel BottomToolbarPanel;
        private RadButton ManageUsersButton;
        public RadDropDownList NicknameDropDownList;
        private Label SelectedVersion;
        private PictureBox LogoBox;
        public RadButton LaunchButton;
        public RadDropDownList profilesDropDownBox;
        private RadButton EditProfile;
        private RadButton AddProfile;
        private RadButton DeleteProfileButton;
    }
}
