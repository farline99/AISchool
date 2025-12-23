using AISchool.Data;
using AISchool.Models;

namespace AISchool.Views
{
    public partial class EditStudentGradesDialog : Form
    {
        private readonly int _lessonId;
        private readonly int _studentId;
        private readonly int _disciplineId;
        private bool _changesMade = false;
        private readonly IDataAccess _dataAccess;

        public EditStudentGradesDialog(int lessonId, int studentId, int disciplineId, string studentFullName, string lessonInfo)
        {
            InitializeComponent();

            _lessonId = lessonId;
            _studentId = studentId;
            _disciplineId = disciplineId;
            _dataAccess = new DataAccess();

            infoLabel.Text = $"{studentFullName}\n{lessonInfo}";

            this.Load += async (s, e) => await LoadGrades();
        }

        private async Task LoadGrades()
        {
            try
            {
                var grades = await Task.Run(() => _dataAccess.GetGradesForStudentLesson(_lessonId, _studentId));
                gradesListBox.DataSource = grades.ToList();
                gradesListBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки оценок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GradesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isSelected = gradesListBox.SelectedItem != null;
            editButton.Enabled = isSelected;
            deleteButton.Enabled = isSelected;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (_changesMade)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private async void AddButton_Click(object sender, EventArgs e)
        {
            var newGrade = ShowSingleGradeDialog(null);
            if (newGrade != null && newGrade.Grade.HasValue && newGrade.WorkType != null)
            {
                try
                {
                    await Task.Run(() => _dataAccess.AddGradeToLesson(
                        _studentId, _lessonId, _disciplineId,
                        newGrade.Grade.Value, DateTime.Now.Date, newGrade.WorkType
                    ));
                    _changesMade = true;
                    await LoadGrades();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления оценки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void EditButton_Click(object sender, EventArgs e)
        {
            if (gradesListBox.SelectedItem is not GradeEntry selectedGrade) return;

            var updatedGrade = ShowSingleGradeDialog(selectedGrade);
            if (updatedGrade != null && updatedGrade.GradebookId.HasValue && updatedGrade.Grade.HasValue && updatedGrade.WorkType != null)
            {
                try
                {
                    await Task.Run(() => _dataAccess.UpdateGrade(
                        updatedGrade.GradebookId.Value, updatedGrade.Grade.Value, updatedGrade.WorkType
                    ));
                    _changesMade = true;
                    await LoadGrades();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка изменения оценки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            if (gradesListBox.SelectedItem is not GradeEntry selectedGrade || !selectedGrade.GradebookId.HasValue) return;

            var confirmation = MessageBox.Show(
                $"Вы уверены, что хотите удалить оценку: {selectedGrade.DisplayValue}?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    await Task.Run(() => _dataAccess.DeleteGrade(selectedGrade.GradebookId.Value));
                    _changesMade = true;
                    await LoadGrades();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления оценки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private GradeEntry? ShowSingleGradeDialog(GradeEntry? existingGrade)
        {
            bool isNew = existingGrade == null;
            var gradeData = existingGrade ?? new GradeEntry();

            using var form = new Form
            {
                Text = isNew ? "Новая оценка" : "Изменить оценку",
                Size = new Size(300, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var gradeLabel = new Label { Text = "Оценка:", Left = 20, Top = 20 };
            var gradeNumeric = new NumericUpDown
            {
                Left = 120,
                Top = 20,
                Width = 150,
                Minimum = 1,
                Maximum = 5,
                Value = gradeData.Grade ?? 5
            };

            var workTypeLabel = new Label { Text = "Тип работы:", Left = 20, Top = 55 };
            var workTypeCombo = new ComboBox
            {
                Left = 120,
                Top = 55,
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            workTypeCombo.Items.AddRange(new[] { "Работа на уроке", "Ответ у доски", "Домашнее задание", "Контрольная работа", "Самостоятельная работа", "Диктант" });
            workTypeCombo.SelectedItem = gradeData.WorkType ?? "Работа на уроке";

            var okButton = new Button { Text = "OK", Left = 100, Top = 110, DialogResult = DialogResult.OK };
            var cancelButton = new Button { Text = "Отмена", Left = 190, Top = 110, DialogResult = DialogResult.Cancel };
            form.AcceptButton = okButton;

            form.Controls.AddRange(new Control[] { gradeLabel, gradeNumeric, workTypeLabel, workTypeCombo, okButton, cancelButton });

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                gradeData.Grade = (short)gradeNumeric.Value;
                gradeData.WorkType = workTypeCombo.SelectedItem?.ToString();
                return gradeData;
            }

            return null;
        }
    }
}