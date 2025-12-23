namespace AISchool.Views
{
    partial class TeacherDashboardControl
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
            this.components = new System.ComponentModel.Container();
            this.topPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblClass = new System.Windows.Forms.Label();
            this.classComboBox = new System.Windows.Forms.ComboBox();
            this.lblDiscipline = new System.Windows.Forms.Label();
            this.disciplineComboBox = new System.Windows.Forms.ComboBox();
            this.navigationPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPrevMonth = new System.Windows.Forms.Button();
            this.lblMonth = new System.Windows.Forms.Label();
            this.btnNextMonth = new System.Windows.Forms.Button();
            this.lblGoToDate = new System.Windows.Forms.Label();
            this.journalDatePicker = new System.Windows.Forms.DateTimePicker();
            this.journalGrid = new System.Windows.Forms.DataGridView();
            this.headerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEditLesson = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSetGradeAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetAbsenceAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuClearGrades = new System.Windows.Forms.ToolStripMenuItem();
            this.navToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.topPanel.SuspendLayout();
            this.navigationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.journalGrid)).BeginInit();
            this.headerContextMenu.SuspendLayout();
            this.SuspendLayout();
         
            this.topPanel.Controls.Add(this.lblClass);
            this.topPanel.Controls.Add(this.classComboBox);
            this.topPanel.Controls.Add(this.lblDiscipline);
            this.topPanel.Controls.Add(this.disciplineComboBox);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(5, 5);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.topPanel.Size = new System.Drawing.Size(1014, 40);
            this.topPanel.TabIndex = 0;
            this.topPanel.WrapContents = false;
          
            this.lblClass.AutoSize = true;
            this.lblClass.Location = new System.Drawing.Point(0, 11);
            this.lblClass.Margin = new System.Windows.Forms.Padding(0, 6, 5, 0);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(42, 15);
            this.lblClass.TabIndex = 0;
            this.lblClass.Text = "Класс:";
         
            this.classComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classComboBox.FormattingEnabled = true;
            this.classComboBox.Location = new System.Drawing.Point(50, 8);
            this.classComboBox.Name = "classComboBox";
            this.classComboBox.Size = new System.Drawing.Size(150, 23);
            this.classComboBox.TabIndex = 1;
        
            this.lblDiscipline.AutoSize = true;
            this.lblDiscipline.Location = new System.Drawing.Point(218, 11);
            this.lblDiscipline.Margin = new System.Windows.Forms.Padding(15, 6, 5, 0);
            this.lblDiscipline.Name = "lblDiscipline";
            this.lblDiscipline.Size = new System.Drawing.Size(58, 15);
            this.lblDiscipline.TabIndex = 2;
            this.lblDiscipline.Text = "Предмет:";
         
            this.disciplineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.disciplineComboBox.FormattingEnabled = true;
            this.disciplineComboBox.Location = new System.Drawing.Point(284, 8);
            this.disciplineComboBox.Name = "disciplineComboBox";
            this.disciplineComboBox.Size = new System.Drawing.Size(200, 23);
            this.disciplineComboBox.TabIndex = 3;
          
            this.navigationPanel.Controls.Add(this.btnPrevMonth);
            this.navigationPanel.Controls.Add(this.lblMonth);
            this.navigationPanel.Controls.Add(this.btnNextMonth);
            this.navigationPanel.Controls.Add(this.lblGoToDate);
            this.navigationPanel.Controls.Add(this.journalDatePicker);
            this.navigationPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.navigationPanel.Location = new System.Drawing.Point(5, 45);
            this.navigationPanel.Name = "navigationPanel";
            this.navigationPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.navigationPanel.Size = new System.Drawing.Size(1014, 40);
            this.navigationPanel.TabIndex = 1;
            this.navigationPanel.WrapContents = false;
          
            this.btnPrevMonth.Location = new System.Drawing.Point(3, 8);
            this.btnPrevMonth.Name = "btnPrevMonth";
            this.btnPrevMonth.Size = new System.Drawing.Size(50, 23);
            this.btnPrevMonth.TabIndex = 0;
            this.btnPrevMonth.Text = "<<";
            this.navToolTip.SetToolTip(this.btnPrevMonth, "Предыдущий месяц");
            this.btnPrevMonth.UseVisualStyleBackColor = true;
           
            this.lblMonth.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMonth.Location = new System.Drawing.Point(56, 11);
            this.lblMonth.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(150, 20);
            this.lblMonth.TabIndex = 1;
            this.lblMonth.Text = "Месяц Год";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
          
            this.btnNextMonth.Location = new System.Drawing.Point(209, 8);
            this.btnNextMonth.Name = "btnNextMonth";
            this.btnNextMonth.Size = new System.Drawing.Size(50, 23);
            this.btnNextMonth.TabIndex = 2;
            this.btnNextMonth.Text = ">>";
            this.navToolTip.SetToolTip(this.btnNextMonth, "Следующий месяц");
            this.btnNextMonth.UseVisualStyleBackColor = true;
          
            this.lblGoToDate.AutoSize = true;
            this.lblGoToDate.Location = new System.Drawing.Point(282, 11);
            this.lblGoToDate.Margin = new System.Windows.Forms.Padding(20, 6, 5, 0);
            this.lblGoToDate.Name = "lblGoToDate";
            this.lblGoToDate.Size = new System.Drawing.Size(92, 15);
            this.lblGoToDate.TabIndex = 3;
            this.lblGoToDate.Text = "Перейти к дате:";
         
            this.journalDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.journalDatePicker.Location = new System.Drawing.Point(382, 8);
            this.journalDatePicker.Name = "journalDatePicker";
            this.journalDatePicker.Size = new System.Drawing.Size(200, 23);
            this.journalDatePicker.TabIndex = 4;
         
            this.journalGrid.AllowUserToAddRows = false;
            this.journalGrid.AllowUserToDeleteRows = false;
            this.journalGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.journalGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.journalGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.journalGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.journalGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.journalGrid.Location = new System.Drawing.Point(5, 85);
            this.journalGrid.Name = "journalGrid";
            this.journalGrid.RowHeadersVisible = false;
            this.journalGrid.Size = new System.Drawing.Size(1014, 678);
            this.journalGrid.TabIndex = 2;
        
            this.headerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditLesson,
            this.toolStripSeparator1,
            this.menuSetGradeAll,
            this.menuSetAbsenceAll,
            this.toolStripSeparator2,
            this.menuClearGrades});
            this.headerContextMenu.Name = "headerContextMenu";
            this.headerContextMenu.Size = new System.Drawing.Size(262, 104);
         
            this.menuEditLesson.Name = "menuEditLesson";
            this.menuEditLesson.Size = new System.Drawing.Size(261, 22);
            this.menuEditLesson.Text = "Редактировать урок...";
          
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(258, 6);
          
            this.menuSetGradeAll.Name = "menuSetGradeAll";
            this.menuSetGradeAll.Size = new System.Drawing.Size(261, 22);
            this.menuSetGradeAll.Text = "Выставить оценку всему классу...";
          
            this.menuSetAbsenceAll.Name = "menuSetAbsenceAll";
            this.menuSetAbsenceAll.Size = new System.Drawing.Size(261, 22);
            this.menuSetAbsenceAll.Text = "Отметить отсутствующих (Н)";
         
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(258, 6);
         
            this.menuClearGrades.Name = "menuClearGrades";
            this.menuClearGrades.Size = new System.Drawing.Size(261, 22);
            this.menuClearGrades.Text = "Очистить оценки за урок";
         
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.journalGrid);
            this.Controls.Add(this.navigationPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "TeacherDashboardControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(1024, 768);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.navigationPanel.ResumeLayout(false);
            this.navigationPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.journalGrid)).EndInit();
            this.headerContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.FlowLayoutPanel topPanel;
        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.ComboBox classComboBox;
        private System.Windows.Forms.Label lblDiscipline;
        private System.Windows.Forms.ComboBox disciplineComboBox;
        private System.Windows.Forms.FlowLayoutPanel navigationPanel;
        private System.Windows.Forms.Button btnPrevMonth;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Button btnNextMonth;
        private System.Windows.Forms.Label lblGoToDate;
        private System.Windows.Forms.DateTimePicker journalDatePicker;
        private System.Windows.Forms.DataGridView journalGrid;
        private System.Windows.Forms.ToolTip navToolTip;
        private System.Windows.Forms.ContextMenuStrip headerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuEditLesson;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuSetGradeAll;
        private System.Windows.Forms.ToolStripMenuItem menuSetAbsenceAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuClearGrades;
    }
}