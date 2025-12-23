using Npgsql;
using AISchool.Data;
using AISchool.Models;
using AISchool.Presenters;
using AISchool.Utils;
using static AISchool.Data.DataAccess;

namespace AISchool.Views
{
    public partial class AdminDashboardControl : UserControl, IAdminDashboardView
    {
        private readonly AppUser _admin;
        private readonly IDataAccess _dataAccess;

        private SortableBindingList<TeacherDetails> _teachersBindingList = null!;
        private SortableBindingList<ClassDetails> _classesBindingList = null!;
        private SortableBindingList<StudentInfo> _studentsBindingList = null!;
        private SortableBindingList<AcademicPerformanceReportItem> _performanceReportBindingList = null!;
        private SortableBindingList<TeacherWorkloadReportItem> _workloadReportBindingList = null!;
        private SortableBindingList<StudyPlanView> _studyPlansBindingList = null!;
        private SortableBindingList<StudyPlanItem> _studyPlanItemsBindingList = null!;
        private SortableBindingList<WorkloadView> _workloadBindingList = null!;
        private SortableBindingList<AcademicYear> _academicYearsBindingList = null!;
        private SortableBindingList<ParentInfo> _parentsBindingList = null!;

        private List<TeacherDetails> _allTeachersMaster = new List<TeacherDetails>();
        private List<ClassDetails> _allClassesMaster = new List<ClassDetails>();
        private List<StudentInfo> _currentClassStudentsMaster = new List<StudentInfo>();
        private List<StudyPlanView> _allStudyPlansMaster = new List<StudyPlanView>();
        private List<StudyPlanItem> _currentStudyPlanItemsMaster = new List<StudyPlanItem>();
        private List<AcademicYear> _academicYearsMaster = new List<AcademicYear>();
        private List<ParentInfo> _currentStudentParentsMaster = new List<ParentInfo>();

        private Button _generateReportButton = null!;
        private ComboBox _reportYearComboBox = null!;
        private DateTimePicker _reportStartDatePicker = null!;
        private DateTimePicker _reportEndDatePicker = null!;

        public AdminDashboardControl(AppUser admin)
        {
            InitializeComponent();
            _admin = admin;
            _dataAccess = new DataAccess();
            new AdminDashboardPresenter(this, _dataAccess);

            this.Dock = DockStyle.Fill;

            SubscribeEvents();

            this.Load += (s, e) => {
                LoadTeachers?.Invoke();
                LoadClasses?.Invoke();
                LoadStudyPlans?.Invoke();
                LoadAcademicYears?.Invoke();

                if (reportsListBox.Items.Count > 0) reportsListBox.SelectedIndex = 0;
            };

            tabReports.Enter += (s, e) =>
            {
                if (_academicYearsMaster == null || !_academicYearsMaster.Any())
                {
                    LoadAcademicYears?.Invoke();
                }
                if (reportsListBox.SelectedIndex >= 0)
                {
                    var current = reportsListBox.SelectedIndex;
                    reportsListBox.SelectedIndex = -1;
                    reportsListBox.SelectedIndex = current;
                }
            };
        }

        public event Action<int>? OpenStudentProfileRequested;

        private void SubscribeEvents()
        {
            teacherSearchTextBox.TextChanged += (s, e) => SearchTeacher?.Invoke(teacherSearchTextBox.Text);
            btnAddTeacher.Click += (s, e) => AddTeacher?.Invoke();
            btnEditTeacher.Click += (s, e) => EditTeacher?.Invoke();
            btnDeleteTeacher.Click += (s, e) => DeleteTeacher?.Invoke();

            classesGrid.SelectionChanged += (s, e) => ClassSelected?.Invoke(SelectedClass);
            btnAddClass.Click += (s, e) => AddClass?.Invoke();
            btnEditClass.Click += (s, e) => EditClass?.Invoke();
            classSearchTextBox.TextChanged += (s, e) =>
            {
                string searchText = classSearchTextBox.Text.ToLower().Trim();
                var filtered = string.IsNullOrEmpty(searchText)
                    ? _allClassesMaster
                    : _allClassesMaster.Where(c => (c.ClassName?.ToLower() ?? "").Contains(searchText) || (c.HeadTeacherFullName?.ToLower() ?? "").Contains(searchText)).ToList();
                classesGrid.DataSource = new SortableBindingList<ClassDetails>(filtered);
            };

            btnAddStudent.Click += (s, e) => AddStudent?.Invoke();
            btnEditStudent.Click += (s, e) => EditStudent?.Invoke();
            btnExpelStudent.Click += (s, e) => ExpelStudent?.Invoke();
            btnTransferStudent.Click += (s, e) => TransferStudent?.Invoke();
            studentSearchTextBox.TextChanged += (s, e) =>
            {
                string searchText = studentSearchTextBox.Text.ToLower().Trim();
                var filtered = string.IsNullOrEmpty(searchText)
                    ? _currentClassStudentsMaster
                    : _currentClassStudentsMaster.Where(st => (st.FullName?.ToLower() ?? "").Contains(searchText)).ToList();
                studentsGrid.DataSource = new SortableBindingList<StudentInfo>(filtered);
            };
            studentsGrid.SelectionChanged += (s, e) =>
            {
                var selectedStudent = SelectedStudent;
                parentsPanel.Enabled = selectedStudent != null;
                if (selectedStudent != null)
                {
                    lblParentsHeader.Text = $"Родители: {selectedStudent.FullName}";
                }
                else
                {
                    lblParentsHeader.Text = "Родители ученика";
                    LinkedParentsList = new List<ParentInfo>();
                }
                StudentSelected?.Invoke(selectedStudent);
            };

            studentsGrid.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && SelectedStudent != null)
                {
                    OpenStudentProfileRequested?.Invoke(SelectedStudent.Id);
                }
            };

            btnAddNewParent.Click += (s, e) => AddNewParent?.Invoke();
            btnLinkParent.Click += (s, e) => LinkExistingParent?.Invoke();
            btnEditParent.Click += (s, e) => EditParent?.Invoke();
            btnUnlinkParent.Click += (s, e) => UnlinkParent?.Invoke();

            btnAddStudyPlan.Click += (s, e) => AddStudyPlan?.Invoke();
            studyPlansGrid.SelectionChanged += (s, e) => StudyPlanSelected?.Invoke(SelectedStudyPlan);
            btnAddPlanItem.Click += (s, e) => AddStudyPlanItem?.Invoke();
            btnDeletePlanItem.Click += (s, e) => DeleteStudyPlanItem?.Invoke();

            tabWorkload.Enter += (s, e) => { if (yearComboBox.DataSource == null) LoadWorkloadData?.Invoke(); };
            btnAssignTeacherToWorkload.Click += (s, e) => AssignTeacherToWorkload?.Invoke();
            btnRemoveTeacherFromWorkload.Click += (s, e) => RemoveTeacherFromWorkload?.Invoke();
            workloadClassComboBox.SelectedIndexChanged += (s, e) => WorkloadFilterChanged?.Invoke(SelectedWorkloadClass?.ClassId, SelectedWorkloadAcademicYear?.Id);
            yearComboBox.SelectedIndexChanged += (s, e) => WorkloadFilterChanged?.Invoke(SelectedWorkloadClass?.ClassId, SelectedWorkloadAcademicYear?.Id);

            tabAcademicYear.Enter += (s, e) => LoadAcademicYears?.Invoke();
            btnPromoteStudents.Click += (s, e) => PromoteStudents?.Invoke();
            btnCreateYear.Click += (s, e) => ShowCreateYearDialog();

