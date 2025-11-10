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
		private TabControl _tabControl = null!;

		private DataGridView _teachersGrid = null!;
		private SortableBindingList<TeacherDetails> _teachersBindingList = null!;

		private DataGridView _classesGrid = null!;
		private SortableBindingList<ClassDetails> _classesBindingList = null!;
		private DataGridView _studentsGrid = null!;
		private SortableBindingList<StudentInfo> _studentsBindingList = null!;
		private TextBox _studentSearchTextBox = null!;

		private ListBox _reportsListBox = null!;
		private Panel _reportsFilterPanel = null!;
		private DataGridView _reportsResultGrid = null!;
		private Button _generateReportButton = null!;
		private ComboBox _reportYearComboBox = null!;
		private DateTimePicker _reportStartDatePicker = null!;
		private DateTimePicker _reportEndDatePicker = null!;
		private SortableBindingList<AcademicPerformanceReportItem> _performanceReportBindingList = null!;
		private SortableBindingList<TeacherWorkloadReportItem> _workloadReportBindingList = null!;

		private DataGridView _studyPlansGrid = null!;
		private SortableBindingList<StudyPlanView> _studyPlansBindingList = null!;
		private DataGridView _studyPlanItemsGrid = null!;
		private SortableBindingList<StudyPlanItem> _studyPlanItemsBindingList = null!;

		private ComboBox _workloadClassComboBox = null!;
		private ComboBox _yearComboBox = null!;
		private DataGridView _workloadGrid = null!;
		private SortableBindingList<WorkloadView> _workloadBindingList = null!;

		private DataGridView _academicYearsGrid = null!;
		private SortableBindingList<AcademicYear> _academicYearsBindingList = null!;

		private List<ClassDetails> _allClassesMaster = new List<ClassDetails>();
		private List<StudentInfo> _currentClassStudentsMaster = new List<StudentInfo>();

		private DataGridView _parentsGrid = null!;
		private SortableBindingList<ParentInfo> _parentsBindingList = null!;
		private Panel _parentsPanel = null!;

		private List<AcademicYear> _academicYearsMaster = new List<AcademicYear>();

		private readonly IDataAccess _dataAccess;

		public AdminDashboardControl(AppUser admin)
		{
			_admin = admin;
			_dataAccess = new DataAccess();
			new AdminDashboardPresenter(this, _dataAccess);

			this.Dock = DockStyle.Fill;
			SetupUI();

			this.Load += (s, e) => {
				LoadTeachers?.Invoke();
				LoadClasses?.Invoke();
				LoadStudyPlans?.Invoke();
			};
		}

		public event Action<StudentInfo?>? StudentSelected;

		public IList<ParentInfo> LinkedParentsList
		{
			set
			{
				_parentsBindingList = new SortableBindingList<ParentInfo>(value.ToList());
				_parentsGrid.DataSource = _parentsBindingList;
				if (_parentsGrid.Columns.Count > 0)
				{
					_parentsGrid.Columns["Id"].Visible = false;
					_parentsGrid.Columns["PasswordHash"].Visible = false;
					_parentsGrid.Columns["PasswordSalt"].Visible = false;
					_parentsGrid.Columns["Email"].Visible = false;
					_parentsGrid.Columns["LastName"].Visible = false;
					_parentsGrid.Columns["FirstName"].Visible = false;
					_parentsGrid.Columns["Patronymic"].Visible = false;
					_parentsGrid.Columns["FullName"].HeaderText = "ФИО Родителя";
					_parentsGrid.Columns["Phone"].HeaderText = "Телефон";
					_parentsGrid.Columns["Login"].HeaderText = "Логин";
				}
			}
		}
		public ParentInfo? SelectedParent => _parentsGrid.CurrentRow?.DataBoundItem as ParentInfo;

		public event Action? AddNewParent;
		public event Action? LinkExistingParent;
		public event Action? EditParent;
		public event Action? UnlinkParent;

		public void ClearStudentSearch() => _studentSearchTextBox.Clear();

		public IList<TeacherDetails> TeachersList
		{
			set
			{
				try
				{
					_teachersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

					_teachersBindingList = new SortableBindingList<TeacherDetails>(value.ToList());
					_teachersGrid.DataSource = _teachersBindingList;

					if (_teachersGrid.Columns.Count > 0)
					{
						_teachersGrid.Columns["Id"].Visible = false;
						_teachersGrid.Columns["PasswordHash"].Visible = false;
						_teachersGrid.Columns["PasswordSalt"].Visible = false;
						_teachersGrid.Columns["FullName"].Visible = false;
						_teachersGrid.Columns["LastName"].HeaderText = "Фамилия";
						_teachersGrid.Columns["FirstName"].HeaderText = "Имя";
						_teachersGrid.Columns["Patronymic"].HeaderText = "Отчество";
						_teachersGrid.Columns["Login"].HeaderText = "Логин";
						_teachersGrid.Columns["Role"].HeaderText = "Роль";
						_teachersGrid.Columns["Phone"].HeaderText = "Телефон";
						_teachersGrid.Columns["Email"].HeaderText = "Email";
						_teachersGrid.Columns["Notes"].HeaderText = "Заметки";
					}
				}
				finally
				{
					_teachersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
				}
			}
		}

		public TeacherDetails? SelectedTeacher => _teachersGrid.CurrentRow?.DataBoundItem as TeacherDetails;
		public AppUser CurrentUser => _admin;

		public IList<ClassDetails> ClassesList
		{
			set
			{
				_allClassesMaster = value.ToList();
				_classesBindingList = new SortableBindingList<ClassDetails>(_allClassesMaster);
				_classesGrid.DataSource = _classesBindingList;
				if (_classesGrid.Columns.Count > 0)
				{
					_classesGrid.Columns["ClassId"].Visible = false;
					_classesGrid.Columns["ParallelNumber"].Visible = false;
					_classesGrid.Columns["HeadTeacherId"].Visible = false;
					_classesGrid.Columns["ClassName"].HeaderText = "Класс";
					_classesGrid.Columns["HeadTeacherFullName"].HeaderText = "Классный руководитель";
				}
			}
		}

		public IList<StudentInfo> StudentsList
		{
			set
			{
				_currentClassStudentsMaster = value.ToList();
				_studentsBindingList = new SortableBindingList<StudentInfo>(_currentClassStudentsMaster);
				_studentsGrid.DataSource = _studentsBindingList;
				_studentSearchTextBox.Clear();
				if (_studentsGrid.Columns.Count > 0)
				{
					_studentsGrid.Columns["Id"].Visible = false;
					_studentsGrid.Columns["FullName"].Visible = false;
					_studentsGrid.Columns["LastName"].HeaderText = "Фамилия";
					_studentsGrid.Columns["FirstName"].HeaderText = "Имя";
					_studentsGrid.Columns["Patronymic"].HeaderText = "Отчество";
				}
			}
		}
		public ClassDetails? SelectedClass => _classesGrid.CurrentRow?.DataBoundItem as ClassDetails;
		public StudentInfo? SelectedStudent => _studentsGrid.CurrentRow?.DataBoundItem as StudentInfo;

		public IList<StudyPlanView> StudyPlansList
		{
			set
			{
				_studyPlansBindingList = new SortableBindingList<StudyPlanView>(value.ToList());
				_studyPlansGrid.DataSource = _studyPlansBindingList;
				if (_studyPlansGrid.Columns.Count > 0)
				{
					_studyPlansGrid.Columns["Id"].Visible = false;
					_studyPlansGrid.Columns["ParallelId"].Visible = false;
					_studyPlansGrid.Columns["AcademicYearId"].Visible = false;
					_studyPlansGrid.Columns["Name"].HeaderText = "Название плана";
					_studyPlansGrid.Columns["AcademicYearName"].HeaderText = "Учебный год";
					_studyPlansGrid.Columns["ParallelNumber"].HeaderText = "Параллель";
				}
			}
		}

		public IList<StudyPlanItem> StudyPlanItemsList
		{
			set
			{
				_studyPlanItemsBindingList = new SortableBindingList<StudyPlanItem>(value.ToList());
				_studyPlanItemsGrid.DataSource = _studyPlanItemsBindingList;
				if (_studyPlanItemsGrid.Columns.Count > 0)
				{
					_studyPlanItemsGrid.Columns["Id"].Visible = false;
					_studyPlanItemsGrid.Columns["DisciplineId"].Visible = false;
					_studyPlanItemsGrid.Columns["DisciplineName"].HeaderText = "Дисциплина";
					_studyPlanItemsGrid.Columns["LessonsCount"].HeaderText = "Кол-во занятий";
					_studyPlanItemsGrid.Columns["AcademicHours"].HeaderText = "Академ. часы";
				}
			}
		}
		public StudyPlanView? SelectedStudyPlan => _studyPlansGrid.CurrentRow?.DataBoundItem as StudyPlanView;
		public StudyPlanItem? SelectedStudyPlanItem => _studyPlanItemsGrid.CurrentRow?.DataBoundItem as StudyPlanItem;

		public IList<AcademicYear> WorkloadAcademicYearsList
		{
			set
			{
				_yearComboBox.DataSource = value;
				_yearComboBox.DisplayMember = "Name";
				_yearComboBox.ValueMember = "Id";

				var currentYear = value.FirstOrDefault(y => y.Status == "Current");
				if (currentYear != null)
				{
					_yearComboBox.SelectedValue = currentYear.Id;
				}
				else if (value.Any())
				{
					_yearComboBox.SelectedIndex = 0;
				}
			}
		}

		public IList<ClassDetails> WorkloadClassesList
		{
			set
			{
				_workloadClassComboBox.DataSource = value;
				_workloadClassComboBox.DisplayMember = "ClassName";
				_workloadClassComboBox.ValueMember = "ClassId";
				_workloadClassComboBox.SelectedIndex = -1;
			}
		}

		public IList<WorkloadView> WorkloadList
		{
			set
			{
				_workloadBindingList = new SortableBindingList<WorkloadView>(value.ToList());
				_workloadGrid.DataSource = _workloadBindingList;

				var assignButton = this.Controls.Find("assignTeacherButton", true).FirstOrDefault() as Button;
				var removeButton = this.Controls.Find("removeTeacherButton", true).FirstOrDefault() as Button;
				bool isPrimary = SelectedWorkloadClass?.ParallelNumber <= 3;
				if (assignButton != null) assignButton.Enabled = !isPrimary;
				if (removeButton != null) removeButton.Enabled = !isPrimary;

				if (_workloadGrid.Columns.Count > 0)
				{
					_workloadGrid.Columns["DisciplineId"].Visible = false;
					_workloadGrid.Columns["TeacherId"].Visible = false;
					_workloadGrid.Columns["DisciplineName"].HeaderText = "Дисциплина";
					_workloadGrid.Columns["LessonsCount"].HeaderText = "Кол-во занятий";
					_workloadGrid.Columns["TeacherFullName"].HeaderText = "Назначенный учитель";
				}
			}
		}

		public WorkloadView? SelectedWorkloadItem => _workloadGrid.CurrentRow?.DataBoundItem as WorkloadView;
		public ClassDetails? SelectedWorkloadClass => _workloadClassComboBox.SelectedItem as ClassDetails;
		public AcademicYear? SelectedWorkloadAcademicYear => _yearComboBox.SelectedItem as AcademicYear;

		public IList<AcademicYear> AcademicYearsList
		{
			set
			{
				_academicYearsMaster = value.ToList();
				_academicYearsBindingList = new SortableBindingList<AcademicYear>(_academicYearsMaster);
				_academicYearsGrid.DataSource = _academicYearsBindingList;
				if (_academicYearsGrid.Columns.Count > 0)
				{
					_academicYearsGrid.Columns["Id"].Visible = false;
					_academicYearsGrid.Columns["Name"].HeaderText = "Учебный год";
					_academicYearsGrid.Columns["StartDate"].HeaderText = "Дата начала";
					_academicYearsGrid.Columns["EndDate"].HeaderText = "Дата окончания";
					_academicYearsGrid.Columns["Status"].HeaderText = "Статус";
				}
			}
		}
		public AcademicYear? SelectedAcademicYear => _academicYearsGrid.CurrentRow?.DataBoundItem as AcademicYear;


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
		public event Action? CreateNewYear;
		public event Action? PromoteStudents;
		public event Action<int, string>? UpdateYearStatus;

		public void ShowError(string message) => MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
		public void ShowSuccess(string message) => MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
		public bool ShowConfirmation(string message) => MessageBox.Show(message, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

		private void SetupUI()
		{
			_tabControl = new TabControl { Dock = DockStyle.Fill };

			var teachersTab = new TabPage("Учителя");
			SetupTeachersTab(teachersTab);
			_tabControl.TabPages.Add(teachersTab);

			var classesTab = new TabPage("Классы и Ученики");
			SetupClassesTab(classesTab);
			_tabControl.TabPages.Add(classesTab);

			var plansTab = new TabPage("Учебные планы");
			SetupStudyPlansTab(plansTab);
			_tabControl.TabPages.Add(plansTab);

			var workloadTab = new TabPage("Нагрузка");
			SetupWorkloadTab(workloadTab);
			_tabControl.TabPages.Add(workloadTab);

			var academicYearTab = new TabPage("Учебный год");
			SetupAcademicYearTab(academicYearTab);
			_tabControl.TabPages.Add(academicYearTab);

			var reportsTab = new TabPage("Отчеты");
			SetupReportsTab(reportsTab);
			_tabControl.TabPages.Add(reportsTab);

			this.Controls.Add(_tabControl);
		}

		private void SetupReportsTab(TabPage tab)
		{
			var mainSplitContainer = new SplitContainer
			{
				Dock = DockStyle.Fill,
				Orientation = Orientation.Vertical,
				FixedPanel = FixedPanel.Panel1,
				SplitterDistance = 220
			};

			var leftPanel = mainSplitContainer.Panel1;
			leftPanel.Padding = new Padding(5);
			var reportsListLabel = new Label { Text = "Выберите отчет:", Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 0, 0, 5) };
			_reportsListBox = new ListBox { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle };
			_reportsListBox.Items.Add("Сводный отчет по успеваемости");
			_reportsListBox.Items.Add("Нагрузка преподавателей");
			_reportsListBox.Items.Add("Движение контингента");
			_reportsListBox.SelectedIndexChanged += ReportsListBox_SelectedIndexChanged;
			leftPanel.Controls.Add(_reportsListBox);
			leftPanel.Controls.Add(reportsListLabel);

			var rightPanel = mainSplitContainer.Panel2;
			rightPanel.Padding = new Padding(5);

			_reportsFilterPanel = new Panel { Dock = DockStyle.Top, Height = 110, Padding = new Padding(5), BorderStyle = BorderStyle.FixedSingle };

			_reportsResultGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				BorderStyle = BorderStyle.None
			};

			rightPanel.Controls.Add(_reportsResultGrid);
			rightPanel.Controls.Add(_reportsFilterPanel);
			tab.Controls.Add(mainSplitContainer);

			if (_reportsListBox.IsHandleCreated && _reportsListBox.Items.Count > 0)
			{
				_reportsListBox.SelectedIndex = 0;
			}
			else if (_reportsListBox.Items.Count > 0)
			{
				this.HandleCreated += (s, ev) => {
					if (_reportsListBox.Items.Count > 0)
						_reportsListBox.SelectedIndex = 0;
				};
			}
		}

		private void ReportsListBox_SelectedIndexChanged(object? sender, EventArgs e)
		{
			_reportsFilterPanel.Controls.Clear();

			var resultPanel = this.Controls.Find("MovementReportPanel", true).FirstOrDefault();
			if (resultPanel != null) resultPanel.Visible = false;
			_reportsResultGrid.Visible = true;
			_reportsResultGrid.DataSource = null;

			string? selectedReport = _reportsListBox.SelectedItem?.ToString();

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
				_reportsResultGrid.Visible = false;
				if (resultPanel != null) resultPanel.Visible = true;
				SetupStudentMovementReportUI();
			}
		}

		private void SetupStudentMovementReportUI()
		{
			var startDateLabel = new Label { Text = "Начало периода:", Left = 10, Top = 10 };
			_reportStartDatePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Left = 120, Top = 10, Width = 120, Value = DateTime.Now.AddMonths(-1) };
			var endDateLabel = new Label { Text = "Конец периода:", Left = 10, Top = 40 };
			_reportEndDatePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Left = 120, Top = 40, Width = 120 };

			_generateReportButton = new Button { Text = "Сформировать", Left = 10, Top = 70, Width = 150 };
			_generateReportButton.Click += GenerateStudentMovementReport_Click;

			_reportsFilterPanel.Controls.Clear();
			_reportsFilterPanel.Controls.AddRange(new Control[] {
		startDateLabel, _reportStartDatePicker,
		endDateLabel, _reportEndDatePicker,
		_generateReportButton
	});

			_reportsResultGrid.Visible = false;

			var resultPanel = new Panel { Name = "MovementReportPanel", Dock = DockStyle.Fill, AutoScroll = true };

			var oldPanel = this.Controls.Find("MovementReportPanel", true).FirstOrDefault();
			oldPanel?.Dispose();

			_reportsResultGrid.Parent.Controls.Add(resultPanel);
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

				var summaryBox = new GroupBox { Text = "Сводка за период", Dock = DockStyle.Top, Height = 80, Font = new Font(this.Font, FontStyle.Bold) };
				var summaryFlowPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10) };
				summaryFlowPanel.Controls.Add(new Label { Text = $"Учащихся на начало периода: {reportData.TotalAtStart}", AutoSize = true, Font = this.Font });
				summaryFlowPanel.Controls.Add(new Label { Text = $"Прибыло: {reportData.ArrivedCount}", AutoSize = true, Margin = new Padding(20, 0, 0, 0), Font = this.Font });
				summaryFlowPanel.Controls.Add(new Label { Text = $"Выбыло: {reportData.DepartedCount}", AutoSize = true, Margin = new Padding(20, 0, 0, 0), Font = this.Font });
				summaryFlowPanel.Controls.Add(new Label { Text = $"Учащихся на конец периода: {reportData.TotalAtEnd}", AutoSize = true, Margin = new Padding(20, 0, 0, 0), Font = this.Font });
				summaryBox.Controls.Add(summaryFlowPanel);

				var listsContainer = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };

				var arrivedBox = new GroupBox { Text = "Прибывшие учащиеся", Dock = DockStyle.Fill };
				var arrivedGrid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, RowHeadersVisible = false };
				arrivedGrid.DataSource = reportData.ArrivedStudents;
				arrivedBox.Controls.Add(arrivedGrid);
				if (arrivedGrid.Columns.Count > 0)
				{
					arrivedGrid.Columns["FullName"].HeaderText = "ФИО";
					arrivedGrid.Columns["EnrollmentDate"].HeaderText = "Дата зачисления";
					arrivedGrid.Columns["ClassName"].HeaderText = "Класс";
					arrivedGrid.Columns["DepartureDate"].Visible = false;
				}

				var departedBox = new GroupBox { Text = "Выбывшие учащиеся", Dock = DockStyle.Fill };
				var departedGrid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, RowHeadersVisible = false };
				departedGrid.DataSource = reportData.DepartedStudents;
				departedBox.Controls.Add(departedGrid);
				if (departedGrid.Columns.Count > 0)
				{
					departedGrid.Columns["FullName"].HeaderText = "ФИО";
					departedGrid.Columns["DepartureDate"].HeaderText = "Дата отчисления";
					departedGrid.Columns["ClassName"].HeaderText = "Класс";
					departedGrid.Columns["EnrollmentDate"].Visible = false;
				}

				listsContainer.Panel1.Controls.Add(arrivedBox);
				listsContainer.Panel2.Controls.Add(departedBox);

				resultPanel.Controls.Add(listsContainer);
				resultPanel.Controls.Add(summaryBox);
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

		private void SetupTeacherWorkloadReportFilters()
		{
			var yearLabel = new Label { Text = "Учебный год:", Left = 10, Top = 10 };
			_reportYearComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Left = 100, Top = 10, Width = 150 };
			_reportYearComboBox.DataSource = _academicYearsMaster;
			_reportYearComboBox.DisplayMember = "Name";
			_reportYearComboBox.ValueMember = "Id";

			_generateReportButton = new Button { Text = "Сформировать", Left = 10, Top = 40, Width = 150 };
			_generateReportButton.Click += GenerateTeacherWorkloadReport_Click;

			_reportsFilterPanel.Controls.AddRange(new Control[] {
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

				_workloadReportBindingList = new SortableBindingList<TeacherWorkloadReportItem>(reportData.ToList());
				_reportsResultGrid.DataSource = _workloadReportBindingList;

				if (_reportsResultGrid.Columns.Count > 0)
				{
					_reportsResultGrid.Columns["TeacherId"].Visible = false;
					_reportsResultGrid.Columns["TeacherFullName"].HeaderText = "ФИО Учителя";
					_reportsResultGrid.Columns["TotalLessonsCount"].HeaderText = "Всего занятий";
					_reportsResultGrid.Columns["WorkloadDetails"].HeaderText = "Детализация нагрузки";

					_reportsResultGrid.Columns["TeacherFullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
					_reportsResultGrid.Columns["TotalLessonsCount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
					_reportsResultGrid.Columns["WorkloadDetails"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
					_reportsResultGrid.Columns["WorkloadDetails"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
					_reportsResultGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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

		private void SetupPerformanceReportFilters()
		{
			var yearLabel = new Label { Text = "Учебный год:", Left = 10, Top = 10 };
			_reportYearComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Left = 100, Top = 10, Width = 150 };
			_reportYearComboBox.DataSource = _academicYearsMaster;
			_reportYearComboBox.DisplayMember = "Name";
			_reportYearComboBox.ValueMember = "Id";

			var startDateLabel = new Label { Text = "Начало периода:", Left = 270, Top = 10 };
			_reportStartDatePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, Left = 380, Top = 10, Width = 120 };
			var endDateLabel = new Label { Text = "Конец периода:", Left = 270, Top = 40 };
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

			_reportsFilterPanel.Controls.AddRange(new Control[] {
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
				_reportsResultGrid.DataSource = _performanceReportBindingList;

				if (_reportsResultGrid.Columns.Count > 0)
				{
					_reportsResultGrid.Columns["ParallelNumber"].HeaderText = "Параллель";
					_reportsResultGrid.Columns["ClassName"].HeaderText = "Класс";
					_reportsResultGrid.Columns["DisciplineName"].HeaderText = "Предмет";
					_reportsResultGrid.Columns["AvgGrade"].HeaderText = "Средний балл";
					_reportsResultGrid.Columns["QualityPercent"].HeaderText = "Качество (%)";
					_reportsResultGrid.Columns["SuccessPercent"].HeaderText = "Успеваемость (%)";
					_reportsResultGrid.Columns["TotalGrades"].HeaderText = "Всего оценок";
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

		private void SetupTeachersTab(TabPage tab)
		{
			var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
			var topPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5) };
			var buttonContainer = new FlowLayoutPanel { Dock = DockStyle.Right, FlowDirection = FlowDirection.RightToLeft, Width = 400 };
			var deleteButton = new Button { Text = "Удалить", Width = 120, Margin = new Padding(3) };
			var editButton = new Button { Text = "Редактировать", Width = 120, Margin = new Padding(3) };
			var addButton = new Button { Text = "Добавить", Width = 120, Margin = new Padding(3) };
			buttonContainer.Controls.AddRange(new Control[] { deleteButton, editButton, addButton });
			addButton.Click += (s, e) => AddTeacher?.Invoke();
			editButton.Click += (s, e) => EditTeacher?.Invoke();
			deleteButton.Click += (s, e) => DeleteTeacher?.Invoke();
			var searchContainer = new FlowLayoutPanel { Dock = DockStyle.Left, WrapContents = false, AutoSize = true, Padding = new Padding(0, 3, 0, 0) };
			var searchLabel = new Label { Text = "Поиск:", AutoSize = true, Margin = new Padding(0, 3, 0, 0) };
			var searchTextBox = new TextBox { Width = 250, Margin = new Padding(5, 0, 0, 0) };
			searchTextBox.TextChanged += (s, e) => SearchTeacher?.Invoke(searchTextBox.Text);
			searchContainer.Controls.AddRange(new Control[] { searchLabel, searchTextBox });
			topPanel.Controls.Add(buttonContainer);
			topPanel.Controls.Add(searchContainer);
			_teachersGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				BorderStyle = BorderStyle.None
			};
			mainPanel.Controls.Add(_teachersGrid);
			mainPanel.Controls.Add(topPanel);
			tab.Controls.Add(mainPanel);
		}

		private void SetupClassesTab(TabPage tab)
		{
			var mainSplitContainer = new SplitContainer
			{
				Dock = DockStyle.Fill,
				Orientation = Orientation.Horizontal,
				BorderStyle = BorderStyle.None
			};
			mainSplitContainer.Panel1.Padding = new Padding(5);
			mainSplitContainer.Panel2.Padding = new Padding(5);

			var classesPanel = new Panel { Dock = DockStyle.Fill };
			var classesTopPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5) };
			var classesButtonContainer = new FlowLayoutPanel { Dock = DockStyle.Right, FlowDirection = FlowDirection.RightToLeft, Width = 250 };
			var editClassButton = new Button { Text = "Назначить", Width = 120, Margin = new Padding(3) };
			var addClassButton = new Button { Text = "Создать класс", Width = 120, Margin = new Padding(3) };
			classesButtonContainer.Controls.AddRange(new Control[] { editClassButton, addClassButton });
			addClassButton.Click += (s, e) => AddClass?.Invoke();
			editClassButton.Click += (s, e) => EditClass?.Invoke();

			var classesSearchContainer = new FlowLayoutPanel { Dock = DockStyle.Left, WrapContents = false, AutoSize = true, Padding = new Padding(0, 3, 0, 0) };
			var classSearchLabel = new Label { Text = "Поиск:", AutoSize = true, Margin = new Padding(0, 3, 0, 0) };
			var classSearchTextBox = new TextBox { Width = 250, Margin = new Padding(5, 0, 0, 0) };
			classSearchTextBox.TextChanged += (s, e) =>
			{
				string searchText = classSearchTextBox.Text.ToLower().Trim();
				var filtered = string.IsNullOrEmpty(searchText)
					? _allClassesMaster
					: _allClassesMaster.Where(c => (c.ClassName?.ToLower() ?? "").Contains(searchText) || (c.HeadTeacherFullName?.ToLower() ?? "").Contains(searchText)).ToList();
				_classesGrid.DataSource = new SortableBindingList<ClassDetails>(filtered);
			};
			classesSearchContainer.Controls.AddRange(new Control[] { classSearchLabel, classSearchTextBox });
			classesTopPanel.Controls.AddRange(new Control[] { classesButtonContainer, classesSearchContainer });

			_classesGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				BorderStyle = BorderStyle.None
			};
			_classesGrid.SelectionChanged += (s, e) => ClassSelected?.Invoke(SelectedClass);

			classesPanel.Controls.Add(_classesGrid);
			classesPanel.Controls.Add(classesTopPanel);
			mainSplitContainer.Panel1.Controls.Add(classesPanel);

			var bottomSplitContainer = new SplitContainer
			{
				Dock = DockStyle.Fill,
				Orientation = Orientation.Horizontal,
				BorderStyle = BorderStyle.None
			};
			bottomSplitContainer.Panel1.Padding = new Padding(0, 0, 5, 0);
			bottomSplitContainer.Panel2.Padding = new Padding(5, 0, 0, 0);
			mainSplitContainer.Panel2.Controls.Add(bottomSplitContainer);

			var studentsPanel = new Panel { Dock = DockStyle.Fill };
			var studentsTopPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5) };
			var studentsButtonContainer = new FlowLayoutPanel { Dock = DockStyle.Right, FlowDirection = FlowDirection.RightToLeft, Width = 550 };
			var transferStudentButton = new Button { Text = "Перевести", Width = 120, Margin = new Padding(3) };
			var deleteStudentButton = new Button { Text = "Отчислить", Width = 120, Margin = new Padding(3) };
			var editStudentButton = new Button { Text = "Редактировать", Width = 120, Margin = new Padding(3) };
			var addStudentButton = new Button { Text = "Добавить ученика", Width = 140, Margin = new Padding(3) };
			studentsButtonContainer.Controls.AddRange(new Control[] { transferStudentButton, deleteStudentButton, editStudentButton, addStudentButton });
			addStudentButton.Click += (s, e) => AddStudent?.Invoke();
			editStudentButton.Click += (s, e) => EditStudent?.Invoke();
			deleteStudentButton.Click += (s, e) => ExpelStudent?.Invoke();
			transferStudentButton.Click += (s, e) => TransferStudent?.Invoke();

			var studentsSearchContainer = new FlowLayoutPanel { Dock = DockStyle.Left, WrapContents = false, AutoSize = true, Padding = new Padding(0, 3, 0, 0) };
			var studentSearchLabel = new Label { Text = "Поиск:", AutoSize = true, Margin = new Padding(0, 3, 0, 0) };
			_studentSearchTextBox = new TextBox { Width = 250, Margin = new Padding(5, 0, 0, 0) };
			_studentSearchTextBox.TextChanged += (s, e) =>
			{
				string searchText = _studentSearchTextBox.Text.ToLower().Trim();
				var filtered = string.IsNullOrEmpty(searchText)
					? _currentClassStudentsMaster
					: _currentClassStudentsMaster.Where(st => (st.FullName?.ToLower() ?? "").Contains(searchText)).ToList();
				_studentsGrid.DataSource = new SortableBindingList<StudentInfo>(filtered);
			};
			studentsSearchContainer.Controls.AddRange(new Control[] { studentSearchLabel, _studentSearchTextBox });
			studentsTopPanel.Controls.AddRange(new Control[] { studentsButtonContainer, studentsSearchContainer });

			_studentsGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				BorderStyle = BorderStyle.None
			};

			studentsPanel.Controls.Add(_studentsGrid);
			studentsPanel.Controls.Add(studentsTopPanel);
			bottomSplitContainer.Panel1.Controls.Add(studentsPanel);

			_parentsPanel = new Panel { Dock = DockStyle.Fill, Enabled = false };

			var parentsButtonPanel = new FlowLayoutPanel
			{
				Dock = DockStyle.Top,
				Height = 40,
				Padding = new Padding(0, 5, 0, 5),
				FlowDirection = FlowDirection.RightToLeft
			};
			var unlinkParentBtn = new Button { Text = "Отвязать", Width = 90, Margin = new Padding(3) };
			var editParentBtn = new Button { Text = "Редактировать", Width = 110, Margin = new Padding(3) };
			var linkParentBtn = new Button { Text = "Привязать", Width = 90, Margin = new Padding(3) };
			var addNewParentBtn = new Button { Text = "Добавить нового", Width = 120, Margin = new Padding(3) };

			addNewParentBtn.Click += (s, e) => AddNewParent?.Invoke();
			linkParentBtn.Click += (s, e) => LinkExistingParent?.Invoke();
			editParentBtn.Click += (s, e) => EditParent?.Invoke();
			unlinkParentBtn.Click += (s, e) => UnlinkParent?.Invoke();
			parentsButtonPanel.Controls.AddRange(new Control[] { unlinkParentBtn, editParentBtn, linkParentBtn, addNewParentBtn });

			var parentsLabel = new Label
			{
				Text = "Родители ученика",
				Font = new Font(this.Font, FontStyle.Bold),
				Dock = DockStyle.Top,
				Padding = new Padding(0, 5, 0, 5),
				AutoSize = true
			};

			_parentsGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				BorderStyle = BorderStyle.None
			};

			_parentsPanel.Controls.Add(_parentsGrid);
			_parentsPanel.Controls.Add(parentsLabel);
			_parentsPanel.Controls.Add(parentsButtonPanel);

			bottomSplitContainer.Panel2.Controls.Add(_parentsPanel);

			_studentsGrid.SelectionChanged += (s, e) =>
			{
				var selectedStudent = SelectedStudent;
				_parentsPanel.Enabled = selectedStudent != null;
				if (selectedStudent != null)
				{
					parentsLabel.Text = $"Родители: {selectedStudent.FullName}";
				}
				else
				{
					parentsLabel.Text = "Родители ученика";
					LinkedParentsList = new List<ParentInfo>();
				}
				StudentSelected?.Invoke(selectedStudent);
			};

			this.Load += async (s, e) => {
				LoadTeachers?.Invoke();
				LoadClasses?.Invoke();
				LoadStudyPlans?.Invoke();
				LoadAcademicYears?.Invoke();
			};

			tab.Controls.Add(mainSplitContainer);
		}

		public void ShowParentDialog(ParentInfo? parent, bool isNew)
		{
			parent ??= new ParentInfo();
			using var form = new Form
			{
				Text = isNew ? "Добавить нового родителя" : "Редактировать данные родителя",
				Width = 450,
				Height = isNew ? 350 : 280,
				StartPosition = FormStartPosition.CenterParent,
				FormBorderStyle = FormBorderStyle.FixedDialog
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

			form.Controls.AddRange(new Control[] { lastNameLabel, lastNameText, firstNameLabel, firstNameText, patronymicLabel, patronymicText, phoneLabel, phoneText, emailLabel, emailText });

			Label? loginLabel = null, passwordLabel = null;
			TextBox? loginText = null, passwordText = null;

			if (isNew && SelectedStudent != null)
			{
				loginLabel = new Label { Text = "Логин*:", Left = 10, Top = 170 };
				var studentLastName = SelectedStudent.LastName.ToLower();
				var studentFirstNameInitial = SelectedStudent.FirstName.ToLower().FirstOrDefault();
				var autoLogin = $"{studentLastName}_{studentFirstNameInitial}_parent";
				loginText = new TextBox { Text = autoLogin, Left = 150, Top = 170, Width = 250 };

				passwordLabel = new Label { Text = "Пароль*:", Left = 10, Top = 200 };
				passwordText = new TextBox { Left = 150, Top = 200, Width = 250, UseSystemPasswordChar = true };

				form.Controls.AddRange(new Control[] { loginLabel, loginText, passwordLabel, passwordText });
			}

			var okButton = new Button { Text = "Сохранить", Left = 260, Top = form.ClientSize.Height - 50, DialogResult = DialogResult.OK };
			var cancelButton = new Button { Text = "Отмена", Left = 350, Top = form.ClientSize.Height - 50, DialogResult = DialogResult.Cancel };
			form.Controls.AddRange(new Control[] { okButton, cancelButton });
			form.AcceptButton = okButton;
			form.CancelButton = cancelButton;

			if (form.ShowDialog() == DialogResult.OK)
			{
				if (string.IsNullOrWhiteSpace(lastNameText.Text) || string.IsNullOrWhiteSpace(firstNameText.Text) ||
					(isNew && (string.IsNullOrWhiteSpace(loginText?.Text) || string.IsNullOrWhiteSpace(passwordText?.Text))))
				{
					ShowError("Поля, отмеченные звездочкой (*), обязательны для заполнения.");
					return;
				}

				parent.LastName = lastNameText.Text;
				parent.FirstName = firstNameText.Text;
				parent.Patronymic = patronymicText.Text;
				parent.Phone = phoneText.Text;
				parent.Email = emailText.Text;

				try
				{
					if (isNew && loginText != null && passwordText != null)
					{
						parent.Login = loginText.Text;
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
			using var form = new Form
			{
				Text = "Привязка существующего родителя",
				Width = 500,
				Height = 400,
				StartPosition = FormStartPosition.CenterParent,
				FormBorderStyle = FormBorderStyle.Sizable
			};

			var searchLabel = new Label { Text = "Поиск по ФИО:", Dock = DockStyle.Top, Padding = new Padding(5) };
			var searchTextBox = new TextBox { Dock = DockStyle.Top, Margin = new Padding(5) };
			var parentsGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				AllowUserToAddRows = false,
				ReadOnly = true,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
			};

			var buttonPanel = new Panel { Dock = DockStyle.Bottom, Height = 40, Padding = new Padding(5) };
			var okButton = new Button { Text = "Привязать", Dock = DockStyle.Right, DialogResult = DialogResult.OK, Enabled = false };
			var cancelButton = new Button { Text = "Отмена", Dock = DockStyle.Right, DialogResult = DialogResult.Cancel };
			buttonPanel.Controls.AddRange(new Control[] { okButton, cancelButton });

			form.Controls.AddRange(new Control[] { parentsGrid, searchLabel, searchTextBox, buttonPanel });

			var parentBindingList = new SortableBindingList<ParentInfo>(allParents);
			parentsGrid.DataSource = parentBindingList;

			if (parentsGrid.Columns.Count > 0)
			{
				parentsGrid.Columns["Id"].Visible = false;
				parentsGrid.Columns["PasswordHash"].Visible = false;
				parentsGrid.Columns["PasswordSalt"].Visible = false;
				parentsGrid.Columns["Email"].Visible = false;
				parentsGrid.Columns["LastName"].Visible = false;
				parentsGrid.Columns["FirstName"].Visible = false;
				parentsGrid.Columns["Patronymic"].Visible = false;
				parentsGrid.Columns["FullName"].HeaderText = "ФИО Родителя";
				parentsGrid.Columns["Phone"].HeaderText = "Телефон";
				parentsGrid.Columns["Login"].HeaderText = "Логин";
			}


			searchTextBox.TextChanged += (s, e) =>
			{
				var searchText = searchTextBox.Text.ToLower().Trim();
				var filteredList = string.IsNullOrWhiteSpace(searchText)
					? allParents
					: allParents.Where(p => p.FullName.ToLower().Contains(searchText)).ToList();
				parentsGrid.DataSource = new SortableBindingList<ParentInfo>(filteredList);
			};

			parentsGrid.SelectionChanged += (s, e) =>
			{
				okButton.Enabled = parentsGrid.SelectedRows.Count > 0;
			};

			if (form.ShowDialog() == DialogResult.OK)
			{
				if (parentsGrid.CurrentRow?.DataBoundItem is ParentInfo selectedParent)
				{
					try
					{
						_dataAccess.LinkStudentToParent(SelectedStudent!.Id, selectedParent.Id);
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

		private void SetupStudyPlansTab(TabPage tab)
		{
			var mainSplitContainer = new SplitContainer
			{
				Dock = DockStyle.Fill,
				Orientation = Orientation.Horizontal,
			};
			mainSplitContainer.Resize += (s, e) => {
				try { if (mainSplitContainer.Height > 0) mainSplitContainer.SplitterDistance = (int)(mainSplitContainer.Height * 0.6); }
				catch { }
			};
			mainSplitContainer.Panel1.Padding = new Padding(5);
			mainSplitContainer.Panel2.Padding = new Padding(5);
			var plansTopPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5) };
			var plansButtonContainer = new FlowLayoutPanel { Dock = DockStyle.Right, FlowDirection = FlowDirection.RightToLeft, AutoSize = true };
			var planAddButton = new Button { Text = "Создать план", Width = 140, Margin = new Padding(3) };
			planAddButton.Click += (s, e) => AddStudyPlan?.Invoke();
			plansButtonContainer.Controls.Add(planAddButton);
			plansTopPanel.Controls.Add(plansButtonContainer);
			var plansLabel = new Label { Text = "Учебные планы", Font = new Font(this.Font, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Top, Padding = new Padding(0, 0, 0, 5) };
			_studyPlansGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				BorderStyle = BorderStyle.None
			};
			_studyPlansGrid.SelectionChanged += (s, e) => StudyPlanSelected?.Invoke(SelectedStudyPlan);
			mainSplitContainer.Panel1.Controls.Add(_studyPlansGrid);
			mainSplitContainer.Panel1.Controls.Add(plansLabel);
			mainSplitContainer.Panel1.Controls.Add(plansTopPanel);
			var itemsTopPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5) };
			var itemsButtonContainer = new FlowLayoutPanel { Dock = DockStyle.Right, FlowDirection = FlowDirection.RightToLeft, Width = 300 }; // Изменено
			var deleteItemButton = new Button { Text = "Удалить предмет", Width = 140, Margin = new Padding(3) };
			var addItemButton = new Button { Text = "Добавить предмет", Width = 140, Margin = new Padding(3) };
			itemsButtonContainer.Controls.AddRange(new Control[] { deleteItemButton, addItemButton });
			addItemButton.Click += (s, e) => AddStudyPlanItem?.Invoke();
			deleteItemButton.Click += (s, e) => DeleteStudyPlanItem?.Invoke();
			itemsTopPanel.Controls.Add(itemsButtonContainer);
			var itemsLabel = new Label { Text = "Предметы плана", Font = new Font(this.Font, FontStyle.Bold), AutoSize = true, Dock = DockStyle.Top, Padding = new Padding(0, 0, 0, 5) };
			_studyPlanItemsGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				BorderStyle = BorderStyle.None
			};

			mainSplitContainer.Panel2.Controls.Add(_studyPlanItemsGrid);
			mainSplitContainer.Panel2.Controls.Add(itemsTopPanel);
			itemsTopPanel.Controls.Add(itemsLabel);
			itemsLabel.Dock = DockStyle.Left;

			mainSplitContainer.Panel2.Controls.Add(_studyPlanItemsGrid);
			mainSplitContainer.Panel2.Controls.Add(itemsTopPanel);
			tab.Controls.Add(mainSplitContainer);
		}

		private void SetupWorkloadTab(TabPage tab)
		{
			var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
			var topPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5) };
			var buttonContainer = new FlowLayoutPanel { Dock = DockStyle.Right, FlowDirection = FlowDirection.RightToLeft, Width = 320 };
			var removeTeacherButton = new Button { Text = "Снять учителя", Width = 150, Name = "removeTeacherButton", Margin = new Padding(3) };
			var assignTeacherButton = new Button { Text = "Назначить/изменить", Width = 150, Name = "assignTeacherButton", Margin = new Padding(3) };
			buttonContainer.Controls.AddRange(new Control[] { removeTeacherButton, assignTeacherButton });
			assignTeacherButton.Click += (s, e) => AssignTeacherToWorkload?.Invoke();
			removeTeacherButton.Click += (s, e) => RemoveTeacherFromWorkload?.Invoke();
			var filterContainer = new FlowLayoutPanel { Dock = DockStyle.Left, WrapContents = false, AutoSize = true, Padding = new Padding(0, 3, 0, 0) };
			var classLabel = new Label { Text = "Выберите класс:", AutoSize = true, Margin = new Padding(0, 3, 0, 0) };
			_workloadClassComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150, Margin = new Padding(5, 0, 0, 0) };
			var yearLabel = new Label { Text = "Учебный год:", AutoSize = true, Margin = new Padding(10, 3, 0, 0) };
			_yearComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120, Name = "yearComboBox", Margin = new Padding(5, 0, 0, 0) };
			filterContainer.Controls.AddRange(new Control[] { classLabel, _workloadClassComboBox, yearLabel, _yearComboBox });
			topPanel.Controls.AddRange(new Control[] { buttonContainer, filterContainer });
			tab.Enter += (s, e) => { if (_yearComboBox.DataSource == null) LoadWorkloadData?.Invoke(); };
			_workloadClassComboBox.SelectedIndexChanged += (s, e) => WorkloadFilterChanged?.Invoke(SelectedWorkloadClass?.ClassId, SelectedWorkloadAcademicYear?.Id);
			_yearComboBox.SelectedIndexChanged += (s, e) => WorkloadFilterChanged?.Invoke(SelectedWorkloadClass?.ClassId, SelectedWorkloadAcademicYear?.Id);
			_workloadGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				BorderStyle = BorderStyle.None
			};
			mainPanel.Controls.Add(_workloadGrid);
			mainPanel.Controls.Add(topPanel);
			tab.Controls.Add(mainPanel);
		}

		private void SetupAcademicYearTab(TabPage tab)
		{
			var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
			var topPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5) };

			var buttonContainer = new FlowLayoutPanel { Dock = DockStyle.Right, FlowDirection = FlowDirection.RightToLeft, Width = 450 };

			var promoteButton = new Button { Text = "Завершить год и перевести учащихся", Width = 250, Margin = new Padding(10, 3, 3, 3) };
			promoteButton.Click += (s, e) => PromoteStudents?.Invoke();

			var createButton = new Button { Text = "Создать новый год", Width = 150, Margin = new Padding(3) };
			createButton.Click += (s, e) => ShowCreateYearDialog();

			buttonContainer.Controls.AddRange(new Control[] { promoteButton, createButton });
			topPanel.Controls.Add(buttonContainer);

			_academicYearsGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				BorderStyle = BorderStyle.None
			};

			var contextMenu = new ContextMenuStrip();
			var setCurrentItem = new ToolStripMenuItem("Сделать текущим");
			setCurrentItem.Click += (s, e) => {
				if (SelectedAcademicYear != null && SelectedAcademicYear.Status != "Current") UpdateYearStatus?.Invoke(SelectedAcademicYear.Id, "Current");
			};
			var archiveItem = new ToolStripMenuItem("Архивировать");
			archiveItem.Click += (s, e) => {
				if (SelectedAcademicYear != null && SelectedAcademicYear.Status != "Archived") UpdateYearStatus?.Invoke(SelectedAcademicYear.Id, "Archived");
			};
			contextMenu.Items.AddRange(new ToolStripItem[] { setCurrentItem, archiveItem });
			_academicYearsGrid.ContextMenuStrip = contextMenu;
			_academicYearsGrid.MouseClick += (s, e) => {
				if (e.Button == MouseButtons.Right)
				{
					int rowIndex = _academicYearsGrid.HitTest(e.X, e.Y).RowIndex;
					if (rowIndex >= 0)
					{
						_academicYearsGrid.ClearSelection();
						_academicYearsGrid.Rows[rowIndex].Selected = true;
					}
				}
			};

			mainPanel.Controls.Add(_academicYearsGrid);
			mainPanel.Controls.Add(topPanel);

			tab.Controls.Add(mainPanel);
		}

		#region Dialog Implementations

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
				Width = 450,
				Height = 420,
				StartPosition = FormStartPosition.CenterParent,
				FormBorderStyle = FormBorderStyle.FixedDialog
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
			roleCombo.Items.AddRange(new object[] { "teacher", "admin" });
			roleCombo.SelectedItem = teacher.Role;
			var notesLabel = new Label { Text = "Заметки:", Left = 10, Top = 260 };
			var notesText = new TextBox { Text = teacher.Notes, Left = 150, Top = 260, Width = 250, Height = 60, Multiline = true, ScrollBars = ScrollBars.Vertical };
			var okButton = new Button { Text = "Сохранить", Left = 260, Top = 330, DialogResult = DialogResult.OK };
			var cancelButton = new Button { Text = "Отмена", Left = 350, Top = 330, DialogResult = DialogResult.Cancel };
			form.Controls.AddRange(new Control[] { lastNameLabel, lastNameText, firstNameLabel, firstNameText, patronymicLabel, patronymicText, phoneLabel, phoneText, emailLabel, emailText, loginLabel, loginText, passwordLabel, passwordText, roleLabel, roleCombo, notesLabel, notesText, okButton, cancelButton });
			form.AcceptButton = okButton; form.CancelButton = cancelButton;
			if (form.ShowDialog() == DialogResult.OK)
			{
				if (string.IsNullOrWhiteSpace(lastNameText.Text) || string.IsNullOrWhiteSpace(firstNameText.Text) || string.IsNullOrWhiteSpace(loginText.Text) || (isNew && string.IsNullOrWhiteSpace(passwordText.Text)))
				{
					ShowError("Поля, отмеченные звездочкой (*), обязательны для заполнения."); return;
				}
				teacher.LastName = lastNameText.Text; teacher.FirstName = firstNameText.Text; teacher.Patronymic = patronymicText.Text; teacher.Phone = phoneText.Text; teacher.Email = emailText.Text; teacher.Login = loginText.Text;
				if (isNew) { var (hash, salt) = PasswordHasher.HashPassword(passwordText.Text); teacher.PasswordHash = hash; teacher.PasswordSalt = salt; }
				teacher.Role = roleCombo.SelectedItem?.ToString() ?? "teacher"; teacher.Notes = notesText.Text;
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
			using var form = new Form { Text = "Создание нового класса", Width = 400, Height = 220, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
			var parallelLabel = new Label { Text = "Параллель (цифра):", Left = 10, Top = 20 };
			var parallelNumeric = new NumericUpDown { Minimum = 1, Maximum = 11, Left = 180, Top = 20, Width = 190 };
			var letterLabel = new Label { Text = "Буква:", Left = 10, Top = 50 };
			var letterText = new TextBox { MaxLength = 1, Left = 180, Top = 50, Width = 190 };
			var teacherLabel = new Label { Text = "Классный руководитель:", Left = 10, Top = 80 };
			var teacherCombo = new ComboBox { Left = 180, Top = 80, Width = 190, DropDownStyle = ComboBoxStyle.DropDownList };
			var teacherListForCombo = new List<TeacherDetails> { new TeacherDetails { Id = 0, LastName = "(Без руководителя)" } };
			teacherListForCombo.AddRange(allTeachers);
			teacherCombo.DataSource = teacherListForCombo; teacherCombo.DisplayMember = "FullName"; teacherCombo.ValueMember = "Id";
			var okButton = new Button { Text = "Создать", Left = 200, Top = 130, DialogResult = DialogResult.OK };
			var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 130, DialogResult = DialogResult.Cancel };
			form.Controls.AddRange(new Control[] { parallelLabel, parallelNumeric, letterLabel, letterText, teacherLabel, teacherCombo, okButton, cancelButton });
			if (form.ShowDialog() == DialogResult.OK)
			{
				if (string.IsNullOrWhiteSpace(letterText.Text)) { ShowError("Необходимо указать букву класса."); return; }
				try
				{
					var letter = letterText.Text.ToUpper(); var parallel = (int)parallelNumeric.Value;
					int? teacherId = teacherCombo.SelectedValue is int selectedId && selectedId > 0 ? selectedId : null;
					Task.Run(() => _dataAccess.AddClass(letter, parallel, teacherId)).Wait();
					ShowSuccess("Класс успешно создан."); LoadClasses?.Invoke();
				}
				catch (Exception ex) { ShowError($"Ошибка создания класса: {ex.InnerException?.Message ?? ex.Message}"); }
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
			using var form = new Form { Text = isNew ? "Добавить нового ученика" : "Редактировать данные ученика", Width = 450, Height = 320, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
			var lastNameLabel = new Label { Text = "Фамилия*:", Left = 10, Top = 20 }; var lastNameText = new TextBox { Text = student.LastName, Left = 150, Top = 20, Width = 250 };
			var firstNameLabel = new Label { Text = "Имя*:", Left = 10, Top = 50 }; var firstNameText = new TextBox { Text = student.FirstName, Left = 150, Top = 50, Width = 250 };
			var patronymicLabel = new Label { Text = "Отчество:", Left = 10, Top = 80 }; var patronymicText = new TextBox { Text = student.Patronymic, Left = 150, Top = 80, Width = 250 };
			var birthDateLabel = new Label { Text = "Дата рождения*:", Left = 10, Top = 110 }; var birthDatePicker = new DateTimePicker { Value = student.BirthDate, Left = 150, Top = 110, Width = 250, Format = DateTimePickerFormat.Short };
			var classLabel = new Label { Text = "Класс*:", Left = 10, Top = 140 }; var classCombo = new ComboBox { Left = 150, Top = 140, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
			classCombo.DataSource = allClasses; classCombo.DisplayMember = "ClassName"; classCombo.ValueMember = "ClassId"; classCombo.SelectedValue = student.ClassId;
			var notesLabel = new Label { Text = "Заметки:", Left = 10, Top = 170 }; var notesText = new TextBox { Text = student.Notes, Left = 150, Top = 170, Width = 250, Height = 60, Multiline = true, ScrollBars = ScrollBars.Vertical };
			var okButton = new Button { Text = "Сохранить", Left = 260, Top = 240, DialogResult = DialogResult.OK }; var cancelButton = new Button { Text = "Отмена", Left = 350, Top = 240, DialogResult = DialogResult.Cancel };
			form.Controls.AddRange(new Control[] { lastNameLabel, lastNameText, firstNameLabel, firstNameText, patronymicLabel, patronymicText, birthDateLabel, birthDatePicker, classLabel, classCombo, notesLabel, notesText, okButton, cancelButton });
			if (form.ShowDialog() == DialogResult.OK)
			{
				if (string.IsNullOrWhiteSpace(lastNameText.Text) || string.IsNullOrWhiteSpace(firstNameText.Text) || classCombo.SelectedValue == null) { ShowError("Поля, отмеченные звездочкой (*), обязательны для заполнения."); return; }
				student.LastName = lastNameText.Text; student.FirstName = firstNameText.Text; student.Patronymic = patronymicText.Text; student.BirthDate = birthDatePicker.Value; student.ClassId = (int)classCombo.SelectedValue; student.Notes = notesText.Text;
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
						Task.Run(() => _dataAccess.UpsertWorkload(SelectedWorkloadClass.ClassId, selectedWorkload.DisciplineId, selectedTeacherId, SelectedWorkloadAcademicYear.Id)).Wait();
						ShowSuccess("Учитель успешно назначен.");
						WorkloadFilterChanged?.Invoke(SelectedWorkloadClass.ClassId, SelectedWorkloadAcademicYear.Id);
					}
					catch (Exception ex) { ShowError($"Ошибка назначения: {ex.InnerException?.Message ?? ex.Message}"); }
				}
			}
		}
		#endregion
	}
}