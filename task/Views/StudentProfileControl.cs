using AISchool.Data;
using AISchool.Models;
using AISchool.Presenters;
using AISchool.Utils;
using static AISchool.Data.DataAccess;

namespace AISchool.Views
{
	public partial class StudentProfileControl : UserControl, IStudentProfileView
	{
		private readonly IDataAccess _dataAccess;
		private readonly StudentProfilePresenter _presenter;

		public event Action? GoBackRequested;

		private TabControl _tabControl = null!;
		private Label _studentNameLabel = null!;
		private ComboBox _classmatesComboBox = null!;
		private ComboBox _disciplinesListBox = null!;

		private DataGridView _lessonsGrid = null!;
		private DataGridView _statsGrid = null!;
		private DataGridView _achievementsGrid = null!;

		private TextBox _lessonsSearchBox = null!;
		private TextBox _statsSearchBox = null!;
		private TextBox _achievementsSearchBox = null!;

		private bool _isComboBoxLoading = false;

		private List<LessonInfo> _lessonsMasterList = new List<LessonInfo>();
		private List<AverageGrade> _statsMasterList = new List<AverageGrade>();
		private List<Achievement> _achievementsMasterList = new List<Achievement>();

		public StudentProfileControl(int studentId, string userRole, int currentUserId)
		{
			UserRole = userRole;
			CurrentUserId = currentUserId;
			StudentId = studentId;

			_dataAccess = new DataAccess();
			_presenter = new StudentProfilePresenter(this, _dataAccess);

			SetupUI();
			this.Dock = DockStyle.Fill;
			LoadStudentData?.Invoke(StudentId);
		}

		#region IStudentProfileView Implementation
		public string UserRole { get; }
		public int CurrentUserId { get; }
		public int StudentId { get; private set; }

		public event Action<int>? LoadStudentData;

		public void InvokeGoBack() => GoBackRequested?.Invoke();

		public void SetStudentProfile(StudentProfile profile)
		{
			StudentId = profile.Id;
			_studentNameLabel.Text = profile.FullName;
			var classNameLabel = this.Controls.Find("ClassNameLabel", true).FirstOrDefault() as Label;
			if (classNameLabel != null) classNameLabel.Text = profile.ClassName;
			var birthDateLabel = this.Controls.Find("BirthDateLabel", true).FirstOrDefault() as Label;
			if (birthDateLabel != null) birthDateLabel.Text = profile.BirthDate.ToShortDateString();
			var notesLabel = this.Controls.Find("NotesLabel", true).FirstOrDefault() as Label;
			if (notesLabel != null)
			{
				notesLabel.Text = string.IsNullOrWhiteSpace(profile.Notes) ? "" : $"Заметки: {profile.Notes}";
				notesLabel.Visible = !string.IsNullOrWhiteSpace(profile.Notes);
			}
		}

		public void SetClassmates(IList<StudentInfo> classmates)
		{
			_classmatesComboBox.DataSource = classmates;
			_classmatesComboBox.DisplayMember = "FullName";
			_classmatesComboBox.ValueMember = "Id";
			_isComboBoxLoading = true;
			_classmatesComboBox.SelectedValue = StudentId;
			_isComboBoxLoading = false;
		}

		public void SetDisciplines(IList<DisciplineInfo> disciplines)
		{
			_disciplinesListBox.DataSource = disciplines;
			_disciplinesListBox.DisplayMember = "DisciplineName";
			_disciplinesListBox.ValueMember = "DisciplineId";
			if (!disciplines.Any())
			{
				_lessonsGrid.DataSource = null;
			}
		}

		public void SetLessons(IList<LessonInfo> lessons)
		{
			_lessonsMasterList = lessons.ToList(); // Добавить эту строку
			var lessonsBindingList = new SortableBindingList<LessonInfo>(_lessonsMasterList);
			_lessonsGrid.DataSource = lessonsBindingList;
			SetupLessonsGridColumns();
		}

		public void SetStats(IList<AverageGrade> stats)
		{
			_statsMasterList = stats.ToList(); // Добавить эту строку
			var statsBindingList = new SortableBindingList<AverageGrade>(_statsMasterList);
			_statsGrid.DataSource = statsBindingList;
			if (_statsGrid.Columns.Count > 0)
			{
				_statsGrid.Columns["DisciplineName"].HeaderText = "Дисциплина";
				_statsGrid.Columns["AverageGradeValue"].HeaderText = "Средний балл";
			}
		}

