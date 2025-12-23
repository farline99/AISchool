namespace AISchool.Views
{
    partial class LoginControl
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
            this.centerPanel = new System.Windows.Forms.Panel();
            this.loginButton = new System.Windows.Forms.Button();
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.loginLabel = new System.Windows.Forms.Label();
            this.centerPanel.SuspendLayout();
            this.SuspendLayout();
         
            this.centerPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.centerPanel.Controls.Add(this.loginButton);
            this.centerPanel.Controls.Add(this.rememberCheckBox);
            this.centerPanel.Controls.Add(this.passwordTextBox);
            this.centerPanel.Controls.Add(this.passwordLabel);
            this.centerPanel.Controls.Add(this.loginTextBox);
            this.centerPanel.Controls.Add(this.loginLabel);
            this.centerPanel.Location = new System.Drawing.Point(140, 110);
            this.centerPanel.Name = "centerPanel";
            this.centerPanel.Size = new System.Drawing.Size(320, 180);
            this.centerPanel.TabIndex = 0;
          
            this.loginButton.Location = new System.Drawing.Point(215, 120);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 25);
            this.loginButton.TabIndex = 5;
            this.loginButton.Text = "Войти";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LoginButton_Click);
          
            this.rememberCheckBox.AutoSize = true;
            this.rememberCheckBox.Location = new System.Drawing.Point(100, 80);
            this.rememberCheckBox.Name = "rememberCheckBox";
            this.rememberCheckBox.Size = new System.Drawing.Size(119, 19);
            this.rememberCheckBox.TabIndex = 4;
            this.rememberCheckBox.Text = "Сохранить пароль";
            this.rememberCheckBox.UseVisualStyleBackColor = true;
          
            this.passwordTextBox.Location = new System.Drawing.Point(100, 50);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(190, 23);
            this.passwordTextBox.TabIndex = 3;
            this.passwordTextBox.UseSystemPasswordChar = true;
          
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(20, 53);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(52, 15);
            this.passwordLabel.TabIndex = 2;
            this.passwordLabel.Text = "Пароль:";
          
            this.loginTextBox.Location = new System.Drawing.Point(100, 20);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Size = new System.Drawing.Size(190, 23);
            this.loginTextBox.TabIndex = 1;
          
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(20, 23);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(44, 15);
            this.loginLabel.TabIndex = 0;
            this.loginLabel.Text = "Логин:";
          
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.centerPanel);
            this.Name = "LoginControl";
            this.Size = new System.Drawing.Size(600, 400);
            this.centerPanel.ResumeLayout(false);
            this.centerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel centerPanel;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.TextBox loginTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.CheckBox rememberCheckBox;
        private System.Windows.Forms.Button loginButton;
    }
}