namespace AISchool.Views
{
    partial class AdminDashboardControl
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabTeachers = new System.Windows.Forms.TabPage();
            this.teachersGrid = new System.Windows.Forms.DataGridView();
            this.teachersTopPanel = new System.Windows.Forms.Panel();
            this.teachersButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDeleteTeacher = new System.Windows.Forms.Button();
            this.btnEditTeacher = new System.Windows.Forms.Button();
            this.btnAddTeacher = new System.Windows.Forms.Button();
            this.teachersSearchPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSearchTeacher = new System.Windows.Forms.Label();
            this.teacherSearchTextBox = new System.Windows.Forms.TextBox();
            this.tabClasses = new System.Windows.Forms.TabPage();
            this.splitClassesMain = new System.Windows.Forms.SplitContainer();
            this.classesPanel = new System.Windows.Forms.Panel();
            this.classesGrid = new System.Windows.Forms.DataGridView();
            this.classesTopPanel = new System.Windows.Forms.Panel();
            this.classesButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnEditClass = new System.Windows.Forms.Button();
            this.btnAddClass = new System.Windows.Forms.Button();
            this.classesSearchPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSearchClass = new System.Windows.Forms.Label();
            this.classSearchTextBox = new System.Windows.Forms.TextBox();
            this.splitStudentsParents = new System.Windows.Forms.SplitContainer();
            this.studentsPanel = new System.Windows.Forms.Panel();
            this.studentsGrid = new System.Windows.Forms.DataGridView();
            this.studentsTopPanel = new System.Windows.Forms.Panel();
            this.studentsButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTransferStudent = new System.Windows.Forms.Button();
            this.btnExpelStudent = new System.Windows.Forms.Button();
            this.btnEditStudent = new System.Windows.Forms.Button();
            this.btnAddStudent = new System.Windows.Forms.Button();
            this.studentsSearchPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSearchStudent = new System.Windows.Forms.Label();
            this.studentSearchTextBox = new System.Windows.Forms.TextBox();
            this.parentsPanel = new System.Windows.Forms.Panel();
            this.parentsGrid = new System.Windows.Forms.DataGridView();
            this.lblParentsHeader = new System.Windows.Forms.Label();
            this.parentsButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUnlinkParent = new System.Windows.Forms.Button();
            this.btnEditParent = new System.Windows.Forms.Button();
            this.btnLinkParent = new System.Windows.Forms.Button();
            this.btnAddNewParent = new System.Windows.Forms.Button();
            this.tabPlans = new System.Windows.Forms.TabPage();
            this.splitPlans = new System.Windows.Forms.SplitContainer();
            this.studyPlansGrid = new System.Windows.Forms.DataGridView();
            this.lblPlansHeader = new System.Windows.Forms.Label();
            this.plansTopPanel = new System.Windows.Forms.Panel();
            this.plansButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddStudyPlan = new System.Windows.Forms.Button();
            this.studyPlanItemsGrid = new System.Windows.Forms.DataGridView();
            this.planItemsTopPanel = new System.Windows.Forms.Panel();
            this.lblPlanItemsHeader = new System.Windows.Forms.Label();
            this.planItemsButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDeletePlanItem = new System.Windows.Forms.Button();
            this.btnAddPlanItem = new System.Windows.Forms.Button();
            this.tabWorkload = new System.Windows.Forms.TabPage();
            this.workloadGrid = new System.Windows.Forms.DataGridView();
            this.workloadTopPanel = new System.Windows.Forms.Panel();
            this.workloadButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRemoveTeacherFromWorkload = new System.Windows.Forms.Button();
            this.btnAssignTeacherToWorkload = new System.Windows.Forms.Button();
            this.workloadFilterPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblWorkloadClass = new System.Windows.Forms.Label();
            this.workloadClassComboBox = new System.Windows.Forms.ComboBox();
            this.lblWorkloadYear = new System.Windows.Forms.Label();
            this.yearComboBox = new System.Windows.Forms.ComboBox();
            this.tabAcademicYear = new System.Windows.Forms.TabPage();
            this.academicYearsGrid = new System.Windows.Forms.DataGridView();
            this.academicYearsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuSetCurrentYear = new System.Windows.Forms.ToolStripMenuItem();
            this.menuArchiveYear = new System.Windows.Forms.ToolStripMenuItem();
            this.academicYearsTopPanel = new System.Windows.Forms.Panel();
            this.academicYearsButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPromoteStudents = new System.Windows.Forms.Button();
            this.btnCreateYear = new System.Windows.Forms.Button();
            this.tabReports = new System.Windows.Forms.TabPage();
            this.splitReports = new System.Windows.Forms.SplitContainer();
            this.reportsListBox = new System.Windows.Forms.ListBox();
            this.lblReportsHeader = new System.Windows.Forms.Label();
            this.reportsResultGrid = new System.Windows.Forms.DataGridView();
            this.reportsFilterPanel = new System.Windows.Forms.Panel();
            this.tabControl.SuspendLayout();
            this.tabTeachers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teachersGrid)).BeginInit();
            this.teachersTopPanel.SuspendLayout();
            this.teachersButtonPanel.SuspendLayout();
            this.teachersSearchPanel.SuspendLayout();
            this.tabClasses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitClassesMain)).BeginInit();
            this.splitClassesMain.Panel1.SuspendLayout();
            this.splitClassesMain.Panel2.SuspendLayout();
            this.splitClassesMain.SuspendLayout();
            this.classesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.classesGrid)).BeginInit();
            this.classesTopPanel.SuspendLayout();
            this.classesButtonPanel.SuspendLayout();
            this.classesSearchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitStudentsParents)).BeginInit();
            this.splitStudentsParents.Panel1.SuspendLayout();
            this.splitStudentsParents.Panel2.SuspendLayout();
            this.splitStudentsParents.SuspendLayout();
            this.studentsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.studentsGrid)).BeginInit();
            this.studentsTopPanel.SuspendLayout();
            this.studentsButtonPanel.SuspendLayout();
            this.studentsSearchPanel.SuspendLayout();
            this.parentsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parentsGrid)).BeginInit();
            this.parentsButtonPanel.SuspendLayout();
            this.tabPlans.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPlans)).BeginInit();
            this.splitPlans.Panel1.SuspendLayout();
            this.splitPlans.Panel2.SuspendLayout();
            this.splitPlans.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.studyPlansGrid)).BeginInit();
            this.plansTopPanel.SuspendLayout();
            this.plansButtonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.studyPlanItemsGrid)).BeginInit();
            this.planItemsTopPanel.SuspendLayout();
            this.planItemsButtonPanel.SuspendLayout();
            this.tabWorkload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workloadGrid)).BeginInit();
            this.workloadTopPanel.SuspendLayout();
            this.workloadButtonPanel.SuspendLayout();
            this.workloadFilterPanel.SuspendLayout();
            this.tabAcademicYear.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.academicYearsGrid)).BeginInit();
            this.academicYearsContextMenu.SuspendLayout();
            this.academicYearsTopPanel.SuspendLayout();
            this.academicYearsButtonPanel.SuspendLayout();
            this.tabReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitReports)).BeginInit();
            this.splitReports.Panel1.SuspendLayout();
            this.splitReports.Panel2.SuspendLayout();
            this.splitReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reportsResultGrid)).BeginInit();
            this.SuspendLayout();

            this.tabControl.Controls.Add(this.tabTeachers);
            this.tabControl.Controls.Add(this.tabClasses);
            this.tabControl.Controls.Add(this.tabPlans);
            this.tabControl.Controls.Add(this.tabWorkload);
            this.tabControl.Controls.Add(this.tabAcademicYear);
            this.tabControl.Controls.Add(this.tabReports);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1024, 768);
            this.tabControl.TabIndex = 0;

            this.tabTeachers.Controls.Add(this.teachersGrid);
            this.tabTeachers.Controls.Add(this.teachersTopPanel);
            this.tabTeachers.Location = new System.Drawing.Point(4, 24);
            this.tabTeachers.Name = "tabTeachers";
            this.tabTeachers.Padding = new System.Windows.Forms.Padding(5);
            this.tabTeachers.Size = new System.Drawing.Size(1016, 740);
            this.tabTeachers.TabIndex = 0;
            this.tabTeachers.Text = "Учителя";
            this.tabTeachers.UseVisualStyleBackColor = true;

            this.teachersGrid.AllowUserToAddRows = false;
            this.teachersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.teachersGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.teachersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.teachersGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teachersGrid.Location = new System.Drawing.Point(5, 45);
            this.teachersGrid.Name = "teachersGrid";
            this.teachersGrid.ReadOnly = true;
            this.teachersGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.teachersGrid.Size = new System.Drawing.Size(1006, 690);
            this.teachersGrid.TabIndex = 1;

            this.teachersTopPanel.Controls.Add(this.teachersButtonPanel);
            this.teachersTopPanel.Controls.Add(this.teachersSearchPanel);
            this.teachersTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.teachersTopPanel.Location = new System.Drawing.Point(5, 5);
            this.teachersTopPanel.Name = "teachersTopPanel";
            this.teachersTopPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.teachersTopPanel.Size = new System.Drawing.Size(1006, 40);
            this.teachersTopPanel.TabIndex = 0;

            this.teachersButtonPanel.Controls.Add(this.btnDeleteTeacher);
            this.teachersButtonPanel.Controls.Add(this.btnEditTeacher);
            this.teachersButtonPanel.Controls.Add(this.btnAddTeacher);
            this.teachersButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.teachersButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.teachersButtonPanel.Location = new System.Drawing.Point(606, 5);
            this.teachersButtonPanel.Name = "teachersButtonPanel";
            this.teachersButtonPanel.Size = new System.Drawing.Size(400, 30);
            this.teachersButtonPanel.TabIndex = 1;

            this.btnDeleteTeacher.Location = new System.Drawing.Point(277, 3);
            this.btnDeleteTeacher.Name = "btnDeleteTeacher";
            this.btnDeleteTeacher.Size = new System.Drawing.Size(120, 23);
            this.btnDeleteTeacher.TabIndex = 0;
            this.btnDeleteTeacher.Text = "Удалить";
            this.btnDeleteTeacher.UseVisualStyleBackColor = true;

            this.btnEditTeacher.Location = new System.Drawing.Point(151, 3);
            this.btnEditTeacher.Name = "btnEditTeacher";
            this.btnEditTeacher.Size = new System.Drawing.Size(120, 23);
            this.btnEditTeacher.TabIndex = 1;
            this.btnEditTeacher.Text = "Редактировать";
            this.btnEditTeacher.UseVisualStyleBackColor = true;

            this.btnAddTeacher.Location = new System.Drawing.Point(25, 3);
            this.btnAddTeacher.Name = "btnAddTeacher";
            this.btnAddTeacher.Size = new System.Drawing.Size(120, 23);
            this.btnAddTeacher.TabIndex = 2;
            this.btnAddTeacher.Text = "Добавить";
            this.btnAddTeacher.UseVisualStyleBackColor = true;

            this.teachersSearchPanel.AutoSize = true;
            this.teachersSearchPanel.Controls.Add(this.lblSearchTeacher);
            this.teachersSearchPanel.Controls.Add(this.teacherSearchTextBox);
            this.teachersSearchPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.teachersSearchPanel.Location = new System.Drawing.Point(0, 5);
            this.teachersSearchPanel.Name = "teachersSearchPanel";
            this.teachersSearchPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.teachersSearchPanel.Size = new System.Drawing.Size(306, 30);
            this.teachersSearchPanel.TabIndex = 0;
            this.teachersSearchPanel.WrapContents = false;

            this.lblSearchTeacher.AutoSize = true;
            this.lblSearchTeacher.Location = new System.Drawing.Point(0, 3);
            this.lblSearchTeacher.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblSearchTeacher.Name = "lblSearchTeacher";
            this.lblSearchTeacher.Size = new System.Drawing.Size(45, 15);
            this.lblSearchTeacher.TabIndex = 0;
            this.lblSearchTeacher.Text = "Поиск:";

            this.teacherSearchTextBox.Location = new System.Drawing.Point(50, 3);
            this.teacherSearchTextBox.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.teacherSearchTextBox.Name = "teacherSearchTextBox";
            this.teacherSearchTextBox.Size = new System.Drawing.Size(250, 23);
            this.teacherSearchTextBox.TabIndex = 1;

            this.tabClasses.Controls.Add(this.splitClassesMain);
            this.tabClasses.Location = new System.Drawing.Point(4, 24);
            this.tabClasses.Name = "tabClasses";
            this.tabClasses.Size = new System.Drawing.Size(1016, 740);
            this.tabClasses.TabIndex = 1;
            this.tabClasses.Text = "Классы и Ученики";
            this.tabClasses.UseVisualStyleBackColor = true;

            this.splitClassesMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitClassesMain.Location = new System.Drawing.Point(0, 0);
            this.splitClassesMain.Name = "splitClassesMain";
            this.splitClassesMain.Orientation = System.Windows.Forms.Orientation.Horizontal;

            this.splitClassesMain.Panel1.Controls.Add(this.classesPanel);
            this.splitClassesMain.Panel1.Padding = new System.Windows.Forms.Padding(5);

            this.splitClassesMain.Panel2.Controls.Add(this.splitStudentsParents);
            this.splitClassesMain.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitClassesMain.Size = new System.Drawing.Size(1016, 740);
            this.splitClassesMain.SplitterDistance = 244;
            this.splitClassesMain.TabIndex = 0;

            this.classesPanel.Controls.Add(this.classesGrid);
            this.classesPanel.Controls.Add(this.classesTopPanel);
            this.classesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classesPanel.Location = new System.Drawing.Point(5, 5);
            this.classesPanel.Name = "classesPanel";
            this.classesPanel.Size = new System.Drawing.Size(1006, 234);
            this.classesPanel.TabIndex = 0;

            this.classesGrid.AllowUserToAddRows = false;
            this.classesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.classesGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.classesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.classesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classesGrid.Location = new System.Drawing.Point(0, 40);
            this.classesGrid.Name = "classesGrid";
            this.classesGrid.ReadOnly = true;
            this.classesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.classesGrid.Size = new System.Drawing.Size(1006, 194);
            this.classesGrid.TabIndex = 1;

            this.classesTopPanel.Controls.Add(this.classesButtonPanel);
            this.classesTopPanel.Controls.Add(this.classesSearchPanel);
            this.classesTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.classesTopPanel.Location = new System.Drawing.Point(0, 0);
            this.classesTopPanel.Name = "classesTopPanel";
            this.classesTopPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.classesTopPanel.Size = new System.Drawing.Size(1006, 40);
            this.classesTopPanel.TabIndex = 0;

            this.classesButtonPanel.Controls.Add(this.btnEditClass);
            this.classesButtonPanel.Controls.Add(this.btnAddClass);
            this.classesButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.classesButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.classesButtonPanel.Location = new System.Drawing.Point(756, 5);
            this.classesButtonPanel.Name = "classesButtonPanel";
            this.classesButtonPanel.Size = new System.Drawing.Size(300, 30);
            this.classesButtonPanel.TabIndex = 1;

            this.btnEditClass.Location = new System.Drawing.Point(127, 3);
            this.btnEditClass.Name = "btnEditClass";
            this.btnEditClass.Size = new System.Drawing.Size(120, 23);
            this.btnEditClass.TabIndex = 0;
            this.btnEditClass.Text = "Назначить";
            this.btnEditClass.UseVisualStyleBackColor = true;
            
            this.btnAddClass.Location = new System.Drawing.Point(1, 3);
            this.btnAddClass.Name = "btnAddClass";
            this.btnAddClass.Size = new System.Drawing.Size(120, 23);
            this.btnAddClass.TabIndex = 1;
            this.btnAddClass.Text = "Создать класс";
            this.btnAddClass.UseVisualStyleBackColor = true;
            
            this.classesSearchPanel.AutoSize = true;
            this.classesSearchPanel.Controls.Add(this.lblSearchClass);
            this.classesSearchPanel.Controls.Add(this.classSearchTextBox);
            this.classesSearchPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.classesSearchPanel.Location = new System.Drawing.Point(0, 5);
            this.classesSearchPanel.Name = "classesSearchPanel";
            this.classesSearchPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.classesSearchPanel.Size = new System.Drawing.Size(306, 30);
            this.classesSearchPanel.TabIndex = 0;
            this.classesSearchPanel.WrapContents = false;
           
            this.lblSearchClass.AutoSize = true;
            this.lblSearchClass.Location = new System.Drawing.Point(0, 3);
            this.lblSearchClass.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblSearchClass.Name = "lblSearchClass";
            this.lblSearchClass.Size = new System.Drawing.Size(45, 15);
            this.lblSearchClass.TabIndex = 0;
            this.lblSearchClass.Text = "Поиск:";
         
            this.classSearchTextBox.Location = new System.Drawing.Point(50, 3);
            this.classSearchTextBox.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.classSearchTextBox.Name = "classSearchTextBox";
            this.classSearchTextBox.Size = new System.Drawing.Size(250, 23);
            this.classSearchTextBox.TabIndex = 1;
           
            this.splitStudentsParents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitStudentsParents.Location = new System.Drawing.Point(5, 5);
            this.splitStudentsParents.Name = "splitStudentsParents";
            this.splitStudentsParents.Orientation = System.Windows.Forms.Orientation.Horizontal;
          
            this.splitStudentsParents.Panel1.Controls.Add(this.studentsPanel);
            this.splitStudentsParents.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
          
            this.splitStudentsParents.Panel2.Controls.Add(this.parentsPanel);
            this.splitStudentsParents.Panel2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.splitStudentsParents.Size = new System.Drawing.Size(1006, 482);
            this.splitStudentsParents.SplitterDistance = 241;
            this.splitStudentsParents.TabIndex = 0;
          
            this.studentsPanel.Controls.Add(this.studentsGrid);
            this.studentsPanel.Controls.Add(this.studentsTopPanel);
            this.studentsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.studentsPanel.Location = new System.Drawing.Point(0, 0);
            this.studentsPanel.Name = "studentsPanel";
            this.studentsPanel.Size = new System.Drawing.Size(1001, 241);
            this.studentsPanel.TabIndex = 0;
          
            this.studentsGrid.AllowUserToAddRows = false;
            this.studentsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.studentsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.studentsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.studentsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.studentsGrid.Location = new System.Drawing.Point(0, 40);
            this.studentsGrid.Name = "studentsGrid";
            this.studentsGrid.ReadOnly = true;
            this.studentsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.studentsGrid.Size = new System.Drawing.Size(1001, 201);
            this.studentsGrid.TabIndex = 1;
        
            this.studentsTopPanel.Controls.Add(this.studentsButtonPanel);
            this.studentsTopPanel.Controls.Add(this.studentsSearchPanel);
            this.studentsTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.studentsTopPanel.Location = new System.Drawing.Point(0, 0);
            this.studentsTopPanel.Name = "studentsTopPanel";
            this.studentsTopPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.studentsTopPanel.Size = new System.Drawing.Size(1001, 40);
            this.studentsTopPanel.TabIndex = 0;
          
            this.studentsButtonPanel.Controls.Add(this.btnTransferStudent);
            this.studentsButtonPanel.Controls.Add(this.btnExpelStudent);
            this.studentsButtonPanel.Controls.Add(this.btnEditStudent);
            this.studentsButtonPanel.Controls.Add(this.btnAddStudent);
            this.studentsButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.studentsButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.studentsButtonPanel.Location = new System.Drawing.Point(451, 5);
            this.studentsButtonPanel.Name = "studentsButtonPanel";
            this.studentsButtonPanel.Size = new System.Drawing.Size(550, 30);
            this.studentsButtonPanel.TabIndex = 1;
           
            this.btnTransferStudent.Location = new System.Drawing.Point(427, 3);
            this.btnTransferStudent.Name = "btnTransferStudent";
            this.btnTransferStudent.Size = new System.Drawing.Size(120, 23);
            this.btnTransferStudent.TabIndex = 0;
            this.btnTransferStudent.Text = "Перевести";
            this.btnTransferStudent.UseVisualStyleBackColor = true;
          
            this.btnExpelStudent.Location = new System.Drawing.Point(301, 3);
            this.btnExpelStudent.Name = "btnExpelStudent";
            this.btnExpelStudent.Size = new System.Drawing.Size(120, 23);
            this.btnExpelStudent.TabIndex = 1;
            this.btnExpelStudent.Text = "Отчислить";
            this.btnExpelStudent.UseVisualStyleBackColor = true;
          
            this.btnEditStudent.Location = new System.Drawing.Point(175, 3);
            this.btnEditStudent.Name = "btnEditStudent";
            this.btnEditStudent.Size = new System.Drawing.Size(120, 23);
            this.btnEditStudent.TabIndex = 2;
            this.btnEditStudent.Text = "Редактировать";
            this.btnEditStudent.UseVisualStyleBackColor = true;
        
            this.btnAddStudent.Location = new System.Drawing.Point(29, 3);
            this.btnAddStudent.Name = "btnAddStudent";
            this.btnAddStudent.Size = new System.Drawing.Size(140, 23);
            this.btnAddStudent.TabIndex = 3;
            this.btnAddStudent.Text = "Добавить ученика";
            this.btnAddStudent.UseVisualStyleBackColor = true;
          
            this.studentsSearchPanel.AutoSize = true;
            this.studentsSearchPanel.Controls.Add(this.lblSearchStudent);
            this.studentsSearchPanel.Controls.Add(this.studentSearchTextBox);
            this.studentsSearchPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.studentsSearchPanel.Location = new System.Drawing.Point(0, 5);
            this.studentsSearchPanel.Name = "studentsSearchPanel";
            this.studentsSearchPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.studentsSearchPanel.Size = new System.Drawing.Size(306, 30);
            this.studentsSearchPanel.TabIndex = 0;
            this.studentsSearchPanel.WrapContents = false;
         
            this.lblSearchStudent.AutoSize = true;
            this.lblSearchStudent.Location = new System.Drawing.Point(0, 3);
            this.lblSearchStudent.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblSearchStudent.Name = "lblSearchStudent";
            this.lblSearchStudent.Size = new System.Drawing.Size(45, 15);
            this.lblSearchStudent.TabIndex = 0;
            this.lblSearchStudent.Text = "Поиск:";
          
            this.studentSearchTextBox.Location = new System.Drawing.Point(50, 3);
            this.studentSearchTextBox.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.studentSearchTextBox.Name = "studentSearchTextBox";
            this.studentSearchTextBox.Size = new System.Drawing.Size(250, 23);
            this.studentSearchTextBox.TabIndex = 1;
         
            this.parentsPanel.Controls.Add(this.parentsGrid);
            this.parentsPanel.Controls.Add(this.lblParentsHeader);
            this.parentsPanel.Controls.Add(this.parentsButtonPanel);
            this.parentsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parentsPanel.Enabled = false;
            this.parentsPanel.Location = new System.Drawing.Point(5, 0);
            this.parentsPanel.Name = "parentsPanel";
            this.parentsPanel.Size = new System.Drawing.Size(1001, 237);
            this.parentsPanel.TabIndex = 0;
          
            this.parentsGrid.AllowUserToAddRows = false;
            this.parentsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.parentsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.parentsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parentsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parentsGrid.Location = new System.Drawing.Point(0, 65);
            this.parentsGrid.Name = "parentsGrid";
            this.parentsGrid.ReadOnly = true;
            this.parentsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.parentsGrid.Size = new System.Drawing.Size(1001, 172);
            this.parentsGrid.TabIndex = 2;
          
            this.lblParentsHeader.AutoSize = true;
            this.lblParentsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblParentsHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblParentsHeader.Location = new System.Drawing.Point(0, 40);
            this.lblParentsHeader.Name = "lblParentsHeader";
            this.lblParentsHeader.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.lblParentsHeader.Size = new System.Drawing.Size(117, 25);
            this.lblParentsHeader.TabIndex = 1;
            this.lblParentsHeader.Text = "Родители ученика";
          
            this.parentsButtonPanel.Controls.Add(this.btnUnlinkParent);
            this.parentsButtonPanel.Controls.Add(this.btnEditParent);
            this.parentsButtonPanel.Controls.Add(this.btnLinkParent);
            this.parentsButtonPanel.Controls.Add(this.btnAddNewParent);
            this.parentsButtonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.parentsButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.parentsButtonPanel.Location = new System.Drawing.Point(0, 0);
            this.parentsButtonPanel.Name = "parentsButtonPanel";
            this.parentsButtonPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.parentsButtonPanel.Size = new System.Drawing.Size(1001, 40);
            this.parentsButtonPanel.TabIndex = 0;
        
            this.btnUnlinkParent.Location = new System.Drawing.Point(908, 8);
            this.btnUnlinkParent.Name = "btnUnlinkParent";
            this.btnUnlinkParent.Size = new System.Drawing.Size(90, 23);
            this.btnUnlinkParent.TabIndex = 0;
            this.btnUnlinkParent.Text = "Отвязать";
            this.btnUnlinkParent.UseVisualStyleBackColor = true;
        
            this.btnEditParent.Location = new System.Drawing.Point(792, 8);
            this.btnEditParent.Name = "btnEditParent";
            this.btnEditParent.Size = new System.Drawing.Size(110, 23);
            this.btnEditParent.TabIndex = 1;
            this.btnEditParent.Text = "Редактировать";
            this.btnEditParent.UseVisualStyleBackColor = true;
        
            this.btnLinkParent.Location = new System.Drawing.Point(696, 8);
            this.btnLinkParent.Name = "btnLinkParent";
            this.btnLinkParent.Size = new System.Drawing.Size(90, 23);
            this.btnLinkParent.TabIndex = 2;
            this.btnLinkParent.Text = "Привязать";
            this.btnLinkParent.UseVisualStyleBackColor = true;
          
            this.btnAddNewParent.Location = new System.Drawing.Point(570, 8);
            this.btnAddNewParent.Name = "btnAddNewParent";
            this.btnAddNewParent.Size = new System.Drawing.Size(120, 23);
            this.btnAddNewParent.TabIndex = 3;
            this.btnAddNewParent.Text = "Добавить нового";
            this.btnAddNewParent.UseVisualStyleBackColor = true;
         
            this.tabPlans.Controls.Add(this.splitPlans);
            this.tabPlans.Location = new System.Drawing.Point(4, 24);
            this.tabPlans.Name = "tabPlans";
            this.tabPlans.Size = new System.Drawing.Size(1016, 740);
            this.tabPlans.TabIndex = 2;
            this.tabPlans.Text = "Учебные планы";
            this.tabPlans.UseVisualStyleBackColor = true;
        
            this.splitPlans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPlans.Location = new System.Drawing.Point(0, 0);
            this.splitPlans.Name = "splitPlans";
            this.splitPlans.Orientation = System.Windows.Forms.Orientation.Horizontal;
          
            this.splitPlans.Panel1.Controls.Add(this.studyPlansGrid);
            this.splitPlans.Panel1.Controls.Add(this.lblPlansHeader);
            this.splitPlans.Panel1.Controls.Add(this.plansTopPanel);
            this.splitPlans.Panel1.Padding = new System.Windows.Forms.Padding(5);
          
            this.splitPlans.Panel2.Controls.Add(this.studyPlanItemsGrid);
            this.splitPlans.Panel2.Controls.Add(this.planItemsTopPanel);
            this.splitPlans.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitPlans.Size = new System.Drawing.Size(1016, 740);
            this.splitPlans.SplitterDistance = 444;
            this.splitPlans.TabIndex = 0;
          
            this.studyPlansGrid.AllowUserToAddRows = false;
            this.studyPlansGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.studyPlansGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.studyPlansGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.studyPlansGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.studyPlansGrid.Location = new System.Drawing.Point(5, 65);
            this.studyPlansGrid.Name = "studyPlansGrid";
            this.studyPlansGrid.ReadOnly = true;
            this.studyPlansGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.studyPlansGrid.Size = new System.Drawing.Size(1006, 374);
            this.studyPlansGrid.TabIndex = 2;
          
            this.lblPlansHeader.AutoSize = true;
            this.lblPlansHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPlansHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPlansHeader.Location = new System.Drawing.Point(5, 45);
            this.lblPlansHeader.Name = "lblPlansHeader";
            this.lblPlansHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblPlansHeader.Size = new System.Drawing.Size(96, 20);
            this.lblPlansHeader.TabIndex = 1;
            this.lblPlansHeader.Text = "Учебные планы";
          
            this.plansTopPanel.Controls.Add(this.plansButtonPanel);
            this.plansTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.plansTopPanel.Location = new System.Drawing.Point(5, 5);
            this.plansTopPanel.Name = "plansTopPanel";
            this.plansTopPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.plansTopPanel.Size = new System.Drawing.Size(1006, 40);
            this.plansTopPanel.TabIndex = 0;
         
            this.plansButtonPanel.AutoSize = true;
            this.plansButtonPanel.Controls.Add(this.btnAddStudyPlan);
            this.plansButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.plansButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.plansButtonPanel.Location = new System.Drawing.Point(860, 5);
            this.plansButtonPanel.Name = "plansButtonPanel";
            this.plansButtonPanel.Size = new System.Drawing.Size(146, 30);
            this.plansButtonPanel.TabIndex = 0;
         
            this.btnAddStudyPlan.Location = new System.Drawing.Point(3, 3);
            this.btnAddStudyPlan.Name = "btnAddStudyPlan";
            this.btnAddStudyPlan.Size = new System.Drawing.Size(140, 23);
            this.btnAddStudyPlan.TabIndex = 0;
            this.btnAddStudyPlan.Text = "Создать план";
            this.btnAddStudyPlan.UseVisualStyleBackColor = true;
          
            this.studyPlanItemsGrid.AllowUserToAddRows = false;
            this.studyPlanItemsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.studyPlanItemsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.studyPlanItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.studyPlanItemsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.studyPlanItemsGrid.Location = new System.Drawing.Point(5, 45);
            this.studyPlanItemsGrid.Name = "studyPlanItemsGrid";
            this.studyPlanItemsGrid.ReadOnly = true;
            this.studyPlanItemsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.studyPlanItemsGrid.Size = new System.Drawing.Size(1006, 242);
            this.studyPlanItemsGrid.TabIndex = 1;
          
            this.planItemsTopPanel.Controls.Add(this.lblPlanItemsHeader);
            this.planItemsTopPanel.Controls.Add(this.planItemsButtonPanel);
            this.planItemsTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.planItemsTopPanel.Location = new System.Drawing.Point(5, 5);
            this.planItemsTopPanel.Name = "planItemsTopPanel";
            this.planItemsTopPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.planItemsTopPanel.Size = new System.Drawing.Size(1006, 40);
            this.planItemsTopPanel.TabIndex = 0;
         
            this.lblPlanItemsHeader.AutoSize = true;
            this.lblPlanItemsHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPlanItemsHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPlanItemsHeader.Location = new System.Drawing.Point(0, 5);
            this.lblPlanItemsHeader.Name = "lblPlanItemsHeader";
            this.lblPlanItemsHeader.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblPlanItemsHeader.Size = new System.Drawing.Size(106, 18);
            this.lblPlanItemsHeader.TabIndex = 1;
            this.lblPlanItemsHeader.Text = "Предметы плана";
         
            this.planItemsButtonPanel.Controls.Add(this.btnDeletePlanItem);
            this.planItemsButtonPanel.Controls.Add(this.btnAddPlanItem);
            this.planItemsButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.planItemsButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.planItemsButtonPanel.Location = new System.Drawing.Point(706, 5);
            this.planItemsButtonPanel.Name = "planItemsButtonPanel";
            this.planItemsButtonPanel.Size = new System.Drawing.Size(300, 30);
            this.planItemsButtonPanel.TabIndex = 0;
         
            this.btnDeletePlanItem.Location = new System.Drawing.Point(157, 3);
            this.btnDeletePlanItem.Name = "btnDeletePlanItem";
            this.btnDeletePlanItem.Size = new System.Drawing.Size(140, 23);
            this.btnDeletePlanItem.TabIndex = 0;
            this.btnDeletePlanItem.Text = "Удалить предмет";
            this.btnDeletePlanItem.UseVisualStyleBackColor = true;
         
            this.btnAddPlanItem.Location = new System.Drawing.Point(11, 3);
            this.btnAddPlanItem.Name = "btnAddPlanItem";
            this.btnAddPlanItem.Size = new System.Drawing.Size(140, 23);
            this.btnAddPlanItem.TabIndex = 1;
            this.btnAddPlanItem.Text = "Добавить предмет";
            this.btnAddPlanItem.UseVisualStyleBackColor = true;
         
            this.tabWorkload.Controls.Add(this.workloadGrid);
            this.tabWorkload.Controls.Add(this.workloadTopPanel);
            this.tabWorkload.Location = new System.Drawing.Point(4, 24);
            this.tabWorkload.Name = "tabWorkload";
            this.tabWorkload.Padding = new System.Windows.Forms.Padding(5);
            this.tabWorkload.Size = new System.Drawing.Size(1016, 740);
            this.tabWorkload.TabIndex = 3;
            this.tabWorkload.Text = "Нагрузка";
            this.tabWorkload.UseVisualStyleBackColor = true;
          
            this.workloadGrid.AllowUserToAddRows = false;
            this.workloadGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.workloadGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.workloadGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.workloadGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workloadGrid.Location = new System.Drawing.Point(5, 45);
            this.workloadGrid.Name = "workloadGrid";
            this.workloadGrid.ReadOnly = true;
            this.workloadGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.workloadGrid.Size = new System.Drawing.Size(1006, 690);
            this.workloadGrid.TabIndex = 1;
          
            this.workloadTopPanel.Controls.Add(this.workloadButtonPanel);
            this.workloadTopPanel.Controls.Add(this.workloadFilterPanel);
            this.workloadTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.workloadTopPanel.Location = new System.Drawing.Point(5, 5);
            this.workloadTopPanel.Name = "workloadTopPanel";
            this.workloadTopPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.workloadTopPanel.Size = new System.Drawing.Size(1006, 40);
            this.workloadTopPanel.TabIndex = 0;
         
            this.workloadButtonPanel.Controls.Add(this.btnRemoveTeacherFromWorkload);
            this.workloadButtonPanel.Controls.Add(this.btnAssignTeacherToWorkload);
            this.workloadButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.workloadButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.workloadButtonPanel.Location = new System.Drawing.Point(686, 5);
            this.workloadButtonPanel.Name = "workloadButtonPanel";
            this.workloadButtonPanel.Size = new System.Drawing.Size(320, 30);
            this.workloadButtonPanel.TabIndex = 1;
          
            this.btnRemoveTeacherFromWorkload.Location = new System.Drawing.Point(167, 3);
            this.btnRemoveTeacherFromWorkload.Name = "btnRemoveTeacherFromWorkload";
            this.btnRemoveTeacherFromWorkload.Size = new System.Drawing.Size(150, 23);
            this.btnRemoveTeacherFromWorkload.TabIndex = 0;
            this.btnRemoveTeacherFromWorkload.Text = "Снять учителя";
            this.btnRemoveTeacherFromWorkload.UseVisualStyleBackColor = true;
          
            this.btnAssignTeacherToWorkload.Location = new System.Drawing.Point(11, 3);
            this.btnAssignTeacherToWorkload.Name = "btnAssignTeacherToWorkload";
            this.btnAssignTeacherToWorkload.Size = new System.Drawing.Size(150, 23);
            this.btnAssignTeacherToWorkload.TabIndex = 1;
            this.btnAssignTeacherToWorkload.Text = "Назначить/изменить";
            this.btnAssignTeacherToWorkload.UseVisualStyleBackColor = true;
          
            this.workloadFilterPanel.AutoSize = true;
            this.workloadFilterPanel.Controls.Add(this.lblWorkloadClass);
            this.workloadFilterPanel.Controls.Add(this.workloadClassComboBox);
            this.workloadFilterPanel.Controls.Add(this.lblWorkloadYear);
            this.workloadFilterPanel.Controls.Add(this.yearComboBox);
            this.workloadFilterPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.workloadFilterPanel.Location = new System.Drawing.Point(0, 5);
            this.workloadFilterPanel.Name = "workloadFilterPanel";
            this.workloadFilterPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.workloadFilterPanel.Size = new System.Drawing.Size(433, 30);
            this.workloadFilterPanel.TabIndex = 0;
            this.workloadFilterPanel.WrapContents = false;
         
            this.lblWorkloadClass.AutoSize = true;
            this.lblWorkloadClass.Location = new System.Drawing.Point(0, 3);
            this.lblWorkloadClass.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblWorkloadClass.Name = "lblWorkloadClass";
            this.lblWorkloadClass.Size = new System.Drawing.Size(95, 15);
            this.lblWorkloadClass.TabIndex = 0;
            this.lblWorkloadClass.Text = "Выберите класс:";
          
            this.workloadClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.workloadClassComboBox.FormattingEnabled = true;
            this.workloadClassComboBox.Location = new System.Drawing.Point(100, 3);
            this.workloadClassComboBox.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.workloadClassComboBox.Name = "workloadClassComboBox";
            this.workloadClassComboBox.Size = new System.Drawing.Size(150, 23);
            this.workloadClassComboBox.TabIndex = 1;
         
            this.lblWorkloadYear.AutoSize = true;
            this.lblWorkloadYear.Location = new System.Drawing.Point(260, 3);
            this.lblWorkloadYear.Margin = new System.Windows.Forms.Padding(10, 3, 0, 0);
            this.lblWorkloadYear.Name = "lblWorkloadYear";
            this.lblWorkloadYear.Size = new System.Drawing.Size(81, 15);
            this.lblWorkloadYear.TabIndex = 2;
            this.lblWorkloadYear.Text = "Учебный год:";
          
            this.yearComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.yearComboBox.FormattingEnabled = true;
            this.yearComboBox.Location = new System.Drawing.Point(346, 3);
            this.yearComboBox.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.yearComboBox.Name = "yearComboBox";
            this.yearComboBox.Size = new System.Drawing.Size(120, 23);
            this.yearComboBox.TabIndex = 3;
          
            this.tabAcademicYear.Controls.Add(this.academicYearsGrid);
            this.tabAcademicYear.Controls.Add(this.academicYearsTopPanel);
            this.tabAcademicYear.Location = new System.Drawing.Point(4, 24);
            this.tabAcademicYear.Name = "tabAcademicYear";
            this.tabAcademicYear.Padding = new System.Windows.Forms.Padding(5);
            this.tabAcademicYear.Size = new System.Drawing.Size(1016, 740);
            this.tabAcademicYear.TabIndex = 4;
            this.tabAcademicYear.Text = "Учебный год";
            this.tabAcademicYear.UseVisualStyleBackColor = true;
         
            this.academicYearsGrid.AllowUserToAddRows = false;
            this.academicYearsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.academicYearsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.academicYearsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.academicYearsGrid.ContextMenuStrip = this.academicYearsContextMenu;
            this.academicYearsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.academicYearsGrid.Location = new System.Drawing.Point(5, 45);
            this.academicYearsGrid.Name = "academicYearsGrid";
            this.academicYearsGrid.ReadOnly = true;
            this.academicYearsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.academicYearsGrid.Size = new System.Drawing.Size(1006, 690);
            this.academicYearsGrid.TabIndex = 1;
          
            this.academicYearsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSetCurrentYear,
            this.menuArchiveYear});
            this.academicYearsContextMenu.Name = "academicYearsContextMenu";
            this.academicYearsContextMenu.Size = new System.Drawing.Size(175, 48);
         
            this.menuSetCurrentYear.Name = "menuSetCurrentYear";
            this.menuSetCurrentYear.Size = new System.Drawing.Size(174, 22);
            this.menuSetCurrentYear.Text = "Сделать текущим";
          
            this.menuArchiveYear.Name = "menuArchiveYear";
            this.menuArchiveYear.Size = new System.Drawing.Size(174, 22);
            this.menuArchiveYear.Text = "Архивировать";
           
            this.academicYearsTopPanel.Controls.Add(this.academicYearsButtonPanel);
            this.academicYearsTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.academicYearsTopPanel.Location = new System.Drawing.Point(5, 5);
            this.academicYearsTopPanel.Name = "academicYearsTopPanel";
            this.academicYearsTopPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.academicYearsTopPanel.Size = new System.Drawing.Size(1006, 40);
            this.academicYearsTopPanel.TabIndex = 0;
          
            this.academicYearsButtonPanel.Controls.Add(this.btnPromoteStudents);
            this.academicYearsButtonPanel.Controls.Add(this.btnCreateYear);
            this.academicYearsButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.academicYearsButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.academicYearsButtonPanel.Location = new System.Drawing.Point(556, 5);
            this.academicYearsButtonPanel.Name = "academicYearsButtonPanel";
            this.academicYearsButtonPanel.Size = new System.Drawing.Size(450, 30);
            this.academicYearsButtonPanel.TabIndex = 0;
           
            this.btnPromoteStudents.Location = new System.Drawing.Point(197, 3);
            this.btnPromoteStudents.Name = "btnPromoteStudents";
            this.btnPromoteStudents.Size = new System.Drawing.Size(250, 23);
            this.btnPromoteStudents.TabIndex = 0;
            this.btnPromoteStudents.Text = "Завершить год и перевести учащихся";
            this.btnPromoteStudents.UseVisualStyleBackColor = true;
          
            this.btnCreateYear.Location = new System.Drawing.Point(41, 3);
            this.btnCreateYear.Name = "btnCreateYear";
            this.btnCreateYear.Size = new System.Drawing.Size(150, 23);
            this.btnCreateYear.TabIndex = 1;
            this.btnCreateYear.Text = "Создать новый год";
            this.btnCreateYear.UseVisualStyleBackColor = true;
           
            this.tabReports.Controls.Add(this.splitReports);
            this.tabReports.Location = new System.Drawing.Point(4, 24);
            this.tabReports.Name = "tabReports";
            this.tabReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabReports.Size = new System.Drawing.Size(1016, 740);
            this.tabReports.TabIndex = 5;
            this.tabReports.Text = "Отчеты";
            this.tabReports.UseVisualStyleBackColor = true;
          
            this.splitReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitReports.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitReports.Location = new System.Drawing.Point(3, 3);
            this.splitReports.Name = "splitReports";
          
            this.splitReports.Panel1.Controls.Add(this.reportsListBox);
            this.splitReports.Panel1.Controls.Add(this.lblReportsHeader);
            this.splitReports.Panel1.Padding = new System.Windows.Forms.Padding(5);
          
            this.splitReports.Panel2.Controls.Add(this.reportsResultGrid);
            this.splitReports.Panel2.Controls.Add(this.reportsFilterPanel);
            this.splitReports.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitReports.Size = new System.Drawing.Size(1010, 734);
            this.splitReports.SplitterDistance = 220;
            this.splitReports.TabIndex = 0;
          
            this.reportsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.reportsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportsListBox.FormattingEnabled = true;
            this.reportsListBox.ItemHeight = 15;
            this.reportsListBox.Items.AddRange(new object[] {
            "Сводный отчет по успеваемости",
            "Нагрузка преподавателей",
            "Движение контингента"});
            this.reportsListBox.Location = new System.Drawing.Point(5, 20);
            this.reportsListBox.Name = "reportsListBox";
            this.reportsListBox.Size = new System.Drawing.Size(210, 709);
            this.reportsListBox.TabIndex = 1;
          
            this.lblReportsHeader.AutoSize = true;
            this.lblReportsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblReportsHeader.Location = new System.Drawing.Point(5, 5);
            this.lblReportsHeader.Name = "lblReportsHeader";
            this.lblReportsHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblReportsHeader.Size = new System.Drawing.Size(102, 20);
            this.lblReportsHeader.TabIndex = 0;
            this.lblReportsHeader.Text = "Выберите отчет:";
          
            this.reportsResultGrid.AllowUserToAddRows = false;
            this.reportsResultGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.reportsResultGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reportsResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.reportsResultGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportsResultGrid.Location = new System.Drawing.Point(5, 115);
            this.reportsResultGrid.Name = "reportsResultGrid";
            this.reportsResultGrid.ReadOnly = true;
            this.reportsResultGrid.Size = new System.Drawing.Size(776, 614);
            this.reportsResultGrid.TabIndex = 1;
           
            this.reportsFilterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.reportsFilterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.reportsFilterPanel.Location = new System.Drawing.Point(5, 5);
            this.reportsFilterPanel.Name = "reportsFilterPanel";
            this.reportsFilterPanel.Padding = new System.Windows.Forms.Padding(5);
            this.reportsFilterPanel.Size = new System.Drawing.Size(776, 110);
            this.reportsFilterPanel.TabIndex = 0;
          
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "AdminDashboardControl";
            this.Size = new System.Drawing.Size(1024, 768);
            this.tabControl.ResumeLayout(false);
            this.tabTeachers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teachersGrid)).EndInit();
            this.teachersTopPanel.ResumeLayout(false);
            this.teachersTopPanel.PerformLayout();
            this.teachersButtonPanel.ResumeLayout(false);
            this.teachersSearchPanel.ResumeLayout(false);
            this.teachersSearchPanel.PerformLayout();
            this.tabClasses.ResumeLayout(false);
            this.splitClassesMain.Panel1.ResumeLayout(false);
            this.splitClassesMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitClassesMain)).EndInit();
            this.splitClassesMain.ResumeLayout(false);
            this.classesPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.classesGrid)).EndInit();
            this.classesTopPanel.ResumeLayout(false);
            this.classesTopPanel.PerformLayout();
            this.classesButtonPanel.ResumeLayout(false);
            this.classesSearchPanel.ResumeLayout(false);
            this.classesSearchPanel.PerformLayout();
            this.splitStudentsParents.Panel1.ResumeLayout(false);
            this.splitStudentsParents.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitStudentsParents)).EndInit();
            this.splitStudentsParents.ResumeLayout(false);
            this.studentsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.studentsGrid)).EndInit();
            this.studentsTopPanel.ResumeLayout(false);
            this.studentsTopPanel.PerformLayout();
            this.studentsButtonPanel.ResumeLayout(false);
            this.studentsSearchPanel.ResumeLayout(false);
            this.studentsSearchPanel.PerformLayout();
            this.parentsPanel.ResumeLayout(false);
            this.parentsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parentsGrid)).EndInit();
            this.parentsButtonPanel.ResumeLayout(false);
            this.tabPlans.ResumeLayout(false);
            this.splitPlans.Panel1.ResumeLayout(false);
            this.splitPlans.Panel1.PerformLayout();
            this.splitPlans.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPlans)).EndInit();
            this.splitPlans.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.studyPlansGrid)).EndInit();
            this.plansTopPanel.ResumeLayout(false);
            this.plansTopPanel.PerformLayout();
            this.plansButtonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.studyPlanItemsGrid)).EndInit();
            this.planItemsTopPanel.ResumeLayout(false);
            this.planItemsTopPanel.PerformLayout();
            this.planItemsButtonPanel.ResumeLayout(false);
            this.tabWorkload.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.workloadGrid)).EndInit();
            this.workloadTopPanel.ResumeLayout(false);
            this.workloadTopPanel.PerformLayout();
            this.workloadButtonPanel.ResumeLayout(false);
            this.workloadFilterPanel.ResumeLayout(false);
            this.workloadFilterPanel.PerformLayout();
            this.tabAcademicYear.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.academicYearsGrid)).EndInit();
            this.academicYearsContextMenu.ResumeLayout(false);
            this.academicYearsTopPanel.ResumeLayout(false);
            this.academicYearsButtonPanel.ResumeLayout(false);
            this.tabReports.ResumeLayout(false);
            this.splitReports.Panel1.ResumeLayout(false);
            this.splitReports.Panel1.PerformLayout();
            this.splitReports.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitReports)).EndInit();
            this.splitReports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.reportsResultGrid)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabTeachers;
        private System.Windows.Forms.TabPage tabClasses;
        private System.Windows.Forms.TabPage tabPlans;
        private System.Windows.Forms.TabPage tabWorkload;
        private System.Windows.Forms.TabPage tabAcademicYear;
        private System.Windows.Forms.TabPage tabReports;

        private System.Windows.Forms.Panel teachersTopPanel;
        private System.Windows.Forms.DataGridView teachersGrid;
        private System.Windows.Forms.FlowLayoutPanel teachersButtonPanel;
        private System.Windows.Forms.Button btnDeleteTeacher;
        private System.Windows.Forms.Button btnEditTeacher;
        private System.Windows.Forms.Button btnAddTeacher;
        private System.Windows.Forms.FlowLayoutPanel teachersSearchPanel;
        private System.Windows.Forms.Label lblSearchTeacher;
        private System.Windows.Forms.TextBox teacherSearchTextBox;

        private System.Windows.Forms.SplitContainer splitClassesMain;
        private System.Windows.Forms.Panel classesPanel;
        private System.Windows.Forms.DataGridView classesGrid;
        private System.Windows.Forms.Panel classesTopPanel;
        private System.Windows.Forms.FlowLayoutPanel classesButtonPanel;
        private System.Windows.Forms.Button btnEditClass;
        private System.Windows.Forms.Button btnAddClass;
        private System.Windows.Forms.FlowLayoutPanel classesSearchPanel;
        private System.Windows.Forms.Label lblSearchClass;
        private System.Windows.Forms.TextBox classSearchTextBox;
        private System.Windows.Forms.SplitContainer splitStudentsParents;
        private System.Windows.Forms.Panel studentsPanel;
        private System.Windows.Forms.DataGridView studentsGrid;
        private System.Windows.Forms.Panel studentsTopPanel;
        private System.Windows.Forms.FlowLayoutPanel studentsButtonPanel;
        private System.Windows.Forms.Button btnTransferStudent;
        private System.Windows.Forms.Button btnExpelStudent;
        private System.Windows.Forms.Button btnEditStudent;
        private System.Windows.Forms.Button btnAddStudent;
        private System.Windows.Forms.FlowLayoutPanel studentsSearchPanel;
        private System.Windows.Forms.Label lblSearchStudent;
        private System.Windows.Forms.TextBox studentSearchTextBox;
        private System.Windows.Forms.Panel parentsPanel;
        private System.Windows.Forms.DataGridView parentsGrid;
        private System.Windows.Forms.Label lblParentsHeader;
        private System.Windows.Forms.FlowLayoutPanel parentsButtonPanel;
        private System.Windows.Forms.Button btnUnlinkParent;
        private System.Windows.Forms.Button btnEditParent;
        private System.Windows.Forms.Button btnLinkParent;
        private System.Windows.Forms.Button btnAddNewParent;

        private System.Windows.Forms.SplitContainer splitPlans;
        private System.Windows.Forms.DataGridView studyPlansGrid;
        private System.Windows.Forms.Panel plansTopPanel;
        private System.Windows.Forms.Label lblPlansHeader;
        private System.Windows.Forms.FlowLayoutPanel plansButtonPanel;
        private System.Windows.Forms.Button btnAddStudyPlan;
        private System.Windows.Forms.DataGridView studyPlanItemsGrid;
        private System.Windows.Forms.Panel planItemsTopPanel;
        private System.Windows.Forms.Label lblPlanItemsHeader;
        private System.Windows.Forms.FlowLayoutPanel planItemsButtonPanel;
        private System.Windows.Forms.Button btnDeletePlanItem;
        private System.Windows.Forms.Button btnAddPlanItem;

        private System.Windows.Forms.DataGridView workloadGrid;
        private System.Windows.Forms.Panel workloadTopPanel;
        private System.Windows.Forms.FlowLayoutPanel workloadButtonPanel;
        private System.Windows.Forms.Button btnRemoveTeacherFromWorkload;
        private System.Windows.Forms.Button btnAssignTeacherToWorkload;
        private System.Windows.Forms.FlowLayoutPanel workloadFilterPanel;
        private System.Windows.Forms.Label lblWorkloadClass;
        private System.Windows.Forms.ComboBox workloadClassComboBox;
        private System.Windows.Forms.Label lblWorkloadYear;
        private System.Windows.Forms.ComboBox yearComboBox;

        private System.Windows.Forms.DataGridView academicYearsGrid;
        private System.Windows.Forms.Panel academicYearsTopPanel;
        private System.Windows.Forms.FlowLayoutPanel academicYearsButtonPanel;
        private System.Windows.Forms.Button btnPromoteStudents;
        private System.Windows.Forms.Button btnCreateYear;
        private System.Windows.Forms.ContextMenuStrip academicYearsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuSetCurrentYear;
        private System.Windows.Forms.ToolStripMenuItem menuArchiveYear;

        private System.Windows.Forms.SplitContainer splitReports;
        private System.Windows.Forms.ListBox reportsListBox;
        private System.Windows.Forms.Label lblReportsHeader;
        private System.Windows.Forms.DataGridView reportsResultGrid;
        private System.Windows.Forms.Panel reportsFilterPanel;
    }
}