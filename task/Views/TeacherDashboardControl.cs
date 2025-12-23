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
        private DateTime _currentDate = new DateTime(2024, 9, 1);

        private bool _isLoading = false;

        public event Action? DisciplineSelected;
        public event Action<List<int>, int, int?, string, DateTime>? BulkGradeActionRequested;
        public event Action<int, int, string, DateTime>? SingleGradeChanged;
        public event Action? LoadClasses;
        public event Action? ClassSelected;
        public event Action<StudentInfo>? StudentSelected;

        public TeacherDashboardControl(AppUser teacher)
        {
            InitializeComponent();
            _teacher = teacher;
            _dataAccess = new DataAccess();
            new TeacherDashboardPresenter(this, _dataAccess);

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, journalGrid, new object[] { true });

            this.Dock = DockStyle.Fill;

            SubscribeEvents();
            UpdateNavigationUI();

            this.Load += (s, e) => LoadClasses?.Invoke();
        }

        private void SubscribeEvents()
        {
            classComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (_isLoading) return;
                ClassSelected?.Invoke();
            };

            disciplineComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (_isLoading) return;
                DisciplineSelected?.Invoke();
            };

            journalGrid.CellEndEdit += JournalGrid_CellEndEdit;
            journalGrid.CellDoubleClick += JournalGrid_CellDoubleClick;

            btnPrevMonth.Click += (s, e) => ChangeMonth(-1);
            btnNextMonth.Click += (s, e) => ChangeMonth(1);
            journalDatePicker.ValueChanged += DatePicker_ValueChanged;

            journalGrid.ColumnHeaderMouseClick += (s, e) => {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.ColumnIndex >= 0 && e.ColumnIndex < journalGrid.Columns.Count)
                    {
                        var column = journalGrid.Columns[e.ColumnIndex];
                        if (column.Name.StartsWith("lesson_"))
                        {
                            journalGrid.ClearSelection();
                            column.Selected = true;
                            headerContextMenu.Tag = column;
                            headerContextMenu.Show(Cursor.Position);
                        }
                    }
                }
            };

            menuEditLesson.Click += EditLessonItem_Click;
            menuSetGradeAll.Click += SetGradeForAll_Click;
            menuSetAbsenceAll.Click += SetAbsenceForAll_Click;
            menuClearGrades.Click += ClearGradesForAll_Click;
        }
        public AppUser CurrentUser => _teacher;

        public IList<ClassInfo> ClassesList
        {
            set
            {
                _isLoading = true;
                try
                {
                    classComboBox.DataSource = value;
                    classComboBox.DisplayMember = "Name";
                    classComboBox.ValueMember = "Id";
                }
                finally
                {
                    _isLoading = false;
                }

                if (value.Any()) ClassSelected?.Invoke();
            }
        }

        public ClassInfo? SelectedClass => classComboBox.SelectedItem as ClassInfo;

        public IList<DisciplineInfo> DisciplinesList
        {
            set
            {
                _isLoading = true;
                try
                {
                    disciplineComboBox.DataSource = value;
                    disciplineComboBox.DisplayMember = "DisciplineName";
                    disciplineComboBox.ValueMember = "DisciplineId";
                }
                finally
                {
                    _isLoading = false;
                }

                if (value.Any())
                {
                    DisciplineSelected?.Invoke();
                }
                else
                {
                    journalGrid.DataSource = null;
                }
            }
        }

        public DisciplineInfo? SelectedDiscipline => disciplineComboBox.SelectedItem as DisciplineInfo;

        public void SetJournalGrid(DataTable journalData)
        {
            journalGrid.SuspendLayout();

            try
            {
                journalGrid.DataSource = null;
                journalGrid.Columns.Clear();

                journalGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                journalGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                journalGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                journalGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                journalGrid.ColumnHeadersHeight = 55;
                journalGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                journalGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                journalGrid.RowHeadersWidth = 25;

                journalGrid.DataSource = journalData;

                ConfigureGridColumns(journalData);
            }
            finally
            {
                journalGrid.ResumeLayout(false);
            }
        }

        private void ConfigureGridColumns(DataTable journalData)
        {
            if (journalGrid.Columns.Contains("student_id"))
            {
                var colId = journalGrid.Columns["student_id"];
                colId.Visible = false;
                colId.Frozen = true;
            }

            if (journalGrid.Columns.Contains("student_name"))
            {
                var col = journalGrid.Columns["student_name"];
                col.HeaderText = "ФИО Ученика";
                col.Width = 200;
                col.Frozen = true;
                col.ReadOnly = true;
            }

            foreach (DataGridViewColumn col in journalGrid.Columns)
            {
                if (!col.Name.StartsWith("lesson_")) continue;

                col.Width = 85;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

                if (journalData.Columns.Contains(col.DataPropertyName))
                {
                    var extendedProps = journalData.Columns[col.DataPropertyName].ExtendedProperties;
                    if (extendedProps["Tag"] is string tagContent)
                    {
                        int firstPipe = tagContent.IndexOf('|');
                        if (firstPipe > 0)
                        {
                            string datePart = tagContent.Substring(0, firstPipe);
                            string rest = tagContent.Substring(firstPipe + 1);
                            int secondPipe = rest.IndexOf('|');
                            string lessonNum = (secondPipe > 0) ? rest.Substring(0, secondPipe) : rest;

                            col.HeaderText = $"{datePart}\n{lessonNum}";
                            col.ToolTipText = rest.Replace('|', '\n');
                        }
                        else
                        {
                            string[] parts = tagContent.Split('|');
                            if (parts.Length > 1)
                            {
                                col.HeaderText = parts[1];
                                col.ToolTipText = "Дата не назначена\n" + (parts.Length > 2 ? parts[2] : "");
                            }
                        }
                    }
                }
            }
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
            foreach (DataGridViewRow row in journalGrid.Rows)
            {
                if (row.Cells["student_id"].Value != null)
                    ids.Add(Convert.ToInt32(row.Cells["student_id"].Value));
            }
            return ids;
        }

        private (int lessonId, DateTime lessonDate) GetLessonContextFromColumn(DataGridViewColumn column)
        {
            int lessonId = Convert.ToInt32(column.Name.Split('_')[1]);
            return (lessonId, GetCurrentDate());
        }

        private void JournalGrid_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            var column = journalGrid.Columns[e.ColumnIndex];
            if (!column.Name.StartsWith("lesson_")) return;

            var cellValue = journalGrid.Rows[e.RowIndex].Cells["student_id"].Value;
            if (cellValue == null) return;

            int studentId = Convert.ToInt32(cellValue);
            int lessonId = Convert.ToInt32(column.Name.Split('_')[1]);
            string newValue = journalGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";

            SingleGradeChanged?.Invoke(studentId, lessonId, newValue, GetCurrentDate());
        }

        private void JournalGrid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (journalGrid.Columns[e.ColumnIndex].Name == "student_name")
            {
                var idVal = journalGrid.Rows[e.RowIndex].Cells["student_id"].Value;
                var nameVal = journalGrid.Rows[e.RowIndex].Cells["student_name"].Value?.ToString() ?? "";

                if (idVal != null)
                {
                    var studentInfo = new StudentInfo
                    {
                        Id = Convert.ToInt32(idVal),
                        LastName = nameVal
                    };

                    StudentSelected?.Invoke(studentInfo);
                }
            }
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
            lblMonth.Text = _currentDate.ToString("MMMM yyyy");
            journalDatePicker.ValueChanged -= DatePicker_ValueChanged;
            journalDatePicker.Value = _currentDate;
            journalDatePicker.ValueChanged += DatePicker_ValueChanged;
        }

        private void DatePicker_ValueChanged(object? sender, EventArgs e)
        {
            if (sender is DateTimePicker picker)
            {
                GoToDate(picker.Value);
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

            int lessonId = Convert.ToInt32(selectedColumn.Name.Split('_')[1]);
            using var form = new Form { Text = "Редактировать урок", Size = new Size(400, 220), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
            var dateLabel = new Label { Text = "Дата урока:", Left = 20, Top = 20 };
            var lessonDatePicker = new DateTimePicker { Left = 120, Top = 20, Width = 250, Format = DateTimePickerFormat.Short, Value = GetCurrentDate() };
            var topicLabel = new Label { Text = "Тема урока:", Left = 20, Top = 55 };
            var topicText = new TextBox { Left = 120, Top = 55, Width = 250, Height = 60, Multiline = true };
            var okButton = new Button { Text = "Сохранить", Left = 200, Top = 130, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 290, Top = 130, DialogResult = DialogResult.Cancel };
            form.Controls.AddRange(new Control[] { dateLabel, lessonDatePicker, topicLabel, topicText, okButton, cancelButton });
            form.AcceptButton = okButton;
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    _dataAccess.UpdateLessonDetails(lessonId, lessonDatePicker.Value, topicText.Text);
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