            menuSetCurrentYear.Click += (s, e) => {
                if (SelectedAcademicYear != null && SelectedAcademicYear.Status != "Current") UpdateYearStatus?.Invoke(SelectedAcademicYear.Id, "Current");
            };
            menuArchiveYear.Click += (s, e) => {
                if (SelectedAcademicYear != null && SelectedAcademicYear.Status != "Archived") UpdateYearStatus?.Invoke(SelectedAcademicYear.Id, "Archived");
            };
            academicYearsGrid.MouseClick += (s, e) => {
                if (e.Button == MouseButtons.Right)
                {
                    int rowIndex = academicYearsGrid.HitTest(e.X, e.Y).RowIndex;
                    if (rowIndex >= 0)
                    {
                        academicYearsGrid.ClearSelection();
                        academicYearsGrid.Rows[rowIndex].Selected = true;
                    }
                }
            };

            reportsListBox.SelectedIndexChanged += ReportsListBox_SelectedIndexChanged;
        }


        public event Action<StudentInfo?>? StudentSelected;

        public IList<ParentInfo> LinkedParentsList
        {
            set
            {
                _parentsBindingList = new SortableBindingList<ParentInfo>(value.ToList());
                parentsGrid.DataSource = _parentsBindingList;
                if (parentsGrid.Columns.Count > 0)
                {
                    parentsGrid.Columns["Id"].Visible = false;
                    parentsGrid.Columns["PasswordHash"].Visible = false;
                    parentsGrid.Columns["PasswordSalt"].Visible = false;
                    parentsGrid.Columns["Email"].Visible = false;
                    parentsGrid.Columns["AvatarImage"].Visible = false;
                    parentsGrid.Columns["LastName"].Visible = false;
                    parentsGrid.Columns["FirstName"].Visible = false;
                    parentsGrid.Columns["Patronymic"].Visible = false;
                    parentsGrid.Columns["FullName"].HeaderText = "ФИО Родителя";
                    parentsGrid.Columns["Phone"].HeaderText = "Телефон";
                    parentsGrid.Columns["Login"].HeaderText = "Логин";
                }
            }
        }
        public ParentInfo? SelectedParent => parentsGrid.CurrentRow?.DataBoundItem as ParentInfo;

        public event Action? AddNewParent;
        public event Action? LinkExistingParent;
        public event Action? EditParent;
        public event Action? UnlinkParent;

        public void ClearStudentSearch() => studentSearchTextBox.Clear();

        public IList<TeacherDetails> TeachersList
        {
            set
            {
                try
                {
                    teachersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                    _teachersBindingList = new SortableBindingList<TeacherDetails>(value.ToList());
                    teachersGrid.DataSource = _teachersBindingList;

                    if (teachersGrid.Columns.Count > 0)
                    {
                        teachersGrid.Columns["Id"].Visible = false;
                        teachersGrid.Columns["PasswordHash"].Visible = false;
                        teachersGrid.Columns["PasswordSalt"].Visible = false;
                        teachersGrid.Columns["AvatarImage"].Visible = false;
                        teachersGrid.Columns["FullName"].Visible = false;

                        teachersGrid.Columns["Role"].Visible = false;

                        teachersGrid.Columns["RoleDisplay"].HeaderText = "Роль";
                        teachersGrid.Columns["RoleDisplay"].DisplayIndex = 4;

                        teachersGrid.Columns["LastName"].HeaderText = "Фамилия";
                        teachersGrid.Columns["FirstName"].HeaderText = "Имя";
                        teachersGrid.Columns["Patronymic"].HeaderText = "Отчество";
                        teachersGrid.Columns["Login"].HeaderText = "Логин";
                        teachersGrid.Columns["Phone"].HeaderText = "Телефон";
                        teachersGrid.Columns["Email"].HeaderText = "Email";
                        teachersGrid.Columns["Notes"].HeaderText = "Заметки";
                    }
                }
                finally
                {
                    teachersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
        }

        public TeacherDetails? SelectedTeacher => teachersGrid.CurrentRow?.DataBoundItem as TeacherDetails;
        public AppUser CurrentUser => _admin;

        public IList<ClassDetails> ClassesList
        {
            set
            {
                _allClassesMaster = value.ToList();
                _classesBindingList = new SortableBindingList<ClassDetails>(_allClassesMaster);
                classesGrid.DataSource = _classesBindingList;
                if (classesGrid.Columns.Count > 0)
                {
                    classesGrid.Columns["ClassId"].Visible = false;
                    classesGrid.Columns["ParallelNumber"].Visible = false;
                    classesGrid.Columns["HeadTeacherId"].Visible = false;
                    classesGrid.Columns["ClassName"].HeaderText = "Класс";
                    classesGrid.Columns["HeadTeacherFullName"].HeaderText = "Классный руководитель";
                }
            }
        }

        public IList<StudentInfo> StudentsList
        {
            set
            {
                _currentClassStudentsMaster = value.ToList();
                _studentsBindingList = new SortableBindingList<StudentInfo>(_currentClassStudentsMaster);
                studentsGrid.DataSource = _studentsBindingList;
                studentSearchTextBox.Clear();
                if (studentsGrid.Columns.Count > 0)
                {
                    studentsGrid.Columns["Id"].Visible = false;
                    studentsGrid.Columns["FullName"].Visible = false;
                    studentsGrid.Columns["LastName"].HeaderText = "Фамилия";
                    studentsGrid.Columns["FirstName"].HeaderText = "Имя";
                    studentsGrid.Columns["Patronymic"].HeaderText = "Отчество";
                }
            }
        }
        public ClassDetails? SelectedClass => classesGrid.CurrentRow?.DataBoundItem as ClassDetails;
        public StudentInfo? SelectedStudent => studentsGrid.CurrentRow?.DataBoundItem as StudentInfo;

        public IList<StudyPlanView> StudyPlansList
        {
            set
            {
                _studyPlansBindingList = new SortableBindingList<StudyPlanView>(value.ToList());
                studyPlansGrid.DataSource = _studyPlansBindingList;
                if (studyPlansGrid.Columns.Count > 0)
                {
                    studyPlansGrid.Columns["Id"].Visible = false;
                    studyPlansGrid.Columns["ParallelId"].Visible = false;
                    studyPlansGrid.Columns["AcademicYearId"].Visible = false;
                    studyPlansGrid.Columns["Name"].HeaderText = "Название плана";
                    studyPlansGrid.Columns["AcademicYearName"].HeaderText = "Учебный год";
                    studyPlansGrid.Columns["ParallelNumber"].HeaderText = "Параллель";
                }
            }
        }

        public IList<StudyPlanItem> StudyPlanItemsList
        {
            set
            {
                _studyPlanItemsBindingList = new SortableBindingList<StudyPlanItem>(value.ToList());
                studyPlanItemsGrid.DataSource = _studyPlanItemsBindingList;
                if (studyPlanItemsGrid.Columns.Count > 0)
                {
                    studyPlanItemsGrid.Columns["Id"].Visible = false;
                    studyPlanItemsGrid.Columns["DisciplineId"].Visible = false;
                    studyPlanItemsGrid.Columns["DisciplineName"].HeaderText = "Дисциплина";
                    studyPlanItemsGrid.Columns["LessonsCount"].HeaderText = "Кол-во занятий";
                    studyPlanItemsGrid.Columns["AcademicHours"].HeaderText = "Академ. часы";
                }
            }
        }
        public StudyPlanView? SelectedStudyPlan => studyPlansGrid.CurrentRow?.DataBoundItem as StudyPlanView;
        public StudyPlanItem? SelectedStudyPlanItem => studyPlanItemsGrid.CurrentRow?.DataBoundItem as StudyPlanItem;

        public IList<AcademicYear> WorkloadAcademicYearsList
        {
            set
            {
                yearComboBox.DataSource = value;
                yearComboBox.DisplayMember = "Name";
                yearComboBox.ValueMember = "Id";

                var currentYear = value.FirstOrDefault(y => y.Status == "Current");
                if (currentYear != null)
                {
                    yearComboBox.SelectedValue = currentYear.Id;
                }
                else if (value.Any())
                {
                    yearComboBox.SelectedIndex = 0;
                }
            }
        }

        public IList<ClassDetails> WorkloadClassesList
        {
            set
            {
                workloadClassComboBox.DataSource = value;
                workloadClassComboBox.DisplayMember = "ClassName";
                workloadClassComboBox.ValueMember = "ClassId";
                workloadClassComboBox.SelectedIndex = -1;
            }
        }

        public IList<WorkloadView> WorkloadList
        {
            set
            {
                _workloadBindingList = new SortableBindingList<WorkloadView>(value.ToList());
                workloadGrid.DataSource = _workloadBindingList;

                bool isPrimary = SelectedWorkloadClass?.ParallelNumber <= 4;
                btnAssignTeacherToWorkload.Enabled = !isPrimary;
                btnRemoveTeacherFromWorkload.Enabled = !isPrimary;

                if (workloadGrid.Columns.Count > 0)
                {
                    workloadGrid.Columns["DisciplineId"].Visible = false;
                    workloadGrid.Columns["TeacherId"].Visible = false;
                    workloadGrid.Columns["DisciplineName"].HeaderText = "Дисциплина";
                    workloadGrid.Columns["LessonsCount"].HeaderText = "Кол-во занятий";
                    workloadGrid.Columns["TeacherFullName"].HeaderText = "Назначенный учитель";
                }
            }
        }

        public WorkloadView? SelectedWorkloadItem => workloadGrid.CurrentRow?.DataBoundItem as WorkloadView;
        public ClassDetails? SelectedWorkloadClass => workloadClassComboBox.SelectedItem as ClassDetails;
        public AcademicYear? SelectedWorkloadAcademicYear => yearComboBox.SelectedItem as AcademicYear;

        public IList<AcademicYear> AcademicYearsList
        {
            set
            {
                _academicYearsMaster = value.ToList();
                _academicYearsBindingList = new SortableBindingList<AcademicYear>(_academicYearsMaster);
                academicYearsGrid.DataSource = _academicYearsBindingList;
                if (academicYearsGrid.Columns.Count > 0)
                {
                    academicYearsGrid.Columns["Id"].Visible = false;
                    academicYearsGrid.Columns["Name"].HeaderText = "Учебный год";
                    academicYearsGrid.Columns["StartDate"].HeaderText = "Дата начала";
                    academicYearsGrid.Columns["EndDate"].HeaderText = "Дата окончания";
                    academicYearsGrid.Columns["Status"].HeaderText = "Статус";
                }
            }
        }
        public AcademicYear? SelectedAcademicYear => academicYearsGrid.CurrentRow?.DataBoundItem as AcademicYear;


        public event Action? LoadTeachers;
        public event Action? AddTeacher;
        public event Action? EditTeacher;
        public event Action? DeleteTeacher;
        public event Action<string>? SearchTeacher;

        public event Action? LoadClasses;
        public event Action<ClassDetails?>? ClassSelected;
        public event Action? AddClass;
        public event Action? EditClass;
        public event Action? AddStudent;
        public event Action? EditStudent;
        public event Action? ExpelStudent;
        public event Action? TransferStudent;

        public event Action? LoadStudyPlans;
        public event Action<StudyPlanView?>? StudyPlanSelected;
        public event Action? AddStudyPlan;
        public event Action? AddStudyPlanItem;
        public event Action? DeleteStudyPlanItem;

        public event Action? LoadWorkloadData;
        public event Action<int?, int?>? WorkloadFilterChanged;
        public event Action? AssignTeacherToWorkload;
        public event Action? RemoveTeacherFromWorkload;

        public event Action? LoadAcademicYears;
        public event Action? PromoteStudents;
        public event Action<int, string>? UpdateYearStatus;

        public void ShowError(string message) => MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        public void ShowSuccess(string message) => MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        public bool ShowConfirmation(string message) => MessageBox.Show(message, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        private void ReportsListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            reportsFilterPanel.Controls.Clear();

            reportsResultGrid.CellFormatting -= WorkloadReport_CellFormatting;

            var resultPanel = this.Controls.Find("MovementReportPanel", true).FirstOrDefault();
            if (resultPanel != null) resultPanel.Visible = false;

            reportsResultGrid.Visible = true;
            reportsResultGrid.DataSource = null;

            string? selectedReport = reportsListBox.SelectedItem?.ToString();

            if (selectedReport == "Сводный отчет по успеваемости")
            {
                SetupPerformanceReportFilters();
            }
            else if (selectedReport == "Нагрузка преподавателей")
            {
                SetupTeacherWorkloadReportFilters();
            }
            else if (selectedReport == "Движение контингента")
            {
                reportsResultGrid.Visible = false;
                if (resultPanel != null) resultPanel.Visible = true;
                SetupStudentMovementReportUI();
            }
        }

        private void SetupStudentMovementReportUI()
        {
            var startDateLabel = new Label { Text = "Начало периода:", Left = 10, Top = 10, AutoSize = true };
            _reportStartDatePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Left = 120, Top = 10, Width = 120, Value = DateTime.Now.AddMonths(-1) };
            var endDateLabel = new Label { Text = "Конец периода:", Left = 10, Top = 40, AutoSize = true };
            _reportEndDatePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Left = 120, Top = 40, Width = 120 };

            _generateReportButton = new Button { Text = "Сформировать", Left = 10, Top = 70, Width = 150 };
            _generateReportButton.Click += GenerateStudentMovementReport_Click;

            reportsFilterPanel.Controls.AddRange(new Control[] {
                startDateLabel, _reportStartDatePicker,
                endDateLabel, _reportEndDatePicker,
                _generateReportButton
            });

            reportsResultGrid.Visible = false;

            var resultPanel = new Panel { Name = "MovementReportPanel", Dock = DockStyle.Fill, AutoScroll = true };
            var oldPanel = this.Controls.Find("MovementReportPanel", true).FirstOrDefault();
            oldPanel?.Dispose();

            splitReports.Panel2.Controls.Add(resultPanel);
            resultPanel.BringToFront();
        }

        private async void GenerateStudentMovementReport_Click(object? sender, EventArgs e)
        {
            var startDate = _reportStartDatePicker.Value;
            var endDate = _reportEndDatePicker.Value;

            if (startDate > endDate)
            {
                ShowError("Дата начала периода не может быть позже даты окончания.");
                return;
            }

            var resultPanel = this.Controls.Find("MovementReportPanel", true).FirstOrDefault() as Panel;
            if (resultPanel == null) return;

            resultPanel.Controls.Clear();

            try
            {
                _generateReportButton.Enabled = false;
                _generateReportButton.Text = "Загрузка...";

                var reportData = await Task.Run(() => _dataAccess.GetStudentMovementReport(startDate, endDate));

                if (reportData == null)
                {
                    ShowError("Не удалось получить данные для отчета.");
                    return;
                }

                var rootLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 2,
                    Padding = new Padding(0)
                };
                rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
                rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));


                var summaryGroup = new GroupBox
                {
                    Text = "Сводка за период",
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular)
                };

                var summaryLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 4,
                    RowCount = 2,
                    Padding = new Padding(10)
                };

                summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

                Label CreateHeaderLabel(string text) => new Label { Text = text, AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9) };
                Label CreateValueLabel(string text) => new Label { Text = text, AutoSize = true, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.DarkSlateBlue };

                summaryLayout.Controls.Add(CreateHeaderLabel("На начало периода:"), 0, 0);
                summaryLayout.Controls.Add(CreateValueLabel(reportData.TotalAtStart.ToString()), 0, 1);

                summaryLayout.Controls.Add(CreateHeaderLabel("Прибыло:"), 1, 0);
                summaryLayout.Controls.Add(CreateValueLabel($"+{reportData.ArrivedCount}"), 1, 1);

                summaryLayout.Controls.Add(CreateHeaderLabel("Выбыло:"), 2, 0);
                summaryLayout.Controls.Add(CreateValueLabel($"-{reportData.DepartedCount}"), 2, 1);

                summaryLayout.Controls.Add(CreateHeaderLabel("На конец периода:"), 3, 0);
                summaryLayout.Controls.Add(CreateValueLabel(reportData.TotalAtEnd.ToString()), 3, 1);

                summaryGroup.Controls.Add(summaryLayout);


                var listsContainer = new SplitContainer
                {
                    Dock = DockStyle.Fill,
                    Orientation = Orientation.Horizontal,
                    SplitterWidth = 10
                };

                var arrivedBox = new GroupBox { Text = "Прибывшие учащиеся", Dock = DockStyle.Fill, Padding = new Padding(3, 22, 3, 3) };
                var arrivedGrid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToResizeRows = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    RowHeadersVisible = false,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    AutoGenerateColumns = false
                };

                arrivedGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FullName", HeaderText = "ФИО Ученика", FillWeight = 40 });
                arrivedGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ClassName", HeaderText = "Класс", FillWeight = 15 });
                arrivedGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EnrollmentDate", HeaderText = "Дата зачисления", DefaultCellStyle = new DataGridViewCellStyle { Format = "d" }, FillWeight = 20 });

                arrivedGrid.DataSource = reportData.ArrivedStudents;
                arrivedBox.Controls.Add(arrivedGrid);


                var departedBox = new GroupBox { Text = "Выбывшие учащиеся", Dock = DockStyle.Fill, Padding = new Padding(3, 22, 3, 3) };
                var departedGrid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToResizeRows = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    RowHeadersVisible = false,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    AutoGenerateColumns = false
                };

                departedGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FullName", HeaderText = "ФИО Ученика", FillWeight = 40 });
                departedGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ClassName", HeaderText = "Был в классе", FillWeight = 15 });
                departedGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DepartureDate", HeaderText = "Дата выбытия", DefaultCellStyle = new DataGridViewCellStyle { Format = "d" }, FillWeight = 20 });

                departedGrid.DataSource = reportData.DepartedStudents;
                departedBox.Controls.Add(departedGrid);

                listsContainer.Panel1.Controls.Add(arrivedBox);
                listsContainer.Panel2.Controls.Add(departedBox);

                rootLayout.Controls.Add(summaryGroup, 0, 0);
                rootLayout.Controls.Add(listsContainer, 0, 1);

                resultPanel.Controls.Add(rootLayout);
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка формирования отчета: {ex.Message}");
            }
            finally
            {
                _generateReportButton.Enabled = true;
                _generateReportButton.Text = "Сформировать";
            }
        }

        private void WorkloadReport_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs args)
        {
            if (reportsResultGrid.Columns["TeacherFullName"] == null) return;

            if (args.RowIndex > 0 && args.ColumnIndex == reportsResultGrid.Columns["TeacherFullName"].Index)
            {
                var currentTeacher = args.Value?.ToString();
                var prevTeacher = reportsResultGrid.Rows[args.RowIndex - 1].Cells["TeacherFullName"].Value?.ToString();

                if (currentTeacher == prevTeacher)
                {
                    args.Value = "";
                    args.FormattingApplied = true;
                }
            }
        }

        private void SetupTeacherWorkloadReportFilters()
        {
            var yearLabel = new Label { Text = "Учебный год:", Left = 10, Top = 10, AutoSize = true };
            _reportYearComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Left = 100, Top = 10, Width = 150 };
            _reportYearComboBox.DataSource = _academicYearsMaster;
            _reportYearComboBox.DisplayMember = "Name";
            _reportYearComboBox.ValueMember = "Id";

            _generateReportButton = new Button { Text = "Сформировать", Left = 10, Top = 40, Width = 150 };
            _generateReportButton.Click += GenerateTeacherWorkloadReport_Click;

            reportsFilterPanel.Controls.AddRange(new Control[] {
                yearLabel, _reportYearComboBox,
                _generateReportButton
            });
        }

        private async void GenerateTeacherWorkloadReport_Click(object? sender, EventArgs e)
        {
            if (_reportYearComboBox.SelectedValue is not int yearId)
            {
                ShowError("Выберите учебный год.");
                return;
            }

            try
            {
                _generateReportButton.Enabled = false;
                _generateReportButton.Text = "Загрузка...";

                var reportData = await Task.Run(() => _dataAccess.GetTeacherWorkloadReport(yearId));
                var dataList = reportData.ToList();

                _workloadReportBindingList = new SortableBindingList<TeacherWorkloadReportItem>(dataList);

                reportsResultGrid.CellFormatting -= WorkloadReport_CellFormatting;

                reportsResultGrid.DataSource = _workloadReportBindingList;

                if (reportsResultGrid.Columns.Count > 0)
                {
                    reportsResultGrid.Columns["TeacherId"].Visible = false;

                    var colTeacher = reportsResultGrid.Columns["TeacherFullName"];
                    colTeacher.HeaderText = "ФИО Учителя";
                    colTeacher.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    colTeacher.FillWeight = 40;

                    reportsResultGrid.Columns["ClassName"].HeaderText = "Класс";
                    reportsResultGrid.Columns["DisciplineName"].HeaderText = "Предмет";
                    reportsResultGrid.Columns["LessonsCount"].HeaderText = "Часов";
                }

                reportsResultGrid.CellFormatting += WorkloadReport_CellFormatting;
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка формирования отчета: {ex.Message}");
            }
            finally
            {
                _generateReportButton.Enabled = true;
                _generateReportButton.Text = "Сформировать";
            }
        }

        private void SetupPerformanceReportFilters()
        {
            var yearLabel = new Label { Text = "Учебный год:", Left = 10, Top = 10, AutoSize = true };
            _reportYearComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Left = 100, Top = 10, Width = 150 };
            _reportYearComboBox.DataSource = _academicYearsMaster;
            _reportYearComboBox.DisplayMember = "Name";
            _reportYearComboBox.ValueMember = "Id";

            var startDateLabel = new Label { Text = "Начало периода:", Left = 270, Top = 10, AutoSize = true };
            _reportStartDatePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Left = 380, Top = 10, Width = 120 };
            var endDateLabel = new Label { Text = "Конец периода:", Left = 270, Top = 40, AutoSize = true };
            _reportEndDatePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Left = 380, Top = 40, Width = 120 };

            _reportYearComboBox.SelectedIndexChanged += (s, e) => {
                if (_reportYearComboBox.SelectedItem is AcademicYear selectedYear)
                {
                    _reportStartDatePicker.Value = selectedYear.StartDate;
                    _reportEndDatePicker.Value = selectedYear.EndDate;
                }
            };
            if (_reportYearComboBox.Items.Count > 0) _reportYearComboBox.SelectedIndex = 0;

            _generateReportButton = new Button { Text = "Сформировать", Left = 10, Top = 70, Width = 150 };
            _generateReportButton.Click += GeneratePerformanceReport_Click;

            reportsFilterPanel.Controls.AddRange(new Control[] {
                yearLabel, _reportYearComboBox,
                startDateLabel, _reportStartDatePicker,
                endDateLabel, _reportEndDatePicker,
                _generateReportButton
            });
        }

        private async void GeneratePerformanceReport_Click(object? sender, EventArgs e)
        {
            if (_reportYearComboBox.SelectedValue is not int yearId)
            {
                ShowError("Выберите учебный год.");
                return;
            }

            var startDate = _reportStartDatePicker.Value;
            var endDate = _reportEndDatePicker.Value;

            try
            {
                _generateReportButton.Enabled = false;
                _generateReportButton.Text = "Загрузка...";
                var reportData = await Task.Run(() => _dataAccess.GetAcademicPerformanceSummary(yearId, startDate, endDate));

                _performanceReportBindingList = new SortableBindingList<AcademicPerformanceReportItem>(reportData.ToList());
                reportsResultGrid.DataSource = _performanceReportBindingList;

                if (reportsResultGrid.Columns.Count > 0)
                {
                    reportsResultGrid.Columns["ParallelNumber"].HeaderText = "Параллель";
                    reportsResultGrid.Columns["ClassName"].HeaderText = "Класс";
                    reportsResultGrid.Columns["DisciplineName"].HeaderText = "Предмет";
                    reportsResultGrid.Columns["AvgGrade"].HeaderText = "Средний балл";
                    reportsResultGrid.Columns["QualityPercent"].HeaderText = "Качество (%)";
                    reportsResultGrid.Columns["SuccessPercent"].HeaderText = "Успеваемость (%)";
                    reportsResultGrid.Columns["TotalGrades"].HeaderText = "Всего оценок";
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка формирования отчета: {ex.Message}");
            }
            finally
            {
                _generateReportButton.Enabled = true;
                _generateReportButton.Text = "Сформировать";
            }
        }

        private void ShowCreateYearDialog()
        {
            using var form = new Form
            {
                Text = "Создание нового учебного года",
                Width = 400,
                Height = 220,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };
            var nameLabel = new Label { Text = "Название (ГГГГ-ГГГГ)*:", Left = 10, Top = 20 };
            var nameText = new TextBox { Left = 150, Top = 20, Width = 220, Text = $"{DateTime.Now.Year}-{DateTime.Now.Year + 1}" };
            var startLabel = new Label { Text = "Дата начала*:", Left = 10, Top = 50 };
            var startDatePicker = new DateTimePicker { Left = 150, Top = 50, Width = 220, Format = DateTimePickerFormat.Short, Value = new DateTime(DateTime.Now.Year, 9, 1) };
            var endLabel = new Label { Text = "Дата окончания*:", Left = 10, Top = 80 };
            var endDatePicker = new DateTimePicker { Left = 150, Top = 80, Width = 220, Format = DateTimePickerFormat.Short, Value = new DateTime(DateTime.Now.Year + 1, 5, 31) };
            var okButton = new Button { Text = "Создать", Left = 200, Top = 130, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 130, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { nameLabel, nameText, startLabel, startDatePicker, endLabel, endDatePicker, okButton, cancelButton });
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(nameText.Text)) { ShowError("Название года не может быть пустым."); return; }
                if (endDatePicker.Value <= startDatePicker.Value) { ShowError("Дата окончания должна быть позже даты начала."); return; }
                try
                {
                    Task.Run(() => _dataAccess.CreateAcademicYear(nameText.Text, startDatePicker.Value, endDatePicker.Value)).Wait();
                    ShowSuccess("Новый учебный год успешно создан.");
                    LoadAcademicYears?.Invoke();
                }
                catch (Exception ex) { ShowError($"Ошибка создания года: {ex.InnerException?.Message ?? ex.Message}"); }
            }
        }

        public void ShowTeacherDialog(TeacherDetails? teacher, bool isNew)
        {
            teacher ??= new TeacherDetails { Role = "teacher" };
            using var form = new Form
            {
                Text = isNew ? "Добавить нового учителя" : "Редактировать данные учителя",
                Width = 650,
                Height = 420,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            var pbAvatar = new PictureBox
            {
                Size = new Size(120, 120),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Left = 460,
                Top = 20
            };
            byte[]? currentImage = teacher.AvatarImage;
            if (currentImage != null && currentImage.Length > 0)
            {
                using (var ms = new MemoryStream(currentImage)) pbAvatar.Image = new Bitmap(ms);
            }

            var btnUpload = new Button { Text = "Загрузить фото", Left = 460, Top = 150, Width = 120 };
            var btnDelete = new Button { Text = "Удалить фото", Left = 460, Top = 180, Width = 120, Visible = (currentImage != null) };

            btnUpload.Click += (s, e) => {
                using (var ofd = new OpenFileDialog { Filter = "Images|*.jpg;*.png;*.bmp" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            byte[] bytes = File.ReadAllBytes(ofd.FileName);
                            if (bytes.Length > 2 * 1024 * 1024) { ShowError("Файл > 2МБ"); return; }

                            if (pbAvatar.Image != null) pbAvatar.Image.Dispose();
                            using (var ms = new MemoryStream(bytes)) pbAvatar.Image = new Bitmap(ms);
                            currentImage = bytes;
                            btnDelete.Visible = true;
                        }
                        catch { MessageBox.Show("Ошибка чтения файла"); }
                    }
                }
            };

            btnDelete.Click += (s, e) => {
                if (pbAvatar.Image != null) pbAvatar.Image.Dispose();
                pbAvatar.Image = null;
                currentImage = null;
                btnDelete.Visible = false;
            };

            var lastNameLabel = new Label { Text = "Фамилия*:", Left = 10, Top = 20 };
            var lastNameText = new TextBox { Text = teacher.LastName, Left = 150, Top = 20, Width = 250 };
            var firstNameLabel = new Label { Text = "Имя*:", Left = 10, Top = 50 };
            var firstNameText = new TextBox { Text = teacher.FirstName, Left = 150, Top = 50, Width = 250 };
            var patronymicLabel = new Label { Text = "Отчество:", Left = 10, Top = 80 };
            var patronymicText = new TextBox { Text = teacher.Patronymic, Left = 150, Top = 80, Width = 250 };
            var phoneLabel = new Label { Text = "Телефон:", Left = 10, Top = 110 };
            var phoneText = new TextBox { Text = teacher.Phone, Left = 150, Top = 110, Width = 250 };
            var emailLabel = new Label { Text = "Email:", Left = 10, Top = 140 };
            var emailText = new TextBox { Text = teacher.Email, Left = 150, Top = 140, Width = 250 };
            var loginLabel = new Label { Text = "Логин*:", Left = 10, Top = 170 };
            var loginText = new TextBox { Text = teacher.Login, Left = 150, Top = 170, Width = 250 };
            var passwordLabel = new Label { Text = "Пароль*:", Left = 10, Top = 200 };
            var passwordText = new TextBox { Left = 150, Top = 200, Width = 250, UseSystemPasswordChar = true };
            passwordLabel.Visible = isNew;
            passwordText.Visible = isNew;

            var roleLabel = new Label { Text = "Роль*:", Left = 10, Top = 230 };
            var roleCombo = new ComboBox { Left = 150, Top = 230, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            roleCombo.Items.AddRange(new object[] { "Учитель", "Завуч" });
            roleCombo.SelectedItem = teacher.Role == "admin" ? "Завуч" : "Учитель";

            var notesLabel = new Label { Text = "Заметки:", Left = 10, Top = 260 };
            var notesText = new TextBox { Text = teacher.Notes, Left = 150, Top = 260, Width = 250, Height = 60, Multiline = true, ScrollBars = ScrollBars.Vertical };
            var okButton = new Button { Text = "Сохранить", Left = 260, Top = 330, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 350, Top = 330, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { pbAvatar, btnUpload, btnDelete, lastNameLabel, lastNameText, firstNameLabel, firstNameText, patronymicLabel, patronymicText, phoneLabel, phoneText, emailLabel, emailText, loginLabel, loginText, passwordLabel, passwordText, roleLabel, roleCombo, notesLabel, notesText, okButton, cancelButton });
            form.AcceptButton = okButton; form.CancelButton = cancelButton;
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(lastNameText.Text) || string.IsNullOrWhiteSpace(firstNameText.Text) || string.IsNullOrWhiteSpace(loginText.Text) || (isNew && string.IsNullOrWhiteSpace(passwordText.Text)))
                {
                    ShowError("Поля, отмеченные звездочкой (*), обязательны для заполнения."); return;
                }
                teacher.LastName = lastNameText.Text; teacher.FirstName = firstNameText.Text; teacher.Patronymic = patronymicText.Text; teacher.Phone = phoneText.Text; teacher.Email = emailText.Text; teacher.Login = loginText.Text;
                if (isNew) { var (hash, salt) = PasswordHasher.HashPassword(passwordText.Text); teacher.PasswordHash = hash; teacher.PasswordSalt = salt; }

                teacher.Role = roleCombo.SelectedItem?.ToString() == "Завуч" ? "admin" : "teacher";

                teacher.Notes = notesText.Text;

                teacher.AvatarImage = currentImage;

                try
                {
                    if (isNew) { Task.Run(() => _dataAccess.AddTeacher(teacher)).Wait(); ShowSuccess("Новый учитель успешно добавлен."); }
                    else { Task.Run(() => _dataAccess.UpdateTeacher(teacher)).Wait(); ShowSuccess("Данные учителя успешно обновлены."); }
                    LoadTeachers?.Invoke();
                }
                catch (Exception ex) { ShowError($"Ошибка сохранения: {ex.InnerException?.Message ?? ex.Message}"); }
            }
        }

        public void ShowClassDialog(List<TeacherDetails> allTeachers)
        {
            using var form = new Form
            {
                Text = "Создание нового класса",
                Width = 400,
                Height = 220,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var parallelLabel = new Label { Text = "Параллель:", Left = 10, Top = 20 };
            var parallelNumeric = new NumericUpDown { Minimum = 1, Maximum = 11, Left = 180, Top = 20, Width = 190 };

            var letterLabel = new Label { Text = "Буква:", Left = 10, Top = 50 };
            var letterText = new TextBox { MaxLength = 1, Left = 180, Top = 50, Width = 190 };

            letterText.CharacterCasing = CharacterCasing.Upper;
            letterText.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Back) return;

                bool isCyrillic = (e.KeyChar >= 'А' && e.KeyChar <= 'я') || e.KeyChar == 'Ё' || e.KeyChar == 'ё';

                if (!isCyrillic)
                {
                    e.Handled = true;
                }
            };

            var teacherLabel = new Label { Text = "Классный руководитель:", Left = 10, Top = 80 };
            var teacherCombo = new ComboBox { Left = 180, Top = 80, Width = 190, DropDownStyle = ComboBoxStyle.DropDownList };

            var teacherListForCombo = new List<TeacherDetails> { new TeacherDetails { Id = 0, LastName = "(Без руководителя)" } };
            teacherListForCombo.AddRange(allTeachers);

            teacherCombo.DataSource = teacherListForCombo;
            teacherCombo.DisplayMember = "FullName";
            teacherCombo.ValueMember = "Id";

            var okButton = new Button { Text = "Создать", Left = 200, Top = 130, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 130, DialogResult = DialogResult.Cancel };

            form.Controls.AddRange(new Control[] { parallelLabel, parallelNumeric, letterLabel, letterText, teacherLabel, teacherCombo, okButton, cancelButton });

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(letterText.Text))
                {
                    ShowError("Необходимо указать букву класса."); return;
                }

                try
                {
                    var letter = letterText.Text.ToUpper();
                    var parallel = (int)parallelNumeric.Value;
                    int? teacherId = teacherCombo.SelectedValue is int selectedId && selectedId > 0 ? selectedId : null;

                    Task.Run(() => _dataAccess.AddClass(letter, parallel, teacherId)).Wait();

                    ShowSuccess($"Класс {parallel} \"{letter}\" успешно создан.");
                    LoadClasses?.Invoke();
                }
                catch (Exception ex)
                {
                    ShowError(ex.InnerException?.Message ?? ex.Message);
                }
            }
        }

        public void ShowHeadTeacherDialog(ClassDetails selectedClass, List<TeacherDetails> allTeachers)
        {
            using var form = new Form { Text = $"Руководитель класса {selectedClass.ClassName}", Width = 400, Height = 180, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
            var teacherLabel = new Label { Text = "Выберите нового руководителя:", Left = 10, Top = 20 };
            var teacherCombo = new ComboBox { Left = 10, Top = 45, Width = 360, DropDownStyle = ComboBoxStyle.DropDownList };
            var teacherListForCombo = new List<TeacherDetails> { new TeacherDetails { Id = 0, LastName = "(Без руководителя)" } };
            teacherListForCombo.AddRange(allTeachers);
            teacherCombo.DataSource = teacherListForCombo; teacherCombo.DisplayMember = "FullName"; teacherCombo.ValueMember = "Id";
            teacherCombo.SelectedValue = selectedClass.HeadTeacherId ?? 0;
            var okButton = new Button { Text = "Назначить", Left = 200, Top = 90, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 90, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { teacherLabel, teacherCombo, okButton, cancelButton });
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int? newTeacherId = teacherCombo.SelectedValue is int selectedId && selectedId > 0 ? selectedId : null;
                    Task.Run(() => _dataAccess.UpdateClassHeadTeacher(selectedClass.ClassId, newTeacherId)).Wait();
                    ShowSuccess("Классный руководитель успешно обновлен."); LoadClasses?.Invoke();
                }
                catch (Exception ex) when (ex.InnerException is NpgsqlException pgEx && pgEx.SqlState == "23505") { ShowError("Ошибка назначения: этот учитель уже является классным руководителем в другом классе."); }
                catch (Exception ex) { ShowError($"Ошибка обновления: {ex.InnerException?.Message ?? ex.Message}"); }
            }
        }

        public void ShowStudentDialog(StudentDetails student, bool isNew, List<ClassDetails> allClasses)
        {
            using var form = new Form
            {
                Text = isNew ? "Добавить нового ученика" : "Редактировать данные ученика",
                Width = 650,
                Height = isNew ? 420 : 380,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var pbAvatar = new PictureBox { Size = new Size(120, 120), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle, Left = 460, Top = 20 };
            byte[]? currentImage = student.AvatarImage;
            if (currentImage != null && currentImage.Length > 0) using (var ms = new MemoryStream(currentImage)) pbAvatar.Image = new Bitmap(ms);

            var btnUpload = new Button { Text = "Загрузить фото", Left = 460, Top = 150, Width = 120 };
            var btnDelete = new Button { Text = "Удалить фото", Left = 460, Top = 180, Width = 120, Visible = (currentImage != null) };

            btnUpload.Click += (s, e) => {
                using (var ofd = new OpenFileDialog { Filter = "Images|*.jpg;*.png;*.bmp" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            byte[] bytes = File.ReadAllBytes(ofd.FileName);
                            if (bytes.Length > 2 * 1024 * 1024) { ShowError("Файл слишком большой (> 2МБ)"); return; }
                            if (pbAvatar.Image != null) pbAvatar.Image.Dispose();
                            using (var ms = new MemoryStream(bytes)) pbAvatar.Image = new Bitmap(ms);
                            currentImage = bytes;
                            btnDelete.Visible = true;
                        }
                        catch { MessageBox.Show("Ошибка чтения файла"); }
                    }
                }
            };
            btnDelete.Click += (s, e) => {
                if (pbAvatar.Image != null) pbAvatar.Image.Dispose();
                pbAvatar.Image = null;
                currentImage = null;
                btnDelete.Visible = false;
            };

            var lastNameLabel = new Label { Text = "Фамилия*:", Left = 10, Top = 20 };
            var lastNameText = new TextBox { Text = student.LastName, Left = 150, Top = 20, Width = 250 };

            var firstNameLabel = new Label { Text = "Имя*:", Left = 10, Top = 50 };
            var firstNameText = new TextBox { Text = student.FirstName, Left = 150, Top = 50, Width = 250 };

            var patronymicLabel = new Label { Text = "Отчество:", Left = 10, Top = 80 };
            var patronymicText = new TextBox { Text = student.Patronymic, Left = 150, Top = 80, Width = 250 };

            var birthDateLabel = new Label { Text = "Дата рождения*:", Left = 10, Top = 110 };
            var birthDatePicker = new DateTimePicker { Value = student.BirthDate, Left = 150, Top = 110, Width = 250, Format = DateTimePickerFormat.Short };

            var classLabel = new Label { Text = "Класс*:", Left = 10, Top = 140 };
            var classCombo = new ComboBox { Left = 150, Top = 140, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            classCombo.Items.Clear();
            foreach (var cls in allClasses) classCombo.Items.Add(cls);
            classCombo.DisplayMember = "ClassName";

            if (student.ClassId > 0)
            {
                for (int i = 0; i < classCombo.Items.Count; i++)
                {
                    if (classCombo.Items[i] is ClassDetails item && item.ClassId == student.ClassId)
                    {
                        classCombo.SelectedIndex = i;
                        break;
                    }
                }
            }

            var notesLabel = new Label { Text = "Заметки:", Left = 10, Top = 170 };
            var notesText = new TextBox { Text = student.Notes, Left = 150, Top = 170, Width = 250, Height = 60, Multiline = true, ScrollBars = ScrollBars.Vertical };

            var loginLabel = new Label { Text = "Логин*:", Left = 10, Top = 250 };
            var loginText = new TextBox { Text = student.Login, Left = 150, Top = 250, Width = 250 };

            if (isNew && string.IsNullOrEmpty(student.Login))
            {
                loginText.Text = $"student_{DateTime.Now.Ticks % 10000}";
            }

            Label? passwordLabel = null;
            TextBox? passwordText = null;
            if (isNew)
            {
                passwordLabel = new Label { Text = "Пароль*:", Left = 10, Top = 280 };
                passwordText = new TextBox { Left = 150, Top = 280, Width = 250, UseSystemPasswordChar = true };
            }

            int buttonTop = isNew ? 330 : 290;
            var okButton = new Button { Text = "Сохранить", Left = 260, Top = buttonTop, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 350, Top = buttonTop, DialogResult = DialogResult.Cancel };

            form.Controls.AddRange(new Control[] { pbAvatar, btnUpload, btnDelete, lastNameLabel, lastNameText, firstNameLabel, firstNameText, patronymicLabel, patronymicText, birthDateLabel, birthDatePicker, classLabel, classCombo, notesLabel, notesText, loginLabel, loginText, okButton, cancelButton });
            if (isNew && passwordLabel != null) { form.Controls.Add(passwordLabel); form.Controls.Add(passwordText!); }

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(lastNameText.Text) || string.IsNullOrWhiteSpace(firstNameText.Text) || classCombo.SelectedIndex < 0 || string.IsNullOrWhiteSpace(loginText.Text) || (isNew && string.IsNullOrWhiteSpace(passwordText?.Text)))
                {
                    ShowError("Поля, отмеченные звездочкой (*), обязательны для заполнения."); return;
                }

                student.LastName = lastNameText.Text;
                student.FirstName = firstNameText.Text;
                student.Patronymic = patronymicText.Text;
                student.BirthDate = birthDatePicker.Value;
                if (classCombo.SelectedItem is ClassDetails selectedDetails) student.ClassId = selectedDetails.ClassId;
                student.Login = loginText.Text;
                if (isNew) student.Password = passwordText!.Text;
                student.Notes = notesText.Text;
                student.AvatarImage = currentImage;

                try
                {
                    if (isNew) { Task.Run(() => _dataAccess.AddStudent(student)).Wait(); ShowSuccess("Новый ученик успешно добавлен."); }
                    else { Task.Run(() => _dataAccess.UpdateStudent(student)).Wait(); ShowSuccess("Данные ученика успешно обновлены."); }
                    ClassSelected?.Invoke(SelectedClass);
                }
                catch (Exception ex) { ShowError($"Ошибка сохранения ученика: {ex.InnerException?.Message ?? ex.Message}"); }
            }
        }

        public void ShowTransferStudentDialog(StudentInfo selectedStudent, List<ClassDetails> allClasses)
        {
            using var form = new Form { Text = $"Перевод ученика: {selectedStudent.FullName}", Width = 400, Height = 180, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
            var classLabel = new Label { Text = "Выберите новый класс:", Left = 10, Top = 20 };
            var classCombo = new ComboBox { Left = 10, Top = 45, Width = 360, DropDownStyle = ComboBoxStyle.DropDownList };
            classCombo.DataSource = allClasses; classCombo.DisplayMember = "ClassName"; classCombo.ValueMember = "ClassId";
            var okButton = new Button { Text = "Перевести", Left = 200, Top = 90, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 90, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { classLabel, classCombo, okButton, cancelButton }); form.AcceptButton = okButton; form.CancelButton = cancelButton;
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (classCombo.SelectedValue is int newClassId)
                {
                    try { Task.Run(() => _dataAccess.TransferStudent(selectedStudent.Id, newClassId)).Wait(); ShowSuccess("Ученик успешно переведен."); LoadClasses?.Invoke(); }
                    catch (Exception ex) { ShowError($"Ошибка перевода: {ex.InnerException?.Message ?? ex.Message}"); }
                }
            }
        }

        public void ShowParentDialog(ParentInfo? parent, bool isNew)
        {
            parent ??= new ParentInfo();
            using var form = new Form
            {
                Text = isNew ? "Добавить нового родителя" : "Редактировать данные родителя",
                Width = 650,
                Height = isNew ? 380 : 350,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            var pbAvatar = new PictureBox
            {
                Size = new Size(120, 120),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Left = 460,
                Top = 20
            };
            byte[]? currentImage = parent.AvatarImage;
            if (currentImage != null && currentImage.Length > 0)
            {
                using (var ms = new MemoryStream(currentImage)) pbAvatar.Image = new Bitmap(ms);
            }

            var btnUpload = new Button { Text = "Загрузить фото", Left = 460, Top = 150, Width = 120 };
            var btnDelete = new Button { Text = "Удалить фото", Left = 460, Top = 180, Width = 120, Visible = (currentImage != null) };

            btnUpload.Click += (s, e) => {
                using (var ofd = new OpenFileDialog { Filter = "Images|*.jpg;*.png;*.bmp" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            byte[] bytes = File.ReadAllBytes(ofd.FileName);
                            if (bytes.Length > 2 * 1024 * 1024) { ShowError("Файл > 2МБ"); return; }
                            if (pbAvatar.Image != null) pbAvatar.Image.Dispose();
                            using (var ms = new MemoryStream(bytes)) pbAvatar.Image = new Bitmap(ms);
                            currentImage = bytes;
                            btnDelete.Visible = true;
                        }
                        catch { MessageBox.Show("Ошибка чтения файла"); }
                    }
                }
            };

            btnDelete.Click += (s, e) => {
                if (pbAvatar.Image != null) pbAvatar.Image.Dispose();
                pbAvatar.Image = null;
                currentImage = null;
                btnDelete.Visible = false;
            };

            var lastNameLabel = new Label { Text = "Фамилия*:", Left = 10, Top = 20 };
            var lastNameText = new TextBox { Text = parent.LastName, Left = 150, Top = 20, Width = 250 };
            var firstNameLabel = new Label { Text = "Имя*:", Left = 10, Top = 50 };
            var firstNameText = new TextBox { Text = parent.FirstName, Left = 150, Top = 50, Width = 250 };
            var patronymicLabel = new Label { Text = "Отчество:", Left = 10, Top = 80 };
            var patronymicText = new TextBox { Text = parent.Patronymic, Left = 150, Top = 80, Width = 250 };
            var phoneLabel = new Label { Text = "Телефон:", Left = 10, Top = 110 };
            var phoneText = new TextBox { Text = parent.Phone, Left = 150, Top = 110, Width = 250 };
            var emailLabel = new Label { Text = "Email:", Left = 10, Top = 140 };
            var emailText = new TextBox { Text = parent.Email, Left = 150, Top = 140, Width = 250 };

            var loginLabel = new Label { Text = "Логин*:", Left = 10, Top = 170 };
            var loginText = new TextBox { Text = parent.Login, Left = 150, Top = 170, Width = 250 };

            if (isNew && string.IsNullOrEmpty(parent.Login) && SelectedStudent != null)
            {
                var studentLastName = SelectedStudent.LastName.ToLower();
                var studentFirstNameInitial = SelectedStudent.FirstName.ToLower().FirstOrDefault();
                loginText.Text = $"{studentLastName}_{studentFirstNameInitial}_parent";
            }

            form.Controls.AddRange(new Control[] {
                pbAvatar, btnUpload, btnDelete,
                lastNameLabel, lastNameText, firstNameLabel, firstNameText,
                patronymicLabel, patronymicText, phoneLabel, phoneText,
                emailLabel, emailText, loginLabel, loginText
            });

            TextBox? passwordText = null;
            if (isNew)
            {
                var passwordLabel = new Label { Text = "Пароль*:", Left = 10, Top = 200 };
                passwordText = new TextBox { Left = 150, Top = 200, Width = 250, UseSystemPasswordChar = true };
                form.Controls.AddRange(new Control[] { passwordLabel, passwordText });
            }

            var okButton = new Button { Text = "Сохранить", Left = 260, Top = form.ClientSize.Height - 50, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 350, Top = form.ClientSize.Height - 50, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { okButton, cancelButton });
            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(lastNameText.Text) || string.IsNullOrWhiteSpace(firstNameText.Text) ||
                    string.IsNullOrWhiteSpace(loginText.Text) ||
                    (isNew && string.IsNullOrWhiteSpace(passwordText?.Text)))
                {
                    ShowError("Поля, отмеченные звездочкой (*), обязательны для заполнения.");
                    return;
                }

                parent.LastName = lastNameText.Text;
                parent.FirstName = firstNameText.Text;
                parent.Patronymic = patronymicText.Text;
                parent.Phone = phoneText.Text;
                parent.Email = emailText.Text;
                parent.Login = loginText.Text;
                parent.AvatarImage = currentImage;

                try
                {
                    if (isNew && passwordText != null)
                    {
                        var (hash, salt) = PasswordHasher.HashPassword(passwordText.Text);
                        parent.PasswordHash = hash;
                        parent.PasswordSalt = salt;

                        int newParentId = _dataAccess.AddParent(parent);
                        _dataAccess.LinkStudentToParent(SelectedStudent!.Id, newParentId);
                        ShowSuccess("Новый родитель успешно создан и привязан к ученику.");
                    }
                    else
                    {
                        _dataAccess.UpdateParent(parent);
                        ShowSuccess("Данные родителя успешно обновлены.");
                    }
                    StudentSelected?.Invoke(SelectedStudent);
                }
                catch (Exception ex)
                {
                    ShowError($"Ошибка сохранения: {ex.InnerException?.Message ?? ex.Message}");
                }
            }
        }

        public void ShowLinkParentDialog(List<ParentInfo> allParents)
        {
            using var form = new LinkParentDialog(allParents);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var selectedParent = form.SelectedParent;
                if (selectedParent != null && SelectedStudent != null)
                {
                    try
                    {
                        _dataAccess.LinkStudentToParent(SelectedStudent.Id, selectedParent.Id);
                        ShowSuccess($"Родитель '{selectedParent.FullName}' успешно привязан.");
                        StudentSelected?.Invoke(SelectedStudent);
                    }
                    catch (Exception ex)
                    {
                        ShowError($"Ошибка привязки: {ex.InnerException?.Message ?? ex.Message}");
                    }
                }
            }
        }

        public void ShowStudyPlanDialog()
        {
            using var form = new Form { Text = "Создание нового учебного плана", Width = 400, Height = 220, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
            var nameLabel = new Label { Text = "Название плана*:", Left = 10, Top = 20 };
            var nameText = new TextBox { Left = 150, Top = 20, Width = 220, Text = "УП для Х классов на 2024/25" };
            var yearLabel = new Label { Text = "Учебный год*:", Left = 10, Top = 50 };
            var yearCombo = new ComboBox { Left = 150, Top = 50, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            var parallelLabel = new Label { Text = "Параллель*:", Left = 10, Top = 80 };
            var parallelNumeric = new NumericUpDown { Minimum = 1, Maximum = 11, Left = 150, Top = 80, Width = 220 };
            var okButton = new Button { Text = "Создать", Left = 200, Top = 130, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 130, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { nameLabel, nameText, yearLabel, yearCombo, parallelLabel, parallelNumeric, okButton, cancelButton });
            var years = Task.Run(() => _dataAccess.GetAcademicYears()).Result;
            yearCombo.DataSource = years;
            yearCombo.DisplayMember = "Name";
            yearCombo.ValueMember = "Id";
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(nameText.Text) || yearCombo.SelectedValue == null) { ShowError("Все поля обязательны для заполнения."); return; }
                try
                {
                    Task.Run(() => _dataAccess.AddStudyPlan(nameText.Text, (int)yearCombo.SelectedValue, (int)parallelNumeric.Value)).Wait();
                    ShowSuccess("Учебный план успешно создан."); LoadStudyPlans?.Invoke();
                }
                catch (Exception ex) { ShowError($"Ошибка создания плана: {ex.InnerException?.Message ?? ex.Message}"); }
            }
        }

        public void ShowStudyPlanItemDialog(StudyPlanView selectedPlan, List<DisciplineInfo> allDisciplines)
        {
            using var form = new Form { Text = "Добавить предмет в план", Width = 400, Height = 180, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
            var disciplineLabel = new Label { Text = "Дисциплина:", Left = 10, Top = 20 };
            var disciplineCombo = new ComboBox { Left = 150, Top = 20, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            disciplineCombo.DataSource = allDisciplines; disciplineCombo.DisplayMember = "DisciplineName"; disciplineCombo.ValueMember = "DisciplineId";
            var lessonsLabel = new Label { Text = "Количество занятий:", Left = 10, Top = 50 };
            var lessonsNumeric = new NumericUpDown { Minimum = 1, Maximum = 1000, Left = 150, Top = 50, Width = 220 };
            var okButton = new Button { Text = "Добавить", Left = 200, Top = 90, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 90, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { disciplineLabel, disciplineCombo, lessonsLabel, lessonsNumeric, okButton, cancelButton });
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Task.Run(() => _dataAccess.UpsertStudyPlanItem(selectedPlan.Id, (int)disciplineCombo.SelectedValue, (int)lessonsNumeric.Value)).Wait();
                    ShowSuccess("Предмет успешно добавлен/обновлен в плане."); StudyPlanSelected?.Invoke(selectedPlan);
                }
                catch (Exception ex) { ShowError($"Ошибка сохранения: {ex.InnerException?.Message ?? ex.Message}"); }
            }
        }

        public void ShowAssignTeacherDialog(WorkloadView selectedWorkload, List<TeacherDetails> allTeachers)
        {
            using var form = new Form { Text = $"Назначение учителя на предмет '{selectedWorkload.DisciplineName}'", Width = 400, Height = 180, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
            var teacherLabel = new Label { Text = "Выберите учителя:", Left = 10, Top = 20 };
            var teacherCombo = new ComboBox { Left = 10, Top = 45, Width = 360, DropDownStyle = ComboBoxStyle.DropDownList };
            teacherCombo.DataSource = allTeachers; teacherCombo.DisplayMember = "FullName"; teacherCombo.ValueMember = "Id";
            if (selectedWorkload.TeacherId.HasValue) { teacherCombo.SelectedValue = selectedWorkload.TeacherId.Value; }
            else { teacherCombo.SelectedIndex = -1; }
            var okButton = new Button { Text = "Назначить", Left = 200, Top = 90, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 90, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { teacherLabel, teacherCombo, okButton, cancelButton });
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (teacherCombo.SelectedValue is int selectedTeacherId && SelectedWorkloadAcademicYear != null)
                {
                    try
                    {
                        Task.Run(() => _dataAccess.UpsertWorkload(SelectedWorkloadClass!.ClassId, selectedWorkload.DisciplineId, selectedTeacherId, SelectedWorkloadAcademicYear.Id)).Wait();
                        ShowSuccess("Учитель успешно назначен.");
                        WorkloadFilterChanged?.Invoke(SelectedWorkloadClass.ClassId, SelectedWorkloadAcademicYear.Id);
                    }
                    catch (Exception ex) { ShowError($"Ошибка назначения: {ex.InnerException?.Message ?? ex.Message}"); }
                }
            }
        }
    }
}