		public void SetAchievements(IList<Achievement> achievements)
		{
			_achievementsMasterList = achievements.ToList(); // Добавить эту строку
			var achievementsBindingList = new SortableBindingList<Achievement>(_achievementsMasterList);
			_achievementsGrid.DataSource = achievementsBindingList;
			if (_achievementsGrid.Columns.Count > 0)
			{
				_achievementsGrid.Columns["Id"].Visible = false;
				_achievementsGrid.Columns["EventDate"].HeaderText = "Дата";
				_achievementsGrid.Columns["EventName"].HeaderText = "Мероприятие";
				_achievementsGrid.Columns["Level"].HeaderText = "Уровень";
				_achievementsGrid.Columns["Place"].HeaderText = "Место";
			}
		}

		public void ShowError(string message)
		{
			MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		#endregion

		private void SetupUI()
		{
			this.Controls.Clear();
			var headerPanel = new Panel { Dock = DockStyle.Top, Height = 130, Padding = new Padding(10), BorderStyle = BorderStyle.FixedSingle };

			var topFlowPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, WrapContents = false };
			_studentNameLabel = new Label { Font = new Font(this.Font.FontFamily, 14, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 5, 10, 0) };

			_classmatesComboBox = new ComboBox
			{
				Width = 250,
				DropDownStyle = ComboBoxStyle.DropDownList,
				Margin = new Padding(0, 8, 0, 0)
			};
			_classmatesComboBox.SelectedIndexChanged += (s, e) =>
			{
				if (_isComboBoxLoading || _classmatesComboBox.SelectedValue == null) return;
				if (_classmatesComboBox.SelectedValue is int newStudentId && newStudentId != StudentId)
				{
					LoadStudentData?.Invoke(newStudentId);
				}
			};

			_classmatesComboBox.Visible = (UserRole == "teacher" || UserRole == "admin");

			topFlowPanel.Controls.Add(_studentNameLabel);
			topFlowPanel.Controls.Add(_classmatesComboBox);

			var detailsTable = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(0, 5, 0, 0) };
			detailsTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			detailsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			detailsTable.Controls.Add(new Label { Text = "Класс:", Font = new Font(this.Font, FontStyle.Bold), AutoSize = true }, 0, 0);
			detailsTable.Controls.Add(new Label { Name = "ClassNameLabel", AutoSize = true }, 1, 0);
			detailsTable.Controls.Add(new Label { Text = "Дата рождения:", Font = new Font(this.Font, FontStyle.Bold), AutoSize = true }, 0, 1);
			detailsTable.Controls.Add(new Label { Name = "BirthDateLabel", AutoSize = true }, 1, 1);

			var notesLabel = new Label { Name = "NotesLabel", Dock = DockStyle.Top, AutoSize = true, ForeColor = Color.DarkSlateGray, Padding = new Padding(0, 5, 0, 0) };

			headerPanel.Controls.Add(detailsTable);
			headerPanel.Controls.Add(notesLabel);
			headerPanel.Controls.Add(topFlowPanel);

			_tabControl = new TabControl { Dock = DockStyle.Fill };
			var gradesTab = new TabPage("Успеваемость");
			SetupGradesTab(gradesTab);
			_tabControl.TabPages.Add(gradesTab);
			var statsTab = new TabPage("Статистика");
			SetupStatsTab(statsTab);
			_tabControl.TabPages.Add(statsTab);
			var achievementsTab = new TabPage("Достижения");
			SetupAchievementsTab(achievementsTab);
			_tabControl.TabPages.Add(achievementsTab);
			this.Controls.Add(_tabControl);
			this.Controls.Add(headerPanel);
		}

