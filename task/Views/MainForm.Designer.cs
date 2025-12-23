namespace AISchool.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.backMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
          
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backMenuItem,
            this.logoutMenuItem,
            this.changePasswordMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(1024, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            this.mainMenuStrip.Visible = false;
         
            this.backMenuItem.Name = "backMenuItem";
            this.backMenuItem.Size = new System.Drawing.Size(68, 20);
            this.backMenuItem.Text = "<- Назад";
            this.backMenuItem.Visible = false;
            this.backMenuItem.Click += new System.EventHandler(this.backMenuItem_Click);
         
            this.logoutMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.logoutMenuItem.Name = "logoutMenuItem";
            this.logoutMenuItem.Size = new System.Drawing.Size(54, 20);
            this.logoutMenuItem.Text = "Выйти";
            this.logoutMenuItem.Click += new System.EventHandler(this.LogoutMenuItem_Click);
           
            this.changePasswordMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.changePasswordMenuItem.Name = "changePasswordMenuItem";
            this.changePasswordMenuItem.Size = new System.Drawing.Size(113, 20);
            this.changePasswordMenuItem.Text = "Сменить пароль";
            this.changePasswordMenuItem.Click += new System.EventHandler(this.ChangePasswordMenuItem_Click);
          
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(1024, 768);
            this.contentPanel.TabIndex = 1;
          
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "АИС \'Школа\'";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem backMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordMenuItem;
        private System.Windows.Forms.Panel contentPanel;
    }
}