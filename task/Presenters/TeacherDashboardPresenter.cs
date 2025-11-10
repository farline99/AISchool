using AISchool.Data;
using AISchool.Models;
using AISchool.Views;
using System.Data;
using static AISchool.Data.DataAccess;

namespace AISchool.Presenters
{
	public class TeacherDashboardPresenter
	{
		private readonly ITeacherDashboardView _view;
		private readonly IDataAccess _dataAccess;

		public TeacherDashboardPresenter(ITeacherDashboardView view, IDataAccess dataAccess)
		{
			_view = view;
			_dataAccess = dataAccess;
			SubscribeToViewEvents();
		}

		private void SubscribeToViewEvents()
		{
			_view.LoadClasses += OnLoadClasses;
			_view.ClassSelected += OnClassSelected;
			_view.DisciplineSelected += OnDisciplineSelected;
			_view.BulkGradeActionRequested += OnBulkGradeAction;
			_view.SingleGradeChanged += OnSingleGradeChanged;
		}

		private async void OnSingleGradeChanged(int studentId, int lessonId, string newValue, DateTime lessonDate)
		{
			try
			{
				var cleanValue = newValue.Trim().ToUpper();

				if (string.IsNullOrEmpty(cleanValue))
				{
					await Task.Run(() => _dataAccess.ClearBulkGrades(new List<int> { studentId }, lessonId));
				}
				else if (cleanValue == "Н")
				{
					await Task.Run(() => _dataAccess.AddBulkGrades(new List<int> { studentId }, lessonId, null, "Н", lessonDate));
				}
				else if (int.TryParse(cleanValue, out int grade) && grade >= 1 && grade <= 5)
				{
					await Task.Run(() => _dataAccess.AddBulkGrades(new List<int> { studentId }, lessonId, grade, "Работа на уроке", lessonDate));
				}
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка сохранения оценки: {ex.Message}");
			}
			finally
			{
				OnDisciplineSelected();
			}
		}

		private async void OnBulkGradeAction(List<int> studentIds, int lessonId, int? grade, string workType, DateTime date)
		{
			if (studentIds == null || !studentIds.Any())
			{
				_view.ShowError("Не выбраны ученики для выполнения действия.");
				return;
			}

			try
			{
				if (grade == 0)
				{
					await Task.Run(() => _dataAccess.ClearBulkGrades(studentIds, lessonId));
				}
				else
				{
					await Task.Run(() => _dataAccess.AddBulkGrades(studentIds, lessonId, grade, workType, date));
				}
				_view.ShowSuccess("Оценки успешно проставлены.");
				OnDisciplineSelected();
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка при массовом выставлении оценок: {ex.Message}");
			}
		}

		private async void OnLoadClasses()
		{
			try
			{
				var classes = (await Task.Run(() => _dataAccess.GetClassesForTeacher(_view.CurrentUser.Id))).ToList();
				_view.ClassesList = classes;
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки классов: {ex.Message}");
			}
		}

		private async void OnClassSelected()
		{
			if (_view.SelectedClass == null)
			{
				_view.DisciplinesList = new List<DisciplineInfo>();
				return;
			}
			try
			{
				var disciplines = (await Task.Run(() => _dataAccess.GetDisciplinesForClass(_view.SelectedClass.Id, _view.CurrentUser.Id))).ToList();
				_view.DisciplinesList = disciplines;
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки предметов: {ex.Message}");
			}
		}

		private async void OnDisciplineSelected()
		{
			var selectedClass = _view.SelectedClass;
			var selectedDiscipline = _view.SelectedDiscipline;

			if (selectedClass == null || selectedDiscipline == null)
			{
				_view.SetJournalGrid(new DataTable());
				return;
			}
			try
			{
				var currentYear = await Task.Run(() => _dataAccess.GetCurrentAcademicYear());
				if (currentYear == null)
				{
					_view.ShowError("Не удалось определить текущий учебный год.");
					return;
				}
				var currentDate = _view.GetCurrentDate();
				var startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
				var endDate = startDate.AddMonths(1).AddDays(-1);
				var journalData = await Task.Run(() => _dataAccess.GetJournalDataSource(selectedClass.Id, selectedDiscipline.DisciplineId, currentYear.Id, startDate, endDate));
				var journalTable = BuildJournalTable(journalData);
				_view.SetJournalGrid(journalTable);
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки журнала: {ex.Message}");
			}
		}

		private DataTable BuildJournalTable(JournalDataSource journalData)
		{
			var table = new DataTable();
			table.Columns.Add("student_id", typeof(int));
			table.Columns.Add("student_name", typeof(string));
			foreach (var lesson in journalData.Lessons)
			{
				var column = new DataColumn($"lesson_{lesson.LessonId}", typeof(string));
				string dateString = lesson.LessonDate.HasValue ? lesson.LessonDate.Value.ToString("dd.MM.yyyy") : "";
				column.ExtendedProperties["Tag"] = $"{dateString}|Урок №{lesson.LessonNumber}|{lesson.Topic}";
				table.Columns.Add(column);
			}
			var gradesDict = journalData.Grades.GroupBy(g => (g.StudentId, g.LessonId)).ToDictionary(g => g.Key, g => string.Join(", ", g.Select(gr => gr.WorkType == "Н" ? "Н" : gr.Grade?.ToString() ?? "")));
			foreach (var student in journalData.Students)
			{
				var row = table.NewRow();
				row["student_id"] = student.StudentId;
				row["student_name"] = student.FullName;
				foreach (var lesson in journalData.Lessons)
				{
					if (gradesDict.TryGetValue((student.StudentId, lesson.LessonId), out var grades))
					{
						row[$"lesson_{lesson.LessonId}"] = grades;
					}
				}
				table.Rows.Add(row);
			}
			return table;
		}
	}
}