		private void SetupGradesTab(TabPage tab)
		{
			var topPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(5), WrapContents = false };
			var disciplinesLabel = new Label { Text = "Предмет:", AutoSize = true, Margin = new Padding(0, 6, 0, 0) };
			_disciplinesListBox = new ComboBox { Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
			_disciplinesListBox.SelectedIndexChanged += (s, e) =>
			{
				if (_disciplinesListBox.SelectedValue is int disciplineId)
				{
					_presenter.LoadLessonsForDiscipline(disciplineId);
				}
			};
			var searchLabel = new Label { Text = "Поиск по теме:", AutoSize = true, Margin = new Padding(10, 6, 0, 0) };
			_lessonsSearchBox = new TextBox { Width = 200 };
			_lessonsSearchBox.TextChanged += (s, e) => {
				string searchText = _lessonsSearchBox.Text.ToLower().Trim();
				var filtered = string.IsNullOrEmpty(searchText)
					? _lessonsMasterList
					: _lessonsMasterList.Where(l => (l.LessonTopic?.ToLower() ?? "").Contains(searchText)).ToList();

				_lessonsGrid.DataSource = new SortableBindingList<LessonInfo>(filtered);
				SetupLessonsGridColumns();
			};
			topPanel.Controls.AddRange(new Control[] { disciplinesLabel, _disciplinesListBox, searchLabel, _lessonsSearchBox });

			var bottomLabel = new Label
			{
				Dock = DockStyle.Bottom,
				Height = 30,
				TextAlign = ContentAlignment.MiddleCenter,
				ForeColor = SystemColors.GrayText,
			};

			if (UserRole == "teacher" || UserRole == "admin")
			{
				bottomLabel.Text = "Двойной клик на оценках для их редактирования. Клик по заголовку столбца для сортировки.";
			}
			else
			{
				bottomLabel.Text = "Клик по заголовку столбца для сортировки.";
			}

			_lessonsGrid = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.CellSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None, AllowUserToResizeRows = false, BorderStyle = BorderStyle.None };
			_lessonsGrid.CellDoubleClick += LessonsGrid_CellDoubleClick;
			_lessonsGrid.CellEndEdit += LessonsGrid_CellEndEdit;

