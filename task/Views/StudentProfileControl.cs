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

        private bool _isComboBoxLoading = false;

        private List<LessonInfo> _lessonsMasterList = new List<LessonInfo>();
        private List<AverageGrade> _statsMasterList = new List<AverageGrade>();
        private List<Achievement> _achievementsMasterList = new List<Achievement>();

        public StudentProfileControl(int studentId, string userRole, int currentUserId)
        {
            InitializeComponent();

            UserRole = userRole;
            CurrentUserId = currentUserId;
            StudentId = studentId;

            _dataAccess = new DataAccess();
            _presenter = new StudentProfilePresenter(this, _dataAccess);

            ConfigurePermissions();
            SubscribeEvents();

            this.Dock = DockStyle.Fill;
            LoadStudentData?.Invoke(StudentId);
        }

        private void ConfigurePermissions()
        {
            bool isTeacherOrAdmin = (UserRole == "teacher" || UserRole == "admin");

            classmatesComboBox.Visible = isTeacherOrAdmin;
            achievementsButtonPanel.Visible = isTeacherOrAdmin;

            lessonsGrid.ReadOnly = false;
            colLessonNumber.ReadOnly = true;
            colGradesLine.ReadOnly = true;
            colLessonDate.ReadOnly = !isTeacherOrAdmin;
            colLessonTopic.ReadOnly = !isTeacherOrAdmin;

            if (!isTeacherOrAdmin)
            {
                achievementsGrid.ContextMenuStrip = null;
                lblGradesHint.Text = "Клик по заголовку столбца для сортировки.";
            }
            else
            {
                lblGradesHint.Text = "Двойной клик на оценках для их редактирования. Клик по заголовку столбца для сортировки.";
            }
        }

        private void SubscribeEvents()
        {
            classmatesComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (_isComboBoxLoading || classmatesComboBox.SelectedValue == null) return;
                if (classmatesComboBox.SelectedValue is int newStudentId && newStudentId != StudentId)
                {
                    LoadStudentData?.Invoke(newStudentId);
                }
            };

            disciplinesComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (disciplinesComboBox.SelectedValue is int disciplineId)
                {
                    _presenter.LoadLessonsForDiscipline(disciplineId);
                }
            };

            lessonsSearchBox.TextChanged += (s, e) => {
                string searchText = lessonsSearchBox.Text.ToLower().Trim();
                var filtered = string.IsNullOrEmpty(searchText)
                    ? _lessonsMasterList
                    : _lessonsMasterList.Where(l => (l.LessonTopic?.ToLower() ?? "").Contains(searchText)).ToList();
                lessonsGrid.DataSource = new SortableBindingList<LessonInfo>(filtered);
            };

            statsSearchBox.TextChanged += (s, e) => {
                string searchText = statsSearchBox.Text.ToLower().Trim();
                var filtered = string.IsNullOrEmpty(searchText)
                    ? _statsMasterList
                    : _statsMasterList.Where(item => item.DisciplineName.ToLower().Contains(searchText)).ToList();
                statsGrid.DataSource = new SortableBindingList<AverageGrade>(filtered);
            };

            achievementsSearchBox.TextChanged += (s, e) => {
                string searchText = achievementsSearchBox.Text.ToLower().Trim();
                var filtered = string.IsNullOrEmpty(searchText)
                    ? _achievementsMasterList
                    : _achievementsMasterList.Where(item => item.EventName.ToLower().Contains(searchText)).ToList();
                achievementsGrid.DataSource = new SortableBindingList<Achievement>(filtered);
            };

            lessonsGrid.CellDoubleClick += LessonsGrid_CellDoubleClick;
            lessonsGrid.CellEndEdit += LessonsGrid_CellEndEdit;

            btnAddAchievement.Click += AddAchievement_Click;
            menuEditAchievement.Click += EditAchievement_Click;
            menuDeleteAchievement.Click += DeleteAchievement_Click;

            btnUploadAvatar.Click += btnUploadAvatar_Click;
            btnDeleteAvatar.Click += btnDeleteAvatar_Click;
        }

        public string UserRole { get; }
        public int CurrentUserId { get; }
        public int StudentId { get; private set; }

        public event Action<int>? LoadStudentData;

        public void InvokeGoBack() => GoBackRequested?.Invoke();

        public void SetStudentProfile(StudentProfile profile)
        {
            StudentId = profile.Id;
            studentNameLabel.Text = profile.FullName;
            classNameLabel.Text = profile.ClassName;
            birthDateLabel.Text = profile.BirthDate.ToShortDateString();

            if (!string.IsNullOrWhiteSpace(profile.Notes))
            {
                notesLabel.Text = $"Заметки: {profile.Notes}";
                notesLabel.Visible = true;
            }
            else
            {
                notesLabel.Visible = false;
            }

            if (pbAvatar.Image != null)
            {
                pbAvatar.Image.Dispose();
                pbAvatar.Image = null;
            }

            if (profile.AvatarImage != null && profile.AvatarImage.Length > 0)
            {
                using (var ms = new MemoryStream(profile.AvatarImage))
                {
                    pbAvatar.Image = new Bitmap(ms);
                }
                btnDeleteAvatar.Visible = true;
                btnUploadAvatar.Text = "Изменить фото";
            }
            else
            {
                pbAvatar.Image = null;
                btnDeleteAvatar.Visible = false;
                btnUploadAvatar.Text = "Добавить фото";
            }
        }

        public void SetClassmates(IList<StudentInfo> classmates)
        {
            _isComboBoxLoading = true;
            classmatesComboBox.DataSource = classmates;
            classmatesComboBox.DisplayMember = "FullName";
            classmatesComboBox.ValueMember = "Id";
            classmatesComboBox.SelectedValue = StudentId;
            _isComboBoxLoading = false;
        }

        public void SetDisciplines(IList<DisciplineInfo> disciplines)
        {
            disciplinesComboBox.DataSource = disciplines;
            disciplinesComboBox.DisplayMember = "DisciplineName";
            disciplinesComboBox.ValueMember = "DisciplineId";
            if (!disciplines.Any())
            {
                lessonsGrid.DataSource = null;
            }
        }

        public void SetLessons(IList<LessonInfo> lessons)
        {
            _lessonsMasterList = lessons.ToList();
            lessonsGrid.DataSource = new SortableBindingList<LessonInfo>(_lessonsMasterList);
        }

        public void SetStats(IList<AverageGrade> stats)
        {
            _statsMasterList = stats.ToList();
            statsGrid.DataSource = new SortableBindingList<AverageGrade>(_statsMasterList);
        }

        public void SetAchievements(IList<Achievement> achievements)
        {
            _achievementsMasterList = achievements.ToList();
            achievementsGrid.DataSource = new SortableBindingList<Achievement>(_achievementsMasterList);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnUploadAvatar_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Выберите фото";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] imageBytes = File.ReadAllBytes(ofd.FileName);
                        if (imageBytes.Length > 2 * 1024 * 1024)
                        {
                            ShowError("Файл слишком большой. Максимальный размер 2МБ.");
                            return;
                        }

                        _dataAccess.UpdateUserAvatar(StudentId, "student", imageBytes);

                        if (pbAvatar.Image != null) pbAvatar.Image.Dispose();
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            pbAvatar.Image = new Bitmap(ms);
                        }
                        btnDeleteAvatar.Visible = true;
                        btnUploadAvatar.Text = "Изменить фото";
                    }
                    catch (Exception ex)
                    {
                        ShowError($"Ошибка загрузки фото: {ex.Message}");
                    }
                }
            }
        }

        private void btnDeleteAvatar_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить фото?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _dataAccess.UpdateUserAvatar(StudentId, "student", null);

                    if (pbAvatar.Image != null)
                    {
                        pbAvatar.Image.Dispose();
                        pbAvatar.Image = null;
                    }
                    btnDeleteAvatar.Visible = false;
                    btnUploadAvatar.Text = "Добавить фото";
                }
                catch (Exception ex)
                {
                    ShowError($"Ошибка удаления фото: {ex.Message}");
                }
            }
        }
        private async void LessonsGrid_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (lessonsGrid.Rows[e.RowIndex].DataBoundItem is not LessonInfo editedLesson) return;
            if (disciplinesComboBox.SelectedValue is not int selectedDisciplineId) return;

            var lessonIdForDb = editedLesson.LessonId;

            if (lessonIdForDb <= 0)
            {
                lessonIdForDb = await CreateLessonAndGetId(editedLesson, selectedDisciplineId);
                if (lessonIdForDb <= 0) { lessonsGrid.CancelEdit(); return; }
                _presenter.LoadLessonsForDiscipline(selectedDisciplineId);
                return;
            }

            var newDateStr = lessonsGrid.Rows[e.RowIndex].Cells[colLessonDate.Index].Value?.ToString();
            DateTime newDate = DateTime.TryParse(newDateStr, out var dt) ? dt : (editedLesson.LessonDate ?? DateTime.Today);
            string newTopic = lessonsGrid.Rows[e.RowIndex].Cells[colLessonTopic.Index].Value?.ToString() ?? "";

            try
            {
                await Task.Run(() => _dataAccess.UpdateLessonDetails(lessonIdForDb, newDate, newTopic));
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка сохранения: {ex.InnerException?.Message ?? ex.Message}");
                lessonsGrid.CancelEdit();
            }
        }

        private async void LessonsGrid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !(UserRole == "teacher" || UserRole == "admin")) return;
            if (lessonsGrid.Rows[e.RowIndex].DataBoundItem is not LessonInfo selectedLesson) return;
            if (disciplinesComboBox.SelectedValue is not int selectedDisciplineId) return;

            if (e.ColumnIndex == colGradesLine.Index)
            {
                var lessonIdForDb = selectedLesson.LessonId;

                if (lessonIdForDb <= 0)
                {
                    lessonIdForDb = await CreateLessonAndGetId(selectedLesson, selectedDisciplineId);
                    if (lessonIdForDb <= 0) return;
                }

                string lessonInfo = $"Урок №{selectedLesson.LessonNumber}" + (selectedLesson.LessonDate.HasValue ? $" от {selectedLesson.LessonDate.Value.ToShortDateString()}" : "");

                using var dialog = new EditStudentGradesDialog(lessonIdForDb, StudentId, selectedDisciplineId, (lessonsGrid.DataSource as SortableBindingList<LessonInfo>)?.FirstOrDefault()?.GradesLine ?? "", lessonInfo);
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

            using var form = new Form { Text = isNew ? "Добавить достижение" : "Редактировать достижение", Width = 400, Height = 250, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
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
            if (achievementsGrid.CurrentRow?.DataBoundItem is Achievement selectedAchievement)
            {
                ShowAchievementDialog(selectedAchievement);
            }
        }

        private async void DeleteAchievement_Click(object? sender, EventArgs e)
        {
            if (achievementsGrid.CurrentRow?.DataBoundItem is Achievement selectedAchievement)
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