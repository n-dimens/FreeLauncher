namespace NDimens.Minecraft.FreeLauncher; 

partial class MainForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.tpLog = new System.Windows.Forms.TabPage();
        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.txtLog = new System.Windows.Forms.RichTextBox();
        this.tpSettings = new System.Windows.Forms.TabPage();
        this.btnSaveSettings = new System.Windows.Forms.Button();
        this.chbCloseOutput = new System.Windows.Forms.CheckBox();
        this.chbUseLogPrefix = new System.Windows.Forms.CheckBox();
        this.chbEnableGameLogging = new System.Windows.Forms.CheckBox();
        this.btnLaunch = new System.Windows.Forms.Button();
        this.cbProfiles = new System.Windows.Forms.ComboBox();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.panel1 = new System.Windows.Forms.Panel();
        this.btnUsers = new System.Windows.Forms.Button();
        this.cbUsers = new System.Windows.Forms.ComboBox();
        this.lblSelectedVersion = new System.Windows.Forms.Label();
        this.btnEditProfile = new System.Windows.Forms.Button();
        this.btnAddProfile = new System.Windows.Forms.Button();
        this.txtInstallationDir = new System.Windows.Forms.TextBox();
        this.tabControl1.SuspendLayout();
        this.tpLog.SuspendLayout();
        this.tpSettings.SuspendLayout();
        this.tableLayoutPanel1.SuspendLayout();
        this.panel1.SuspendLayout();
        this.SuspendLayout();
        // 
        // tabControl1
        // 
        this.tabControl1.Controls.Add(this.tpLog);
        this.tabControl1.Controls.Add(this.tpSettings);
        this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabControl1.Location = new System.Drawing.Point(3, 3);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(1058, 602);
        this.tabControl1.TabIndex = 0;
        // 
        // tpLog
        // 
        this.tpLog.Controls.Add(this.progressBar);
        this.tpLog.Controls.Add(this.txtLog);
        this.tpLog.Location = new System.Drawing.Point(4, 22);
        this.tpLog.Name = "tpLog";
        this.tpLog.Padding = new System.Windows.Forms.Padding(3);
        this.tpLog.Size = new System.Drawing.Size(1050, 576);
        this.tpLog.TabIndex = 0;
        this.tpLog.Text = "Журнал";
        this.tpLog.UseVisualStyleBackColor = true;
        // 
        // progressBar
        // 
        this.progressBar.Location = new System.Drawing.Point(5, 577);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new System.Drawing.Size(1039, 23);
        this.progressBar.TabIndex = 3;
        // 
        // txtLog
        // 
        this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.txtLog.Location = new System.Drawing.Point(3, 3);
        this.txtLog.Name = "txtLog";
        this.txtLog.ReadOnly = true;
        this.txtLog.Size = new System.Drawing.Size(1044, 569);
        this.txtLog.TabIndex = 0;
        this.txtLog.Text = "";
        this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
        // 
        // tpSettings
        // 
        this.tpSettings.Controls.Add(this.txtInstallationDir);
        this.tpSettings.Controls.Add(this.btnSaveSettings);
        this.tpSettings.Controls.Add(this.chbCloseOutput);
        this.tpSettings.Controls.Add(this.chbUseLogPrefix);
        this.tpSettings.Controls.Add(this.chbEnableGameLogging);
        this.tpSettings.Location = new System.Drawing.Point(4, 22);
        this.tpSettings.Name = "tpSettings";
        this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
        this.tpSettings.Size = new System.Drawing.Size(1050, 576);
        this.tpSettings.TabIndex = 1;
        this.tpSettings.Text = "Настройки";
        this.tpSettings.UseVisualStyleBackColor = true;
        // 
        // btnSaveSettings
        // 
        this.btnSaveSettings.Location = new System.Drawing.Point(7, 104);
        this.btnSaveSettings.Name = "btnSaveSettings";
        this.btnSaveSettings.Size = new System.Drawing.Size(178, 23);
        this.btnSaveSettings.TabIndex = 3;
        this.btnSaveSettings.Text = "Сохранить";
        this.btnSaveSettings.UseVisualStyleBackColor = true;
        this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
        // 
        // chbCloseOutput
        // 
        this.chbCloseOutput.AutoSize = true;
        this.chbCloseOutput.Location = new System.Drawing.Point(7, 55);
        this.chbCloseOutput.Name = "chbCloseOutput";
        this.chbCloseOutput.Size = new System.Drawing.Size(230, 17);
        this.chbCloseOutput.TabIndex = 2;
        this.chbCloseOutput.Text = "Закрывать вкладку при выходе из игры";
        this.chbCloseOutput.UseVisualStyleBackColor = true;
        // 
        // chbUseLogPrefix
        // 
        this.chbUseLogPrefix.AutoSize = true;
        this.chbUseLogPrefix.Location = new System.Drawing.Point(7, 31);
        this.chbUseLogPrefix.Name = "chbUseLogPrefix";
        this.chbUseLogPrefix.Size = new System.Drawing.Size(286, 17);
        this.chbUseLogPrefix.TabIndex = 1;
        this.chbUseLogPrefix.Text = "Использовать префикс [GAME] для логов Minecraft";
        this.chbUseLogPrefix.UseVisualStyleBackColor = true;
        // 
        // chbEnableGameLogging
        // 
        this.chbEnableGameLogging.AutoSize = true;
        this.chbEnableGameLogging.Location = new System.Drawing.Point(7, 7);
        this.chbEnableGameLogging.Name = "chbEnableGameLogging";
        this.chbEnableGameLogging.Size = new System.Drawing.Size(178, 17);
        this.chbEnableGameLogging.TabIndex = 0;
        this.chbEnableGameLogging.Text = "Выводить лог игры в консоль";
        this.chbEnableGameLogging.UseVisualStyleBackColor = true;
        // 
        // btnLaunch
        // 
        this.btnLaunch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnLaunch.Location = new System.Drawing.Point(933, 3);
        this.btnLaunch.Name = "btnLaunch";
        this.btnLaunch.Size = new System.Drawing.Size(124, 23);
        this.btnLaunch.TabIndex = 1;
        this.btnLaunch.Text = "Запуск";
        this.btnLaunch.UseVisualStyleBackColor = true;
        this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
        // 
        // cbProfiles
        // 
        this.cbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbProfiles.FormattingEnabled = true;
        this.cbProfiles.Location = new System.Drawing.Point(10, 3);
        this.cbProfiles.Name = "cbProfiles";
        this.cbProfiles.Size = new System.Drawing.Size(228, 21);
        this.cbProfiles.TabIndex = 2;
        this.cbProfiles.SelectedIndexChanged += new System.EventHandler(this.cbProfiles_SelectedIndexChanged);
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.ColumnCount = 1;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
        this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
        this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 2;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.Size = new System.Drawing.Size(1064, 668);
        this.tableLayoutPanel1.TabIndex = 3;
        // 
        // panel1
        // 
        this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.panel1.Controls.Add(this.btnUsers);
        this.panel1.Controls.Add(this.cbUsers);
        this.panel1.Controls.Add(this.lblSelectedVersion);
        this.panel1.Controls.Add(this.btnEditProfile);
        this.panel1.Controls.Add(this.btnAddProfile);
        this.panel1.Controls.Add(this.btnLaunch);
        this.panel1.Controls.Add(this.cbProfiles);
        this.panel1.Location = new System.Drawing.Point(0, 608);
        this.panel1.Margin = new System.Windows.Forms.Padding(0);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(1064, 60);
        this.panel1.TabIndex = 0;
        // 
        // btnUsers
        // 
        this.btnUsers.Location = new System.Drawing.Point(272, 27);
        this.btnUsers.Name = "btnUsers";
        this.btnUsers.Size = new System.Drawing.Size(175, 23);
        this.btnUsers.TabIndex = 7;
        this.btnUsers.Text = "Пользователи";
        this.btnUsers.UseVisualStyleBackColor = true;
        this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
        // 
        // cbUsers
        // 
        this.cbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbUsers.FormattingEnabled = true;
        this.cbUsers.Location = new System.Drawing.Point(272, 5);
        this.cbUsers.Name = "cbUsers";
        this.cbUsers.Size = new System.Drawing.Size(175, 21);
        this.cbUsers.TabIndex = 6;
        this.cbUsers.SelectedIndexChanged += new System.EventHandler(this.cbUsers_SelectedIndexChanged);
        // 
        // lblSelectedVersion
        // 
        this.lblSelectedVersion.AutoSize = true;
        this.lblSelectedVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.lblSelectedVersion.Location = new System.Drawing.Point(660, 36);
        this.lblSelectedVersion.Name = "lblSelectedVersion";
        this.lblSelectedVersion.Size = new System.Drawing.Size(41, 15);
        this.lblSelectedVersion.TabIndex = 5;
        this.lblSelectedVersion.Text = "label1";
        // 
        // btnEditProfile
        // 
        this.btnEditProfile.Location = new System.Drawing.Point(124, 27);
        this.btnEditProfile.Name = "btnEditProfile";
        this.btnEditProfile.Size = new System.Drawing.Size(114, 23);
        this.btnEditProfile.TabIndex = 4;
        this.btnEditProfile.Text = "Изменить профиль";
        this.btnEditProfile.UseVisualStyleBackColor = true;
        this.btnEditProfile.Click += new System.EventHandler(this.btnEditProfile_Click);
        // 
        // btnAddProfile
        // 
        this.btnAddProfile.Location = new System.Drawing.Point(10, 27);
        this.btnAddProfile.Name = "btnAddProfile";
        this.btnAddProfile.Size = new System.Drawing.Size(114, 23);
        this.btnAddProfile.TabIndex = 3;
        this.btnAddProfile.Text = "Добавить профиль";
        this.btnAddProfile.UseVisualStyleBackColor = true;
        this.btnAddProfile.Click += new System.EventHandler(this.btnAddProfile_Click);
        // 
        // txtInstallationDir
        // 
        this.txtInstallationDir.Location = new System.Drawing.Point(7, 78);
        this.txtInstallationDir.Name = "txtInstallationDir";
        this.txtInstallationDir.Size = new System.Drawing.Size(319, 20);
        this.txtInstallationDir.TabIndex = 4;
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1064, 668);
        this.Controls.Add(this.tableLayoutPanel1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Minecraft Launcher";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
        this.tabControl1.ResumeLayout(false);
        this.tpLog.ResumeLayout(false);
        this.tpSettings.ResumeLayout(false);
        this.tpSettings.PerformLayout();
        this.tableLayoutPanel1.ResumeLayout(false);
        this.panel1.ResumeLayout(false);
        this.panel1.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tpLog;
    private System.Windows.Forms.TabPage tpSettings;
    private System.Windows.Forms.Button btnLaunch;
    private System.Windows.Forms.ComboBox cbProfiles;
    private System.Windows.Forms.RichTextBox txtLog;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.CheckBox chbCloseOutput;
    private System.Windows.Forms.CheckBox chbUseLogPrefix;
    private System.Windows.Forms.CheckBox chbEnableGameLogging;
    private System.Windows.Forms.Button btnSaveSettings;
    private System.Windows.Forms.ProgressBar progressBar;
    private System.Windows.Forms.Button btnEditProfile;
    private System.Windows.Forms.Button btnAddProfile;
    private System.Windows.Forms.Label lblSelectedVersion;
    private System.Windows.Forms.Button btnUsers;
    private System.Windows.Forms.ComboBox cbUsers;
    private System.Windows.Forms.TextBox txtInstallationDir;
}