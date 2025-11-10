using AISchool.Data;
using AISchool.Models;
using AISchool.Presenters;
using System.Data;
using static AISchool.Data.DataAccess;

namespace AISchool.Views
{
	public partial class TeacherDashboardControl : UserControl, ITeacherDashboardView
	{
		private readonly AppUser _teacher;
		private readonly IDataAccess _dataAccess;
		private ComboBox _classComboBox = null!;
		private ComboBox _disciplineComboBox = null!;
		private DataGridView _journalGrid = null!;
		private DateTime _currentDate = new DateTime(2025, 9, 1);

		public event Action? DisciplineSelected;
		public event Action<List<int>, int, int?, string, DateTime>? BulkGradeActionRequested;
		public event Action<int, int, string, DateTime>? SingleGradeChanged;
		public event Action? LoadClasses;
		public event Action? ClassSelected;
		public event Action<StudentInfo>? StudentSelected;

		public TeacherDashboardControl(AppUser teacher)
		{
			_teacher = teacher;
			_dataAccess = new DataAccess();
			new TeacherDashboardPresenter(this, _dataAccess);
			SetupUI();
			typeof(DataGridView).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty, null, _journalGrid, new object[] { true });
			this.Dock = DockStyle.Fill;
			this.Load += (s, e) => LoadClasses?.Invoke();
		}

