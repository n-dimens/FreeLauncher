using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace FreeLauncher.Forms
{
    partial class GameProcessForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameProcessForm));
            this.vs12theme = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.mainPageView = new Telerik.WinControls.UI.RadPageView();
            // this.EditVersions = new Telerik.WinControls.UI.RadPageViewPage();
            // this.versionsListView = new Telerik.WinControls.UI.RadListView();
            ((System.ComponentModel.ISupportInitialize)(this.mainPageView)).BeginInit();
            this.mainPageView.SuspendLayout();
            // this.EditVersions.SuspendLayout();
            // ((System.ComponentModel.ISupportInitialize)(this.versionsListView)).BeginInit();
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
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 446);
            this.Controls.Add(this.mainPageView);
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
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VisualStudio2012DarkTheme vs12theme;
        private RadPageView mainPageView;
        // private RadPageViewPage EditVersions;
        // private RadListView versionsListView;
    }
}
