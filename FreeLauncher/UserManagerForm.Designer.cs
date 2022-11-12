namespace NDimens.Minecraft.FreeLauncher; 

partial class UserManagerForm {
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
        this.lbUsers = new System.Windows.Forms.ListBox();
        this.btnDelete = new System.Windows.Forms.Button();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.btnAdd = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // lbUsers
        // 
        this.lbUsers.FormattingEnabled = true;
        this.lbUsers.Location = new System.Drawing.Point(12, 12);
        this.lbUsers.Name = "lbUsers";
        this.lbUsers.Size = new System.Drawing.Size(155, 186);
        this.lbUsers.TabIndex = 0;
        this.lbUsers.SelectedIndexChanged += new System.EventHandler(this.lbUsers_SelectedIndexChanged);
        // 
        // btnDelete
        // 
        this.btnDelete.Enabled = false;
        this.btnDelete.Location = new System.Drawing.Point(92, 204);
        this.btnDelete.Name = "btnDelete";
        this.btnDelete.Size = new System.Drawing.Size(75, 23);
        this.btnDelete.TabIndex = 1;
        this.btnDelete.Text = "Удалить";
        this.btnDelete.UseVisualStyleBackColor = true;
        this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
        // 
        // txtUsername
        // 
        this.txtUsername.Location = new System.Drawing.Point(174, 13);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(150, 20);
        this.txtUsername.TabIndex = 2;
        this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
        // 
        // btnAdd
        // 
        this.btnAdd.Enabled = false;
        this.btnAdd.Location = new System.Drawing.Point(249, 39);
        this.btnAdd.Name = "btnAdd";
        this.btnAdd.Size = new System.Drawing.Size(75, 23);
        this.btnAdd.TabIndex = 3;
        this.btnAdd.Text = "Добавить";
        this.btnAdd.UseVisualStyleBackColor = true;
        this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
        // 
        // UserManagerForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(329, 233);
        this.Controls.Add(this.btnAdd);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.btnDelete);
        this.Controls.Add(this.lbUsers);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        this.Name = "UserManagerForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Пользователи";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListBox lbUsers;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Button btnAdd;
}