			tab.Controls.Add(_lessonsGrid);
			tab.Controls.Add(topPanel);
			tab.Controls.Add(bottomLabel);
		}

		private void SetupStatsTab(TabPage tab)
		{
			var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
			var searchPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 30, Padding = new Padding(0, 0, 0, 5) };
			var searchLabel = new Label { Text = "Поиск по дисциплине:", AutoSize = true, Margin = new Padding(0, 3, 0, 0) };
			_statsSearchBox = new TextBox { Width = 250 };
			_statsSearchBox.TextChanged += (s, e) => {
				string searchText = _statsSearchBox.Text.ToLower().Trim();
				var filtered = string.IsNullOrEmpty(searchText)
					? _statsMasterList
					: _statsMasterList.Where(item => item.DisciplineName.ToLower().Contains(searchText)).ToList();

				_statsGrid.DataSource = new SortableBindingList<AverageGrade>(filtered);
			};
			searchPanel.Controls.AddRange(new Control[] { searchLabel, _statsSearchBox });
			_statsGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				RowHeadersVisible = false,
				BorderStyle = BorderStyle.None
			};
			panel.Controls.Add(_statsGrid);
			panel.Controls.Add(searchPanel);
			tab.Controls.Add(panel);
		}

		private void SetupAchievementsTab(TabPage tab)
		{
			var topPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5) };

			var buttonContainer = new FlowLayoutPanel { Dock = DockStyle.Right, FlowDirection = FlowDirection.RightToLeft, AutoSize = true };
			var addButton = new Button { Text = "Добавить достижение", Width = 150, Margin = new Padding(3) };
			addButton.Click += AddAchievement_Click;
			buttonContainer.Controls.Add(addButton);

			buttonContainer.Visible = (UserRole == "teacher" || UserRole == "admin");

			var searchPanel = new FlowLayoutPanel { Dock = DockStyle.Left, WrapContents = false, AutoSize = true, Padding = new Padding(5, 3, 0, 0) };
			var searchLabel = new Label { Text = "Поиск по названию:", AutoSize = true, Margin = new Padding(0, 3, 0, 0) };
			_achievementsSearchBox = new TextBox { Width = 250 };
			_achievementsSearchBox.TextChanged += (s, e) => {
				string searchText = _achievementsSearchBox.Text.ToLower().Trim();
				var filtered = string.IsNullOrEmpty(searchText)
					? _achievementsMasterList
					: _achievementsMasterList.Where(item => item.EventName.ToLower().Contains(searchText)).ToList();

				_achievementsGrid.DataSource = new SortableBindingList<Achievement>(filtered);
			};
			searchPanel.Controls.AddRange(new Control[] { searchLabel, _achievementsSearchBox });

			topPanel.Controls.Add(buttonContainer);
			topPanel.Controls.Add(searchPanel);

			_achievementsGrid = new DataGridView
			{
				Dock = DockStyle.Fill,
				ReadOnly = true,
				AllowUserToAddRows = false,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				RowHeadersVisible = false,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				BorderStyle = BorderStyle.None
			};
			if (UserRole == "teacher" || UserRole == "admin")
			{
				var contextMenu = new ContextMenuStrip();
				var editItem = new ToolStripMenuItem("Редактировать достижение");
				var deleteItem = new ToolStripMenuItem("Удалить достижение");
				editItem.Click += EditAchievement_Click;
				deleteItem.Click += DeleteAchievement_Click;
				contextMenu.Items.AddRange(new ToolStripItem[] { editItem, deleteItem });
				_achievementsGrid.ContextMenuStrip = contextMenu;
			}

			tab.Padding = new Padding(5);
			tab.Controls.Add(_achievementsGrid);
			tab.Controls.Add(topPanel);
		}
		private void SetupLessonsGridColumns()
		{
			if (_lessonsGrid.Columns.Count > 0)
			{
				_lessonsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
				_lessonsGrid.Columns["LessonId"].Visible = false;
				_lessonsGrid.Columns["LessonNumber"].HeaderText = "Занятие №";
				_lessonsGrid.Columns["LessonDate"].HeaderText = "Дата проведения";
				_lessonsGrid.Columns["LessonTopic"].HeaderText = "Тема";
				_lessonsGrid.Columns["GradesLine"].HeaderText = "Оценки";
				_lessonsGrid.Columns["LessonNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				_lessonsGrid.Columns["LessonDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				_lessonsGrid.Columns["LessonTopic"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				_lessonsGrid.Columns["GradesLine"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				_lessonsGrid.Columns["LessonTopic"].MinimumWidth = 200;
				_lessonsGrid.Columns["GradesLine"].MinimumWidth = 150;
				bool isEditable = (UserRole == "teacher" || UserRole == "admin");
				_lessonsGrid.Columns["LessonNumber"].ReadOnly = true;
				_lessonsGrid.Columns["GradesLine"].ReadOnly = true;
				_lessonsGrid.Columns["LessonDate"].ReadOnly = !isEditable;
				_lessonsGrid.Columns["LessonTopic"].ReadOnly = !isEditable;
			}
		}

		private async void LessonsGrid_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
		{
			if (_lessonsGrid.Rows[e.RowIndex].DataBoundItem is not LessonInfo editedLesson) return;
			if (_disciplinesListBox.SelectedValue is not int selectedDisciplineId) return;

			var lessonIdForDb = editedLesson.LessonId;

			if (lessonIdForDb <= 0)
			{
				lessonIdForDb = await CreateLessonAndGetId(editedLesson, selectedDisciplineId);
				if (lessonIdForDb <= 0) { _lessonsGrid.CancelEdit(); return; }
				_presenter.LoadLessonsForDiscipline(selectedDisciplineId);
				return;
			}

			var newDateStr = _lessonsGrid.Rows[e.RowIndex].Cells["LessonDate"].Value?.ToString();
			DateTime newDate = DateTime.TryParse(newDateStr, out var dt) ? dt : (editedLesson.LessonDate ?? DateTime.Today);
			string newTopic = _lessonsGrid.Rows[e.RowIndex].Cells["LessonTopic"].Value?.ToString() ?? "";

			try
			{
				await Task.Run(() => _dataAccess.UpdateLessonDetails(lessonIdForDb, newDate, newTopic));
			}
			catch (Exception ex)
			{
				ShowError($"Ошибка сохранения: {ex.InnerException?.Message ?? ex.Message}");
				_lessonsGrid.CancelEdit();
			}
		}

		private async void LessonsGrid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || !(UserRole == "teacher" || UserRole == "admin")) return;
			if (_lessonsGrid.Rows[e.RowIndex].DataBoundItem is not LessonInfo selectedLesson) return;
			if (_disciplinesListBox.SelectedValue is not int selectedDisciplineId) return;

			if (_lessonsGrid.Columns[e.ColumnIndex].Name == "GradesLine")
			{
				var lessonIdForDb = selectedLesson.LessonId;

				if (lessonIdForDb <= 0)
				{
					lessonIdForDb = await CreateLessonAndGetId(selectedLesson, selectedDisciplineId);
					if (lessonIdForDb <= 0) return;
				}

				string lessonInfo = $"Урок №{selectedLesson.LessonNumber}" + (selectedLesson.LessonDate.HasValue ? $" от {selectedLesson.LessonDate.Value.ToShortDateString()}" : "");

				using var dialog = new EditStudentGradesDialog(lessonIdForDb, StudentId, selectedDisciplineId, (_lessonsGrid.DataSource as SortableBindingList<LessonInfo>)?.FirstOrDefault()?.GradesLine ?? "", lessonInfo);
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					_presenter.LoadLessonsForDiscipline(selectedDisciplineId);
				}
			}
		}

		private async Task<int> CreateLessonAndGetId(LessonInfo lesson, int disciplineId)
		{
			try
			{
				await Task.Run(() => _dataAccess.AddGradeToLesson(StudentId, lesson.LessonId, disciplineId, null, DateTime.Today, "sys_create"));

				var updatedLessons = await Task.Run(() => _dataAccess.GetStudentLessonsAndGrades(StudentId, disciplineId));
				var createdLesson = updatedLessons.FirstOrDefault(l => l.LessonNumber == lesson.LessonNumber);

				return createdLesson?.LessonId ?? -1;
			}
			catch (Exception ex)
			{
				ShowError($"Не удалось создать занятие: {ex.InnerException?.Message ?? ex.Message}");
				return -1;
			}
		}

		private void ShowAchievementDialog(Achievement? achievement)
		{
			bool isNew = achievement == null;
			achievement ??= new Achievement();

			using var form = new Form { Text = isNew ? "Добавить достижение" : "Редактировать достижение", Width = 400, Height = 250, StartPosition = FormStartPosition.CenterParent };
			var eventNameLabel = new Label { Text = "Название:", Left = 10, Top = 20 };
			var eventNameText = new TextBox { Text = achievement.EventName, Left = 120, Top = 20, Width = 250 };
			var eventDateLabel = new Label { Text = "Дата:", Left = 10, Top = 50 };
			var eventDatePicker = new DateTimePicker { Value = achievement.EventDate ?? DateTime.Now, Left = 120, Top = 50 };
			var levelLabel = new Label { Text = "Уровень:", Left = 10, Top = 80 };
			var levelText = new TextBox { Text = achievement.Level, Left = 120, Top = 80, Width = 250 };
			var placeLabel = new Label { Text = "Место:", Left = 10, Top = 110 };
			var placeNumeric = new NumericUpDown { Value = achievement.Place ?? 0, Left = 120, Top = 110, Maximum = 1000 };
			var okButton = new Button { Text = "OK", Left = 200, Top = 150, DialogResult = DialogResult.OK };
			var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 150, DialogResult = DialogResult.Cancel };
			form.Controls.AddRange(new Control[] { eventNameLabel, eventNameText, eventDateLabel, eventDatePicker, levelLabel, levelText, placeLabel, placeNumeric, okButton, cancelButton });
			form.AcceptButton = okButton; form.CancelButton = cancelButton;

			if (form.ShowDialog() == DialogResult.OK)
			{
				try
				{
					achievement.EventName = eventNameText.Text;
					achievement.EventDate = eventDatePicker.Value;
					achievement.Level = string.IsNullOrWhiteSpace(levelText.Text) ? null : levelText.Text;
					achievement.Place = placeNumeric.Value > 0 ? (int?)placeNumeric.Value : null;

					if (isNew)
					{
						Task.Run(() => _dataAccess.AddAchievement(StudentId, achievement)).Wait();
					}
					else
					{
						Task.Run(() => _dataAccess.UpdateAchievement(achievement)).Wait();
					}
					LoadStudentData?.Invoke(StudentId);
				}
				catch (Exception ex)
				{
					ShowError($"Ошибка сохранения: {ex.Message}");
				}
			}
		}

		private void AddAchievement_Click(object? sender, EventArgs e) => ShowAchievementDialog(null);

		private void EditAchievement_Click(object? sender, EventArgs e)
		{
			if (_achievementsGrid.CurrentRow?.DataBoundItem is Achievement selectedAchievement)
			{
				ShowAchievementDialog(selectedAchievement);
			}
		}

		private async void DeleteAchievement_Click(object? sender, EventArgs e)
		{
			if (_achievementsGrid.CurrentRow?.DataBoundItem is Achievement selectedAchievement)
			{
				if (MessageBox.Show("Вы уверены, что хотите удалить это достижение?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
				{
					try
					{
						await Task.Run(() => _dataAccess.DeleteAchievement(selectedAchievement.Id));
						LoadStudentData?.Invoke(StudentId);
					}
					catch (Exception ex)
					{
						ShowError($"Ошибка удаления: {ex.Message}");
					}
				}
			}
		}
	}
}