		private void SetupUI()
		{
			this.Controls.Clear();
			this.Padding = new Padding(5);
			this.BackColor = SystemColors.Control;

			var topPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5), WrapContents = false };
			var classLabel = new Label { Text = "Класс:", AutoSize = true, Margin = new Padding(0, 6, 5, 0) };
			_classComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150 };
			var disciplineLabel = new Label { Text = "Предмет:", AutoSize = true, Margin = new Padding(15, 6, 5, 0) };
			_disciplineComboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
			topPanel.Controls.AddRange(new Control[] { classLabel, _classComboBox, disciplineLabel, _disciplineComboBox });

			var navigationPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5), WrapContents = false };
			var prevMonthButton = new Button { Text = "<<", Width = 50 };
			var monthLabel = new Label { Name = "monthLabel", Width = 150, TextAlign = ContentAlignment.MiddleCenter, Font = new Font(this.Font, FontStyle.Bold), Margin = new Padding(0, 6, 0, 0) };
			var nextMonthButton = new Button { Text = ">>", Width = 50 };
			var datePickerLabel = new Label { Text = "Перейти к дате:", Margin = new Padding(20, 6, 5, 0) };
			var datePicker = new DateTimePicker { Name = "datePicker", Format = DateTimePickerFormat.Short };
			var toolTip = new ToolTip();
			toolTip.SetToolTip(prevMonthButton, "Предыдущий месяц");
			toolTip.SetToolTip(nextMonthButton, "Следующий месяц");
			navigationPanel.Controls.AddRange(new Control[] { prevMonthButton, monthLabel, nextMonthButton, datePickerLabel, datePicker });

			_journalGrid = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersVisible = false, BackgroundColor = SystemColors.Window, BorderStyle = BorderStyle.None, EditMode = DataGridViewEditMode.EditOnEnter };

			var headerContextMenu = new ContextMenuStrip();
			var editLessonItem = new ToolStripMenuItem("Редактировать урок...");
			var setGradeItem = new ToolStripMenuItem("Выставить оценку всему классу...");
			var setAbsenceItem = new ToolStripMenuItem("Отметить отсутствующих (Н)");
			var clearGradesItem = new ToolStripMenuItem("Очистить оценки за урок");
			editLessonItem.Click += EditLessonItem_Click;
			setGradeItem.Click += SetGradeForAll_Click;
			setAbsenceItem.Click += SetAbsenceForAll_Click;
			clearGradesItem.Click += ClearGradesForAll_Click;
			headerContextMenu.Items.AddRange(new ToolStripItem[] { editLessonItem, new ToolStripSeparator(), setGradeItem, setAbsenceItem, new ToolStripSeparator(), clearGradesItem });

			_journalGrid.ColumnHeaderMouseClick += (s, e) => {
				if (e.Button == MouseButtons.Right)
				{
					if (e.ColumnIndex >= 0 && e.ColumnIndex < _journalGrid.Columns.Count)
					{
						var column = _journalGrid.Columns[e.ColumnIndex];
						if (column.Name.StartsWith("lesson_"))
						{
							_journalGrid.ClearSelection();
							column.Selected = true;
							headerContextMenu.Tag = column;
							headerContextMenu.Show(Cursor.Position);
						}
					}
				}
			};

			this.Controls.Add(_journalGrid);
			this.Controls.Add(navigationPanel);
			this.Controls.Add(topPanel);

			_classComboBox.SelectedIndexChanged += (s, e) => ClassSelected?.Invoke();
			_disciplineComboBox.SelectedIndexChanged += (s, e) => DisciplineSelected?.Invoke();
			_journalGrid.CellEndEdit += JournalGrid_CellEndEdit;
			prevMonthButton.Click += (s, e) => ChangeMonth(-1);
			nextMonthButton.Click += (s, e) => ChangeMonth(1);
			datePicker.ValueChanged += DatePicker_ValueChanged;

			UpdateNavigationUI();
		}

		private void SetGradeForAll_Click(object? sender, EventArgs e)
		{
			if ((sender as ToolStripMenuItem)?.Owner is ContextMenuStrip menu && menu.Tag is DataGridViewColumn column)
			{
				var (grade, workType) = ShowBulkGradeDialog();
				if (grade.HasValue && !string.IsNullOrEmpty(workType))
				{
					var context = GetLessonContextFromColumn(column);
					BulkGradeActionRequested?.Invoke(GetAllStudentIds(), context.lessonId, grade, workType, context.lessonDate);
				}
			}
		}

		private void SetAbsenceForAll_Click(object? sender, EventArgs e)
		{
			if ((sender as ToolStripMenuItem)?.Owner is ContextMenuStrip menu && menu.Tag is DataGridViewColumn column)
			{
				var context = GetLessonContextFromColumn(column);
				BulkGradeActionRequested?.Invoke(GetAllStudentIds(), context.lessonId, null, "Н", context.lessonDate);
			}
		}

		private void ClearGradesForAll_Click(object? sender, EventArgs e)
		{
			if ((sender as ToolStripMenuItem)?.Owner is ContextMenuStrip menu && menu.Tag is DataGridViewColumn column)
			{
				if (MessageBox.Show("Вы уверены, что хотите удалить ВСЕ оценки за этот урок?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
				{
					var context = GetLessonContextFromColumn(column);
					BulkGradeActionRequested?.Invoke(GetAllStudentIds(), context.lessonId, 0, "", context.lessonDate);
				}
			}
		}

		private List<int> GetAllStudentIds()
		{
			var ids = new List<int>();
			foreach (DataGridViewRow row in _journalGrid.Rows)
			{
				ids.Add(Convert.ToInt32(row.Cells["student_id"].Value));
			}
			return ids;
		}

		private (int lessonId, DateTime lessonDate) GetLessonContextFromColumn(DataGridViewColumn column)
		{
			int lessonId = Convert.ToInt32(column.Name.Split('_')[1]);
			var dateString = (column.Tag?.ToString() ?? "|").Split('|')[0];
			DateTime.TryParse(dateString, out var lessonDate);
			return (lessonId, lessonDate == default ? GetCurrentDate() : lessonDate);
		}

		private void JournalGrid_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
		{
			var column = _journalGrid.Columns[e.ColumnIndex];
			if (!column.Name.StartsWith("lesson_")) return;
			int studentId = Convert.ToInt32(_journalGrid.Rows[e.RowIndex].Cells["student_id"].Value);
			int lessonId = Convert.ToInt32(column.Name.Split('_')[1]);
			string newValue = _journalGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
			var context = GetLessonContextFromColumn(column);
			SingleGradeChanged?.Invoke(studentId, lessonId, newValue, context.lessonDate);
		}

		private void ChangeMonth(int monthOffset)
		{
			_currentDate = _currentDate.AddMonths(monthOffset);
			UpdateNavigationUI();
			DisciplineSelected?.Invoke();
		}

		private void GoToDate(DateTime date)
		{
			_currentDate = date;
			UpdateNavigationUI();
			DisciplineSelected?.Invoke();
		}

		private void UpdateNavigationUI()
		{
			var monthLabel = this.Controls.Find("monthLabel", true).FirstOrDefault() as Label;
			if (monthLabel != null)
			{
				monthLabel.Text = _currentDate.ToString("MMMM yyyy");
			}
			var datePicker = this.Controls.Find("datePicker", true).FirstOrDefault() as DateTimePicker;
			if (datePicker != null)
			{
				datePicker.ValueChanged -= DatePicker_ValueChanged;
				datePicker.Value = _currentDate;
				datePicker.ValueChanged += DatePicker_ValueChanged;
			}
		}

		private void DatePicker_ValueChanged(object? sender, EventArgs e)
		{
			if (sender is DateTimePicker picker)
			{
				GoToDate(picker.Value);
			}
		}

		public AppUser CurrentUser => _teacher;
		public IList<ClassInfo> ClassesList
		{
			set
			{
				_classComboBox.DataSource = value;
				_classComboBox.DisplayMember = "Name";
				_classComboBox.ValueMember = "Id";
				if (value.Any()) ClassSelected?.Invoke();
			}
		}
		public ClassInfo? SelectedClass => _classComboBox.SelectedItem as ClassInfo;
		public IList<DisciplineInfo> DisciplinesList
		{
			set
			{
				_disciplineComboBox.DataSource = value;
				_disciplineComboBox.DisplayMember = "DisciplineName";
				_disciplineComboBox.ValueMember = "DisciplineId";
				if (value.Any()) DisciplineSelected?.Invoke();
				else _journalGrid.DataSource = null;
			}
		}
		public DisciplineInfo? SelectedDiscipline => _disciplineComboBox.SelectedItem as DisciplineInfo;

		public void SetJournalGrid(DataTable journalData)
		{
			_journalGrid.SuspendLayout();
			_journalGrid.DataSource = null;
			_journalGrid.Columns.Clear();
			_journalGrid.DataSource = journalData;
			_journalGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			ConfigureGridColumns(journalData);
			_journalGrid.ResumeLayout(false);
			_journalGrid.Refresh();
		}

		private void ConfigureGridColumns(DataTable journalData)
		{
			if (_journalGrid.Columns.Contains("student_id"))
			{
				var col = _journalGrid.Columns["student_id"];
				col.Visible = false;
				col.Frozen = true;
			}
			if (_journalGrid.Columns.Contains("student_name"))
			{
				var col = _journalGrid.Columns["student_name"];
				col.HeaderText = "ФИО Ученика";
				col.ReadOnly = true;
				col.Width = 200;
				col.Frozen = true;
				col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			}
			foreach (DataGridViewColumn col in _journalGrid.Columns)
			{
				if (!col.Name.StartsWith("lesson_")) continue;
				col.Width = 80;
				col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				if (journalData.Columns.Contains(col.DataPropertyName))
				{
					var tagContent = journalData.Columns[col.DataPropertyName].ExtendedProperties["Tag"] as string ?? "|";
					var parts = tagContent.Split('|');
					col.HeaderText = string.IsNullOrEmpty(parts[0]) ? parts[1] : $"{parts[0]}\n{parts[1]}";
					col.ToolTipText = parts.Length > 2 && !string.IsNullOrEmpty(parts[2]) ? parts[2] : "Тема не указана";
				}
			}
		}

		private (int? grade, string? workType) ShowBulkGradeDialog()
		{
			using var form = new Form { Text = "Выставить оценку", Size = new Size(350, 200), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
			var gradeLabel = new Label { Text = "Оценка:", Left = 20, Top = 20 };
			var gradeNumeric = new NumericUpDown { Left = 120, Top = 20, Width = 180, Minimum = 2, Maximum = 5, Value = 4 };
			var workTypeLabel = new Label { Text = "Тип работы:", Left = 20, Top = 55 };
			var workTypeCombo = new ComboBox { Left = 120, Top = 55, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
			workTypeCombo.Items.AddRange(new object[] { "Работа на уроке", "Контрольная работа", "Самостоятельная работа", "Домашнее задание", "Ответ у доски", "Диктант" });
			workTypeCombo.SelectedIndex = 0;
			var okButton = new Button { Text = "Сохранить", Left = 150, Top = 110, DialogResult = DialogResult.OK };
			var cancelButton = new Button { Text = "Отмена", Left = 240, Top = 110, DialogResult = DialogResult.Cancel };
			form.AcceptButton = okButton;
			form.Controls.AddRange(new Control[] { gradeLabel, gradeNumeric, workTypeLabel, workTypeCombo, okButton, cancelButton });
			if (form.ShowDialog(this) == DialogResult.OK)
			{
				return ((int)gradeNumeric.Value, workTypeCombo.SelectedItem?.ToString());
			}
			return (null, null);
		}

		private void EditLessonItem_Click(object? sender, EventArgs e)
		{
			if (!((sender as ToolStripMenuItem)?.Owner is ContextMenuStrip menu && menu.Tag is DataGridViewColumn selectedColumn)) return;
			var context = GetLessonContextFromColumn(selectedColumn);
			var tagParts = (selectedColumn.Tag?.ToString() ?? "||").Split('|');
			string currentTopic = tagParts.Length > 2 ? tagParts[2] : "";
			using var form = new Form { Text = "Редактировать урок", Size = new Size(400, 220), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
			var dateLabel = new Label { Text = "Дата урока:", Left = 20, Top = 20 };
			var lessonDatePicker = new DateTimePicker { Left = 120, Top = 20, Width = 250, Format = DateTimePickerFormat.Short, Value = context.lessonDate == default ? DateTime.Today : context.lessonDate };
			var topicLabel = new Label { Text = "Тема урока:", Left = 20, Top = 55 };
			var topicText = new TextBox { Text = currentTopic, Left = 120, Top = 55, Width = 250, Height = 60, Multiline = true };
			var okButton = new Button { Text = "Сохранить", Left = 200, Top = 130, DialogResult = DialogResult.OK };
			var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 130, DialogResult = DialogResult.Cancel };
			form.Controls.AddRange(new Control[] { dateLabel, lessonDatePicker, topicLabel, topicText, okButton, cancelButton });
			form.AcceptButton = okButton;
			if (form.ShowDialog(this) == DialogResult.OK)
			{
				try
				{
					_dataAccess.UpdateLessonDetails(context.lessonId, lessonDatePicker.Value, topicText.Text);
					ShowSuccess("Данные урока успешно обновлены.");
					DisciplineSelected?.Invoke();
				}
				catch (Exception ex) { ShowError($"Ошибка сохранения: {ex.Message}"); }
			}
		}

		public void ShowError(string message) => MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
		public void ShowSuccess(string message) => MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
		public DateTime GetCurrentDate() => _currentDate;
	}
}