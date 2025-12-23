using AISchool.Data;
using AISchool.Models;
using AISchool.Views;

namespace AISchool.Presenters
{
	public class StudentProfilePresenter
	{
		private readonly IStudentProfileView _view;
		private readonly IDataAccess _dataAccess;
		private StudentProfile? _currentProfile;

		public StudentProfilePresenter(IStudentProfileView view, IDataAccess dataAccess)
		{
			_view = view;
			_dataAccess = dataAccess;
			_view.LoadStudentData += OnLoadStudentData;
		}

		private async void OnLoadStudentData(int studentId)
		{
			try
			{
				var newProfile = await Task.Run(() => _dataAccess.GetStudentProfile(studentId));
				if (newProfile == null)
				{
					_view.ShowError($"Профиль ученика с ID {studentId} не найден.");
					_view.InvokeGoBack();
					return;
				}

				bool classHasChanged = _currentProfile == null || _currentProfile.ClassId != newProfile.ClassId;
				_currentProfile = newProfile;

				_view.SetStudentProfile(_currentProfile);

				if (classHasChanged)
				{
					await LoadClassmates();
				}

				await LoadDisciplines();
				await LoadStats();
				await LoadAchievements();
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки данных студента: {ex.Message}");
			}
		}

		private async Task LoadClassmates()
		{
			if (_currentProfile == null) return;
			var classmates = (await Task.Run(() => _dataAccess.GetStudentsInClass(_currentProfile.ClassId))).ToList();
			_view.SetClassmates(classmates);
		}

		private async Task LoadDisciplines()
		{
			if (_currentProfile == null) return;
			var disciplines = (_view.UserRole == "teacher" || _view.UserRole == "admin")
				? (await Task.Run(() => _dataAccess.GetDisciplinesForStudent(_currentProfile.Id, _view.CurrentUserId))).ToList()
				: (await Task.Run(() => _dataAccess.GetDisciplinesForStudent(_currentProfile.Id))).ToList();

			_view.SetDisciplines(disciplines);

			if (disciplines.Any())
			{
				LoadLessonsForDiscipline(disciplines.First().DisciplineId);
			}
		}

		public async void LoadLessonsForDiscipline(int disciplineId)
		{
			if (_currentProfile == null) return;
			try
			{
				var lessons = (await Task.Run(() => _dataAccess.GetStudentLessonsAndGrades(_currentProfile.Id, disciplineId))).ToList();
				_view.SetLessons(lessons);
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки занятий: {ex.Message}");
			}
		}

		private async Task LoadStats()
		{
			if (_currentProfile == null) return;
			var stats = (await Task.Run(() => _dataAccess.GetStudentAverageGrades(_currentProfile.Id))).ToList();
			_view.SetStats(stats);
		}

		private async Task LoadAchievements()
		{
			if (_currentProfile == null) return;
			var achievements = (await Task.Run(() => _dataAccess.GetStudentAchievements(_currentProfile.Id))).ToList();
			_view.SetAchievements(achievements);
		}
	}
}