using AISchool.Data;
using AISchool.Models;

namespace AISchool.Views
{
	public class EditStudentGradesDialog : Form
	{
		private readonly int _lessonId;
		private readonly int _studentId;
		private readonly int _disciplineId;
		private bool _changesMade = false;
		private readonly IDataAccess _dataAccess;

		private ListBox _gradesListBox = null!;
		private Button _addButton = null!;
		private Button _editButton = null!;
		private Button _deleteButton = null!;
		private Label _infoLabel = null!;

		public EditStudentGradesDialog(int lessonId, int studentId, int disciplineId, string studentFullName, string lessonInfo)
		{
			_lessonId = lessonId;
			_studentId = studentId;
			_disciplineId = disciplineId;
			_dataAccess = new DataAccess();

			this.Text = "Редактирование оценок";
			this.Size = new Size(400, 350);
			this.StartPosition = FormStartPosition.CenterParent;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;

			SetupUI(studentFullName, lessonInfo);
			this.Load += async (s, e) => await LoadGrades();
		}

		private void SetupUI(string studentFullName, string lessonInfo)
		{
			_infoLabel = new Label
			{
				Text = $"{studentFullName}\n{lessonInfo}",
				Dock = DockStyle.Top,
				Padding = new Padding(10),
				AutoSize = true,
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font(this.Font, FontStyle.Bold)
			};

			var buttonPanel = new Panel
			{
				Dock = DockStyle.Bottom,
				Height = 40,
				Padding = new Padding(5)
			};

			_addButton = new Button { Text = "Добавить", Width = 80, Location = new Point(5, 8) };
			_editButton = new Button { Text = "Изменить", Width = 80, Location = new Point(90, 8), Enabled = false };
			_deleteButton = new Button { Text = "Удалить", Width = 80, Location = new Point(175, 8), Enabled = false };

			var closeButton = new Button { Text = "Готово", Width = 100 };
			closeButton.Dock = DockStyle.Right;

			_addButton.Click += AddButton_Click;
			_editButton.Click += EditButton_Click;
			_deleteButton.Click += DeleteButton_Click;
			closeButton.Click += (s, e) =>
			{
				if (_changesMade)
				{
					this.DialogResult = DialogResult.OK;
				}
				else
				{
					this.DialogResult = DialogResult.Cancel;
				}
			};

			buttonPanel.Controls.Add(_addButton);
			buttonPanel.Controls.Add(_editButton);
			buttonPanel.Controls.Add(_deleteButton);
			buttonPanel.Controls.Add(closeButton);

			_gradesListBox = new ListBox
			{
				Dock = DockStyle.Fill,
				DisplayMember = "DisplayValue"
			};
			_gradesListBox.SelectedIndexChanged += (s, e) =>
			{
				bool isSelected = _gradesListBox.SelectedItem != null;
				_editButton.Enabled = isSelected;
				_deleteButton.Enabled = isSelected;
			};

			this.Controls.Add(_gradesListBox);
			this.Controls.Add(buttonPanel);
			this.Controls.Add(_infoLabel);
		}

		private async Task LoadGrades()
		{
			try
			{
				var grades = await Task.Run(() => _dataAccess.GetGradesForStudentLesson(_lessonId, _studentId));
				_gradesListBox.DataSource = grades.ToList();
				_gradesListBox.SelectedIndex = -1;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки оценок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void AddButton_Click(object? sender, EventArgs e)
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

		private async void EditButton_Click(object? sender, EventArgs e)
		{
			if (_gradesListBox.SelectedItem is not GradeEntry selectedGrade) return;

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

		private async void DeleteButton_Click(object? sender, EventArgs e)
		{
			if (_gradesListBox.SelectedItem is not GradeEntry selectedGrade || !selectedGrade.GradebookId.HasValue) return;

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
				FormBorderStyle = FormBorderStyle.FixedDialog
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