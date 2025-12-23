namespace AISchool.Views
{
    partial class StudentProfileControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.notesLabel = new System.Windows.Forms.Label();
            this.detailsTable = new System.Windows.Forms.TableLayoutPanel();
            this.lblClassTitle = new System.Windows.Forms.Label();
            this.classNameLabel = new System.Windows.Forms.Label();
            this.lblBirthDateTitle = new System.Windows.Forms.Label();
            this.birthDateLabel = new System.Windows.Forms.Label();
            this.topFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pbAvatar = new System.Windows.Forms.PictureBox();
            this.avatarButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUploadAvatar = new System.Windows.Forms.Button();
            this.btnDeleteAvatar = new System.Windows.Forms.Button();
            this.studentNameLabel = new System.Windows.Forms.Label();
            this.classmatesComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGrades = new System.Windows.Forms.TabPage();
            this.lessonsGrid = new System.Windows.Forms.DataGridView();
            this.colLessonId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLessonNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLessonDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLessonTopic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGradesLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblGradesHint = new System.Windows.Forms.Label();
            this.gradesTopPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSubject = new System.Windows.Forms.Label();
            this.disciplinesComboBox = new System.Windows.Forms.ComboBox();
            this.lblSearchGrades = new System.Windows.Forms.Label();
            this.lessonsSearchBox = new System.Windows.Forms.TextBox();
            this.tabStats = new System.Windows.Forms.TabPage();
            this.statsGrid = new System.Windows.Forms.DataGridView();
            this.colStatDiscipline = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatAverage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statsTopPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSearchStats = new System.Windows.Forms.Label();
            this.statsSearchBox = new System.Windows.Forms.TextBox();
            this.tabAchievements = new System.Windows.Forms.TabPage();
            this.achievementsGrid = new System.Windows.Forms.DataGridView();
            this.colAchId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAchDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAchLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAchPlace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.achievementsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEditAchievement = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeleteAchievement = new System.Windows.Forms.ToolStripMenuItem();
            this.achievementsTopPanel = new System.Windows.Forms.Panel();
            this.achievementsButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddAchievement = new System.Windows.Forms.Button();
            this.achievementsSearchPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSearchAch = new System.Windows.Forms.Label();
            this.achievementsSearchBox = new System.Windows.Forms.TextBox();
            this.headerPanel.SuspendLayout();
            this.detailsTable.SuspendLayout();
            this.topFlowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).BeginInit();
            this.avatarButtonPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabGrades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lessonsGrid)).BeginInit();
            this.gradesTopPanel.SuspendLayout();
            this.tabStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statsGrid)).BeginInit();
            this.statsTopPanel.SuspendLayout();
            this.tabAchievements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.achievementsGrid)).BeginInit();
            this.achievementsContextMenu.SuspendLayout();
            this.achievementsTopPanel.SuspendLayout();
            this.achievementsButtonPanel.SuspendLayout();
            this.achievementsSearchPanel.SuspendLayout();
            this.SuspendLayout();
         
            this.headerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.headerPanel.Controls.Add(this.notesLabel);
            this.headerPanel.Controls.Add(this.detailsTable);
            this.headerPanel.Controls.Add(this.topFlowPanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(10);
            this.headerPanel.Size = new System.Drawing.Size(1024, 180);
            this.headerPanel.TabIndex = 0;
         
            this.notesLabel.AutoSize = true;
            this.notesLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.notesLabel.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.notesLabel.Location = new System.Drawing.Point(10, 159);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.notesLabel.Size = new System.Drawing.Size(56, 20);
            this.notesLabel.TabIndex = 2;
            this.notesLabel.Text = "Заметки:";
          
            this.detailsTable.AutoSize = true;
            this.detailsTable.ColumnCount = 2;
            this.detailsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.detailsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.detailsTable.Controls.Add(this.lblClassTitle, 0, 0);
            this.detailsTable.Controls.Add(this.classNameLabel, 1, 0);
            this.detailsTable.Controls.Add(this.lblBirthDateTitle, 0, 1);
            this.detailsTable.Controls.Add(this.birthDateLabel, 1, 1);
            this.detailsTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.detailsTable.Location = new System.Drawing.Point(10, 119);
            this.detailsTable.Name = "detailsTable";
            this.detailsTable.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.detailsTable.RowCount = 2;
            this.detailsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.detailsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.detailsTable.Size = new System.Drawing.Size(1002, 40);
            this.detailsTable.TabIndex = 1;
          
            this.lblClassTitle.AutoSize = true;
            this.lblClassTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblClassTitle.Location = new System.Drawing.Point(3, 5);
            this.lblClassTitle.Name = "lblClassTitle";
            this.lblClassTitle.Size = new System.Drawing.Size(43, 15);
            this.lblClassTitle.TabIndex = 0;
            this.lblClassTitle.Text = "Класс:";
          
            this.classNameLabel.AutoSize = true;
            this.classNameLabel.Location = new System.Drawing.Point(108, 5);
            this.classNameLabel.Name = "classNameLabel";
            this.classNameLabel.Size = new System.Drawing.Size(38, 15);
            this.classNameLabel.TabIndex = 1;
            this.classNameLabel.Text = "label1";
          
            this.lblBirthDateTitle.AutoSize = true;
            this.lblBirthDateTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblBirthDateTitle.Location = new System.Drawing.Point(3, 20);
            this.lblBirthDateTitle.Name = "lblBirthDateTitle";
            this.lblBirthDateTitle.Size = new System.Drawing.Size(99, 15);
            this.lblBirthDateTitle.TabIndex = 2;
            this.lblBirthDateTitle.Text = "Дата рождения:";
          
            this.birthDateLabel.AutoSize = true;
            this.birthDateLabel.Location = new System.Drawing.Point(108, 20);
            this.birthDateLabel.Name = "birthDateLabel";
            this.birthDateLabel.Size = new System.Drawing.Size(38, 15);
            this.birthDateLabel.TabIndex = 3;
            this.birthDateLabel.Text = "label2";
         
            this.topFlowPanel.AutoSize = true;
            this.topFlowPanel.Controls.Add(this.pbAvatar);
            this.topFlowPanel.Controls.Add(this.avatarButtonPanel);
            this.topFlowPanel.Controls.Add(this.studentNameLabel);
            this.topFlowPanel.Controls.Add(this.classmatesComboBox);
            this.topFlowPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topFlowPanel.Location = new System.Drawing.Point(10, 10);
            this.topFlowPanel.Name = "topFlowPanel";
            this.topFlowPanel.Size = new System.Drawing.Size(1002, 109);
            this.topFlowPanel.TabIndex = 0;
            this.topFlowPanel.WrapContents = false;
         
            this.pbAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbAvatar.Location = new System.Drawing.Point(3, 3);
            this.pbAvatar.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.pbAvatar.Name = "pbAvatar";
            this.pbAvatar.Size = new System.Drawing.Size(100, 100);
            this.pbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbAvatar.TabIndex = 2;
            this.pbAvatar.TabStop = false;
          
            this.avatarButtonPanel.AutoSize = true;
            this.avatarButtonPanel.Controls.Add(this.btnUploadAvatar);
            this.avatarButtonPanel.Controls.Add(this.btnDeleteAvatar);
            this.avatarButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.avatarButtonPanel.Location = new System.Drawing.Point(113, 3);
            this.avatarButtonPanel.Margin = new System.Windows.Forms.Padding(0, 3, 15, 3);
            this.avatarButtonPanel.Name = "avatarButtonPanel";
            this.avatarButtonPanel.Size = new System.Drawing.Size(106, 62);
            this.avatarButtonPanel.TabIndex = 3;
          
            this.btnUploadAvatar.Location = new System.Drawing.Point(3, 3);
            this.btnUploadAvatar.Name = "btnUploadAvatar";
            this.btnUploadAvatar.Size = new System.Drawing.Size(100, 25);
            this.btnUploadAvatar.TabIndex = 0;
            this.btnUploadAvatar.Text = "Добавить фото";
            this.btnUploadAvatar.UseVisualStyleBackColor = true;
          
            this.btnDeleteAvatar.Location = new System.Drawing.Point(3, 34);
            this.btnDeleteAvatar.Name = "btnDeleteAvatar";
            this.btnDeleteAvatar.Size = new System.Drawing.Size(100, 25);
            this.btnDeleteAvatar.TabIndex = 1;
            this.btnDeleteAvatar.Text = "Удалить фото";
            this.btnDeleteAvatar.UseVisualStyleBackColor = true;
         
            this.studentNameLabel.AutoSize = true;
            this.studentNameLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.studentNameLabel.Location = new System.Drawing.Point(234, 5);
            this.studentNameLabel.Margin = new System.Windows.Forms.Padding(0, 5, 10, 0);
            this.studentNameLabel.Name = "studentNameLabel";
            this.studentNameLabel.Size = new System.Drawing.Size(137, 25);
            this.studentNameLabel.TabIndex = 0;
            this.studentNameLabel.Text = "Student Name";
          
            this.classmatesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classmatesComboBox.FormattingEnabled = true;
            this.classmatesComboBox.Location = new System.Drawing.Point(381, 8);
            this.classmatesComboBox.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.classmatesComboBox.Name = "classmatesComboBox";
            this.classmatesComboBox.Size = new System.Drawing.Size(250, 23);
            this.classmatesComboBox.TabIndex = 1;
          
            this.tabControl.Controls.Add(this.tabGrades);
            this.tabControl.Controls.Add(this.tabStats);
            this.tabControl.Controls.Add(this.tabAchievements);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 180);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1024, 588);
            this.tabControl.TabIndex = 1;
          
            this.tabGrades.Controls.Add(this.lessonsGrid);
            this.tabGrades.Controls.Add(this.lblGradesHint);
            this.tabGrades.Controls.Add(this.gradesTopPanel);
            this.tabGrades.Location = new System.Drawing.Point(4, 24);
            this.tabGrades.Name = "tabGrades";
            this.tabGrades.Padding = new System.Windows.Forms.Padding(3);
            this.tabGrades.Size = new System.Drawing.Size(1016, 560);
            this.tabGrades.TabIndex = 0;
            this.tabGrades.Text = "Успеваемость";
            this.tabGrades.UseVisualStyleBackColor = true;
          
            this.lessonsGrid.AllowUserToAddRows = false;
            this.lessonsGrid.AllowUserToResizeRows = false;
            this.lessonsGrid.AutoGenerateColumns = false;
            this.lessonsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lessonsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lessonsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLessonId,
            this.colLessonNumber,
            this.colLessonDate,
            this.colLessonTopic,
            this.colGradesLine});
            this.lessonsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lessonsGrid.Location = new System.Drawing.Point(3, 43);
            this.lessonsGrid.Name = "lessonsGrid";
            this.lessonsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.lessonsGrid.Size = new System.Drawing.Size(1010, 484);
            this.lessonsGrid.TabIndex = 2;
          
            this.colLessonId.DataPropertyName = "LessonId";
            this.colLessonId.HeaderText = "ID";
            this.colLessonId.Name = "colLessonId";
            this.colLessonId.Visible = false;
          
            this.colLessonNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLessonNumber.DataPropertyName = "LessonNumber";
            this.colLessonNumber.HeaderText = "Занятие №";
            this.colLessonNumber.Name = "colLessonNumber";
            this.colLessonNumber.ReadOnly = true;
            this.colLessonNumber.Width = 91;
         
            this.colLessonDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLessonDate.DataPropertyName = "LessonDate";
            this.colLessonDate.HeaderText = "Дата проведения";
            this.colLessonDate.Name = "colLessonDate";
            this.colLessonDate.Width = 126;
          
            this.colLessonTopic.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLessonTopic.DataPropertyName = "LessonTopic";
            this.colLessonTopic.HeaderText = "Тема";
            this.colLessonTopic.MinimumWidth = 200;
            this.colLessonTopic.Name = "colLessonTopic";
          
            this.colGradesLine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colGradesLine.DataPropertyName = "GradesLine";
            this.colGradesLine.HeaderText = "Оценки";
            this.colGradesLine.MinimumWidth = 150;
            this.colGradesLine.Name = "colGradesLine";
            this.colGradesLine.ReadOnly = true;
            this.colGradesLine.Width = 150;
         
            this.lblGradesHint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblGradesHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblGradesHint.Location = new System.Drawing.Point(3, 527);
            this.lblGradesHint.Name = "lblGradesHint";
            this.lblGradesHint.Size = new System.Drawing.Size(1010, 30);
            this.lblGradesHint.TabIndex = 1;
            this.lblGradesHint.Text = "Подсказка";
            this.lblGradesHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
           
            this.gradesTopPanel.Controls.Add(this.lblSubject);
            this.gradesTopPanel.Controls.Add(this.disciplinesComboBox);
            this.gradesTopPanel.Controls.Add(this.lblSearchGrades);
            this.gradesTopPanel.Controls.Add(this.lessonsSearchBox);
            this.gradesTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradesTopPanel.Location = new System.Drawing.Point(3, 3);
            this.gradesTopPanel.Name = "gradesTopPanel";
            this.gradesTopPanel.Padding = new System.Windows.Forms.Padding(5);
            this.gradesTopPanel.Size = new System.Drawing.Size(1010, 40);
            this.gradesTopPanel.TabIndex = 0;
            this.gradesTopPanel.WrapContents = false;
           
            this.lblSubject.AutoSize = true;
            this.lblSubject.Location = new System.Drawing.Point(5, 11);
            this.lblSubject.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(58, 15);
            this.lblSubject.TabIndex = 0;
            this.lblSubject.Text = "Предмет:";
          
            this.disciplinesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.disciplinesComboBox.FormattingEnabled = true;
            this.disciplinesComboBox.Location = new System.Drawing.Point(66, 8);
            this.disciplinesComboBox.Name = "disciplinesComboBox";
            this.disciplinesComboBox.Size = new System.Drawing.Size(250, 23);
            this.disciplinesComboBox.TabIndex = 1;
        
            this.lblSearchGrades.AutoSize = true;
            this.lblSearchGrades.Location = new System.Drawing.Point(329, 11);
            this.lblSearchGrades.Margin = new System.Windows.Forms.Padding(10, 6, 0, 0);
            this.lblSearchGrades.Name = "lblSearchGrades";
            this.lblSearchGrades.Size = new System.Drawing.Size(92, 15);
            this.lblSearchGrades.TabIndex = 2;
            this.lblSearchGrades.Text = "Поиск по теме:";
          
            this.lessonsSearchBox.Location = new System.Drawing.Point(424, 8);
            this.lessonsSearchBox.Name = "lessonsSearchBox";
            this.lessonsSearchBox.Size = new System.Drawing.Size(200, 23);
            this.lessonsSearchBox.TabIndex = 3;
          
            this.tabStats.Controls.Add(this.statsGrid);
            this.tabStats.Controls.Add(this.statsTopPanel);
            this.tabStats.Location = new System.Drawing.Point(4, 24);
            this.tabStats.Name = "tabStats";
            this.tabStats.Padding = new System.Windows.Forms.Padding(5);
            this.tabStats.Size = new System.Drawing.Size(1016, 560);
            this.tabStats.TabIndex = 1;
            this.tabStats.Text = "Статистика";
            this.tabStats.UseVisualStyleBackColor = true;
          
            this.statsGrid.AllowUserToAddRows = false;
            this.statsGrid.AutoGenerateColumns = false;
            this.statsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.statsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.statsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStatDiscipline,
            this.colStatAverage});
            this.statsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsGrid.Location = new System.Drawing.Point(5, 35);
            this.statsGrid.Name = "statsGrid";
            this.statsGrid.ReadOnly = true;
            this.statsGrid.RowHeadersVisible = false;
            this.statsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.statsGrid.Size = new System.Drawing.Size(1006, 520);
            this.statsGrid.TabIndex = 1;
          
            this.colStatDiscipline.DataPropertyName = "DisciplineName";
            this.colStatDiscipline.HeaderText = "Дисциплина";
            this.colStatDiscipline.Name = "colStatDiscipline";
            this.colStatDiscipline.ReadOnly = true;
         
            this.colStatAverage.DataPropertyName = "AverageGradeValue";
            this.colStatAverage.HeaderText = "Средний балл";
            this.colStatAverage.Name = "colStatAverage";
            this.colStatAverage.ReadOnly = true;
        
            this.statsTopPanel.Controls.Add(this.lblSearchStats);
            this.statsTopPanel.Controls.Add(this.statsSearchBox);
            this.statsTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statsTopPanel.Location = new System.Drawing.Point(5, 5);
            this.statsTopPanel.Name = "statsTopPanel";
            this.statsTopPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.statsTopPanel.Size = new System.Drawing.Size(1006, 30);
            this.statsTopPanel.TabIndex = 0;
         
            this.lblSearchStats.AutoSize = true;
            this.lblSearchStats.Location = new System.Drawing.Point(0, 3);
            this.lblSearchStats.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblSearchStats.Name = "lblSearchStats";
            this.lblSearchStats.Size = new System.Drawing.Size(127, 15);
            this.lblSearchStats.TabIndex = 0;
            this.lblSearchStats.Text = "Поиск по дисциплине:";
          
            this.statsSearchBox.Location = new System.Drawing.Point(130, 3);
            this.statsSearchBox.Name = "statsSearchBox";
            this.statsSearchBox.Size = new System.Drawing.Size(250, 23);
            this.statsSearchBox.TabIndex = 1;
         
            this.tabAchievements.Controls.Add(this.achievementsGrid);
            this.tabAchievements.Controls.Add(this.achievementsTopPanel);
            this.tabAchievements.Location = new System.Drawing.Point(4, 24);
            this.tabAchievements.Name = "tabAchievements";
            this.tabAchievements.Padding = new System.Windows.Forms.Padding(5);
            this.tabAchievements.Size = new System.Drawing.Size(1016, 560);
            this.tabAchievements.TabIndex = 2;
            this.tabAchievements.Text = "Достижения";
            this.tabAchievements.UseVisualStyleBackColor = true;
          
            this.achievementsGrid.AllowUserToAddRows = false;
            this.achievementsGrid.AutoGenerateColumns = false;
            this.achievementsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.achievementsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.achievementsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.achievementsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAchId,
            this.colAchDate,
            this.colAchName,
            this.colAchLevel,
            this.colAchPlace});
            this.achievementsGrid.ContextMenuStrip = this.achievementsContextMenu;
            this.achievementsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.achievementsGrid.Location = new System.Drawing.Point(5, 45);
            this.achievementsGrid.Name = "achievementsGrid";
            this.achievementsGrid.ReadOnly = true;
            this.achievementsGrid.RowHeadersVisible = false;
            this.achievementsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.achievementsGrid.Size = new System.Drawing.Size(1006, 510);
            this.achievementsGrid.TabIndex = 1;
           
            this.colAchId.DataPropertyName = "Id";
            this.colAchId.HeaderText = "Id";
            this.colAchId.Name = "colAchId";
            this.colAchId.ReadOnly = true;
            this.colAchId.Visible = false;
          
            this.colAchDate.DataPropertyName = "EventDate";
            dataGridViewCellStyle1.Format = "d";
            dataGridViewCellStyle1.NullValue = null;
            this.colAchDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.colAchDate.HeaderText = "Дата";
            this.colAchDate.Name = "colAchDate";
            this.colAchDate.ReadOnly = true;
           
            this.colAchName.DataPropertyName = "EventName";
            this.colAchName.HeaderText = "Мероприятие";
            this.colAchName.Name = "colAchName";
            this.colAchName.ReadOnly = true;
         
            this.colAchLevel.DataPropertyName = "Level";
            this.colAchLevel.HeaderText = "Уровень";
            this.colAchLevel.Name = "colAchLevel";
            this.colAchLevel.ReadOnly = true;
          
            this.colAchPlace.DataPropertyName = "Place";
            this.colAchPlace.HeaderText = "Место";
            this.colAchPlace.Name = "colAchPlace";
            this.colAchPlace.ReadOnly = true;
          
            this.achievementsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditAchievement,
            this.menuDeleteAchievement});
            this.achievementsContextMenu.Name = "achievementsContextMenu";
            this.achievementsContextMenu.Size = new System.Drawing.Size(217, 48);
         
            this.menuEditAchievement.Name = "menuEditAchievement";
            this.menuEditAchievement.Size = new System.Drawing.Size(216, 22);
            this.menuEditAchievement.Text = "Редактировать достижение";
          
            this.menuDeleteAchievement.Name = "menuDeleteAchievement";
            this.menuDeleteAchievement.Size = new System.Drawing.Size(216, 22);
            this.menuDeleteAchievement.Text = "Удалить достижение";
          
            this.achievementsTopPanel.Controls.Add(this.achievementsButtonPanel);
            this.achievementsTopPanel.Controls.Add(this.achievementsSearchPanel);
            this.achievementsTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.achievementsTopPanel.Location = new System.Drawing.Point(5, 5);
            this.achievementsTopPanel.Name = "achievementsTopPanel";
            this.achievementsTopPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.achievementsTopPanel.Size = new System.Drawing.Size(1006, 40);
            this.achievementsTopPanel.TabIndex = 0;
          
            this.achievementsButtonPanel.AutoSize = true;
            this.achievementsButtonPanel.Controls.Add(this.btnAddAchievement);
            this.achievementsButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.achievementsButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.achievementsButtonPanel.Location = new System.Drawing.Point(850, 5);
            this.achievementsButtonPanel.Name = "achievementsButtonPanel";
            this.achievementsButtonPanel.Size = new System.Drawing.Size(156, 30);
            this.achievementsButtonPanel.TabIndex = 1;
          
            this.btnAddAchievement.Location = new System.Drawing.Point(3, 3);
            this.btnAddAchievement.Name = "btnAddAchievement";
            this.btnAddAchievement.Size = new System.Drawing.Size(150, 23);
            this.btnAddAchievement.TabIndex = 0;
            this.btnAddAchievement.Text = "Добавить достижение";
            this.btnAddAchievement.UseVisualStyleBackColor = true;
          
            this.achievementsSearchPanel.AutoSize = true;
            this.achievementsSearchPanel.Controls.Add(this.lblSearchAch);
            this.achievementsSearchPanel.Controls.Add(this.achievementsSearchBox);
            this.achievementsSearchPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.achievementsSearchPanel.Location = new System.Drawing.Point(0, 5);
            this.achievementsSearchPanel.Name = "achievementsSearchPanel";
            this.achievementsSearchPanel.Padding = new System.Windows.Forms.Padding(5, 3, 0, 0);
            this.achievementsSearchPanel.Size = new System.Drawing.Size(374, 30);
            this.achievementsSearchPanel.TabIndex = 0;
            this.achievementsSearchPanel.WrapContents = false;
          
            this.lblSearchAch.AutoSize = true;
            this.lblSearchAch.Location = new System.Drawing.Point(5, 3);
            this.lblSearchAch.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblSearchAch.Name = "lblSearchAch";
            this.lblSearchAch.Size = new System.Drawing.Size(113, 15);
            this.lblSearchAch.TabIndex = 0;
            this.lblSearchAch.Text = "Поиск по названию:";
         
            this.achievementsSearchBox.Location = new System.Drawing.Point(121, 3);
            this.achievementsSearchBox.Name = "achievementsSearchBox";
            this.achievementsSearchBox.Size = new System.Drawing.Size(250, 23);
            this.achievementsSearchBox.TabIndex = 1;
          
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.headerPanel);
            this.Name = "StudentProfileControl";
            this.Size = new System.Drawing.Size(1024, 768);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.detailsTable.ResumeLayout(false);
            this.detailsTable.PerformLayout();
            this.topFlowPanel.ResumeLayout(false);
            this.topFlowPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).EndInit();
            this.avatarButtonPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabGrades.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lessonsGrid)).EndInit();
            this.gradesTopPanel.ResumeLayout(false);
            this.gradesTopPanel.PerformLayout();
            this.tabStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statsGrid)).EndInit();
            this.statsTopPanel.ResumeLayout(false);
            this.statsTopPanel.PerformLayout();
            this.tabAchievements.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.achievementsGrid)).EndInit();
            this.achievementsTopPanel.ResumeLayout(false);
            this.achievementsTopPanel.PerformLayout();
            this.achievementsButtonPanel.ResumeLayout(false);
            this.achievementsSearchPanel.ResumeLayout(false);
            this.achievementsSearchPanel.PerformLayout();
            this.achievementsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.FlowLayoutPanel topFlowPanel;
        private System.Windows.Forms.Label studentNameLabel;
        private System.Windows.Forms.ComboBox classmatesComboBox;
        private System.Windows.Forms.TableLayoutPanel detailsTable;
        private System.Windows.Forms.Label lblClassTitle;
        private System.Windows.Forms.Label classNameLabel;
        private System.Windows.Forms.Label lblBirthDateTitle;
        private System.Windows.Forms.Label birthDateLabel;
        private System.Windows.Forms.Label notesLabel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGrades;
        private System.Windows.Forms.TabPage tabStats;
        private System.Windows.Forms.TabPage tabAchievements;

        private System.Windows.Forms.PictureBox pbAvatar;
        private System.Windows.Forms.FlowLayoutPanel avatarButtonPanel;
        private System.Windows.Forms.Button btnUploadAvatar;
        private System.Windows.Forms.Button btnDeleteAvatar;

        private System.Windows.Forms.FlowLayoutPanel gradesTopPanel;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.ComboBox disciplinesComboBox;
        private System.Windows.Forms.Label lblSearchGrades;
        private System.Windows.Forms.TextBox lessonsSearchBox;
        private System.Windows.Forms.DataGridView lessonsGrid;
        private System.Windows.Forms.Label lblGradesHint;
        
        private System.Windows.Forms.FlowLayoutPanel statsTopPanel;
        private System.Windows.Forms.Label lblSearchStats;
        private System.Windows.Forms.TextBox statsSearchBox;
        private System.Windows.Forms.DataGridView statsGrid;

        private System.Windows.Forms.Panel achievementsTopPanel;
        private System.Windows.Forms.FlowLayoutPanel achievementsButtonPanel;
        private System.Windows.Forms.Button btnAddAchievement;
        private System.Windows.Forms.FlowLayoutPanel achievementsSearchPanel;
        private System.Windows.Forms.Label lblSearchAch;
        private System.Windows.Forms.TextBox achievementsSearchBox;
        private System.Windows.Forms.DataGridView achievementsGrid;
        private System.Windows.Forms.ContextMenuStrip achievementsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuEditAchievement;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteAchievement;

        private System.Windows.Forms.DataGridViewTextBoxColumn colLessonId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLessonNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLessonDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLessonTopic;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGradesLine;

        private System.Windows.Forms.DataGridViewTextBoxColumn colStatDiscipline;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatAverage;

        private System.Windows.Forms.DataGridViewTextBoxColumn colAchId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAchDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAchLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAchPlace;
    }
}