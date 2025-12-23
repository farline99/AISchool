namespace AISchool.Views
{
    partial class EditStudentGradesDialog
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
            this.infoLabel = new System.Windows.Forms.Label();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.addButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.gradesListBox = new System.Windows.Forms.ListBox();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
          
            this.infoLabel.AutoSize = true;
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.infoLabel.Location = new System.Drawing.Point(0, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Padding = new System.Windows.Forms.Padding(10);
            this.infoLabel.Size = new System.Drawing.Size(117, 35);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Student Info\r\nLesson Info";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
          
            this.buttonPanel.Controls.Add(this.addButton);
            this.buttonPanel.Controls.Add(this.editButton);
            this.buttonPanel.Controls.Add(this.deleteButton);
            this.buttonPanel.Controls.Add(this.closeButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 271);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Padding = new System.Windows.Forms.Padding(5);
            this.buttonPanel.Size = new System.Drawing.Size(384, 40);
            this.buttonPanel.TabIndex = 1;
           
            this.addButton.Location = new System.Drawing.Point(5, 8);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 23);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Добавить";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
           
            this.editButton.Enabled = false;
            this.editButton.Location = new System.Drawing.Point(90, 8);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(80, 23);
            this.editButton.TabIndex = 1;
            this.editButton.Text = "Изменить";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
          
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(175, 8);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 23);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "Удалить";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
          
            this.closeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.closeButton.Location = new System.Drawing.Point(279, 5);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(100, 30);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Готово";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
          
            this.gradesListBox.DisplayMember = "DisplayValue";
            this.gradesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradesListBox.FormattingEnabled = true;
            this.gradesListBox.ItemHeight = 15;
            this.gradesListBox.Location = new System.Drawing.Point(0, 35);
            this.gradesListBox.Name = "gradesListBox";
            this.gradesListBox.Size = new System.Drawing.Size(384, 236);
            this.gradesListBox.TabIndex = 2;
            this.gradesListBox.SelectedIndexChanged += new System.EventHandler(this.GradesListBox_SelectedIndexChanged);
          
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 311);
            this.Controls.Add(this.gradesListBox);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.infoLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditStudentGradesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактирование оценок";
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ListBox gradesListBox;
    }
}