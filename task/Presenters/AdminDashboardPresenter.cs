using AISchool.Data;
using AISchool.Models;
using AISchool.Views;

namespace AISchool.Presenters
{
	public class AdminDashboardPresenter
	{
		private readonly IAdminDashboardView _view;
		private readonly IDataAccess _dataAccess;

		private List<TeacherDetails> _allTeachersMaster = new List<TeacherDetails>();
		private List<ClassDetails> _allClassesMaster = new List<ClassDetails>();
		private List<StudentInfo> _currentClassStudentsMaster = new List<StudentInfo>();
		private List<StudyPlanView> _allStudyPlansMaster = new List<StudyPlanView>();
		private List<StudyPlanItem> _currentStudyPlanItemsMaster = new List<StudyPlanItem>();
		private List<AcademicYear> _academicYearsMaster = new List<AcademicYear>();
		private List<ParentInfo> _currentStudentParentsMaster = new List<ParentInfo>();

		public AdminDashboardPresenter(IAdminDashboardView view, IDataAccess dataAccess)
		{
			_view = view;
			_dataAccess = dataAccess;
			SubscribeToViewEvents();
		}

		private void SubscribeToViewEvents()
		{
			_view.LoadTeachers += OnLoadTeachers;
			_view.AddTeacher += OnAddTeacher;
			_view.EditTeacher += OnEditTeacher;
			_view.DeleteTeacher += OnDeleteTeacher;
			_view.SearchTeacher += OnSearchTeacher;
			_view.LoadClasses += OnLoadClasses;
			_view.ClassSelected += OnClassSelected;
			_view.AddClass += OnAddClass;
			_view.EditClass += OnEditClass;
			_view.AddStudent += OnAddStudent;
			_view.EditStudent += OnEditStudent;
			_view.ExpelStudent += OnExpelStudent;
			_view.TransferStudent += OnTransferStudent;
			_view.LoadStudyPlans += OnLoadStudyPlans;
			_view.StudyPlanSelected += OnStudyPlanSelected;
			_view.AddStudyPlan += OnAddStudyPlan;
			_view.AddStudyPlanItem += OnAddStudyPlanItem;
			_view.DeleteStudyPlanItem += OnDeleteStudyPlanItem;
			_view.LoadWorkloadData += OnLoadWorkloadData;
			_view.WorkloadFilterChanged += OnWorkloadFilterChanged;
			_view.AssignTeacherToWorkload += OnAssignTeacherToWorkload;
			_view.RemoveTeacherFromWorkload += OnRemoveTeacherFromWorkload;
			_view.LoadAcademicYears += OnLoadAcademicYears;
			_view.UpdateYearStatus += OnUpdateYearStatus;
			_view.PromoteStudents += OnPromoteStudents;
			_view.StudentSelected += OnStudentSelected;
			_view.AddNewParent += OnAddNewParent;
			_view.LinkExistingParent += OnLinkExistingParent;
			_view.EditParent += OnEditParent;
			_view.UnlinkParent += OnUnlinkParent;
		}

		private async void OnStudentSelected(StudentInfo? selectedStudent)
		{
			if (selectedStudent == null)
			{
				_currentStudentParentsMaster.Clear();
				_view.LinkedParentsList = _currentStudentParentsMaster;
				return;
			}

			try
			{
				_currentStudentParentsMaster = (await Task.Run(() => _dataAccess.GetParentsForStudent(selectedStudent.Id))).ToList();
				_view.LinkedParentsList = _currentStudentParentsMaster;
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки родителей: {ex.Message}");
			}
		}

		private void OnAddNewParent()
		{
			if (_view.SelectedStudent == null)
			{
				_view.ShowError("Сначала выберите ученика.");
				return;
			}
			_view.ShowParentDialog(null, true);
		}

		private async void OnLinkExistingParent()
		{
			if (_view.SelectedStudent == null)
			{
				_view.ShowError("Сначала выберите ученика.");
				return;
			}
			try
			{
				var availableParents = (await Task.Run(() => _dataAccess.GetAllParentsForLinking(_view.SelectedStudent.Id))).ToList();
				_view.ShowLinkParentDialog(availableParents);
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки списка родителей для привязки: {ex.Message}");
			}
		}

		private void OnEditParent()
		{
			if (_view.SelectedParent == null)
			{
				_view.ShowError("Выберите родителя для редактирования.");
				return;
			}
			_view.ShowParentDialog(_view.SelectedParent, false);
		}

		private async void OnUnlinkParent()
		{
			if (_view.SelectedStudent == null || _view.SelectedParent == null)
			{
				_view.ShowError("Выберите ученика и родителя для отвязки.");
				return;
			}

			if (_view.ShowConfirmation($"Вы уверены, что хотите отвязать родителя '{_view.SelectedParent.FullName}' от ученика '{_view.SelectedStudent.FullName}'?"))
			{
				try
				{
					await Task.Run(() => _dataAccess.UnlinkStudentFromParent(_view.SelectedStudent.Id, _view.SelectedParent.Id));
					_view.ShowSuccess("Связь успешно удалена.");
					OnStudentSelected(_view.SelectedStudent);
				}
				catch (Exception ex)
				{
					_view.ShowError($"Ошибка удаления связи: {ex.Message}");
				}
			}
		}

		private async void OnLoadTeachers()
		{
			try
			{
				_allTeachersMaster = (await Task.Run(() => _dataAccess.GetAllTeachers())).ToList();
				_view.TeachersList = _allTeachersMaster;
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки учителей: {ex.Message}");
			}
		}

		private void OnSearchTeacher(string searchText)
		{
			var filteredTeachers = string.IsNullOrEmpty(searchText)
				? _allTeachersMaster
				: _allTeachersMaster.Where(t =>
					(t.FullName?.ToLower() ?? "").Contains(searchText) ||
					(t.Login?.ToLower() ?? "").Contains(searchText)
				).ToList();
			_view.TeachersList = filteredTeachers;
		}

		private void OnAddTeacher() => _view.ShowTeacherDialog(null, true);

		private void OnEditTeacher()
		{
			if (_view.SelectedTeacher != null)
			{
				_view.ShowTeacherDialog(_view.SelectedTeacher, false);
			}
		}

		private async void OnDeleteTeacher()
		{
			var selectedTeacher = _view.SelectedTeacher;
			if (selectedTeacher == null) return;

			if (selectedTeacher.Id == _view.CurrentUser.Id)
			{
				_view.ShowError("Вы не можете удалить свою собственную учетную запись.");
				return;
			}

			if (_view.ShowConfirmation($"Вы уверены, что хотите удалить учителя: {selectedTeacher.FullName}?"))
			{
				try
				{
					await Task.Run(() => _dataAccess.DeleteTeacher(selectedTeacher.Id));
					_view.ShowSuccess("Учитель успешно удален.");
					OnLoadTeachers();
				}
				catch (Exception ex)
				{
					_view.ShowError($"Ошибка удаления: {ex.Message}");
				}
			}
		}

		private async void OnLoadClasses()
		{
			try
			{
				_allClassesMaster = (await Task.Run(() => _dataAccess.GetAllClassesWithDetails())).ToList();
				_view.ClassesList = _allClassesMaster;
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки классов: {ex.Message}");
			}
		}

		private async void OnClassSelected(ClassDetails? selectedClass)
		{
			_view.ClearStudentSearch();

			if (selectedClass == null)
			{
				_currentClassStudentsMaster.Clear();
				_view.StudentsList = _currentClassStudentsMaster;
				return;
			}

			try
			{
				_currentClassStudentsMaster = (await Task.Run(() => _dataAccess.GetStudentsInClass(selectedClass.ClassId))).ToList();
				_view.StudentsList = _currentClassStudentsMaster;
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки учеников: {ex.Message}");
			}
		}

		private async void OnAddClass()
		{
			var allTeachers = (await Task.Run(() => _dataAccess.GetAllTeachers())).ToList();
			_view.ShowClassDialog(allTeachers);
		}

		private async void OnEditClass()
		{
			if (_view.SelectedClass == null)
			{
				_view.ShowError("Пожалуйста, выберите класс для назначения руководителя.");
				return;
			}
			var allTeachers = (await Task.Run(() => _dataAccess.GetAllTeachers())).ToList();
			_view.ShowHeadTeacherDialog(_view.SelectedClass, allTeachers);
		}

		private void OnAddStudent()
		{
			if (_view.SelectedClass == null)
			{
				_view.ShowError("Сначала выберите класс в верхней таблице, чтобы добавить в него ученика.");
				return;
			}

			var newStudent = new StudentDetails
			{
				ClassId = _view.SelectedClass.ClassId,
				BirthDate = DateTime.Now.AddYears(-7)
			};
			_view.ShowStudentDialog(newStudent, true, _allClassesMaster);
		}

		private async void OnEditStudent()
		{
			if (_view.SelectedStudent == null)
			{
				_view.ShowError("Выберите ученика для редактирования.");
				return;
			}

			var studentToEdit = await Task.Run(() => _dataAccess.GetStudentDetails(_view.SelectedStudent.Id));
			if (studentToEdit == null)
			{
				_view.ShowError("Не удалось загрузить данные ученика.");
				return;
			}

			_view.ShowStudentDialog(studentToEdit, false, _allClassesMaster);
		}

		private async void OnExpelStudent()
		{
			if (_view.SelectedStudent == null)
			{
				_view.ShowError("Выберите ученика для отчисления.");
				return;
			}
			if (_view.ShowConfirmation($"Вы уверены, что хотите отчислить ученика: {_view.SelectedStudent.FullName}?\n\nЕго статус будет изменен на 'Отчислен', а все данные сохранены в архиве."))
			{
				try
				{
					await Task.Run(() => _dataAccess.ExpelStudent(_view.SelectedStudent.Id));
					_view.ShowSuccess("Ученик успешно отчислен.");
					OnClassSelected(_view.SelectedClass);
				}
				catch (Exception ex)
				{
					_view.ShowError($"Ошибка отчисления: {ex.InnerException?.Message ?? ex.Message}");
				}
			}
		}

		private async void OnTransferStudent()
		{
			if (_view.SelectedStudent == null)
			{
				_view.ShowError("Пожалуйста, выберите ученика для перевода.");
				return;
			}

			var allClasses = (await Task.Run(() => _dataAccess.GetAllClassesWithDetails())).ToList();
			_view.ShowTransferStudentDialog(_view.SelectedStudent, allClasses);
		}

		private async void OnLoadStudyPlans()
		{
			try
			{
				_allStudyPlansMaster = (await Task.Run(() => _dataAccess.GetAllStudyPlans())).ToList();
				_view.StudyPlansList = _allStudyPlansMaster;
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки учебных планов: {ex.Message}");
			}
		}

		private async void OnStudyPlanSelected(StudyPlanView? selectedPlan)
		{
			if (selectedPlan == null)
			{
				_currentStudyPlanItemsMaster.Clear();
				_view.StudyPlanItemsList = _currentStudyPlanItemsMaster;
				return;
			}
			try
			{
				_currentStudyPlanItemsMaster = (await Task.Run(() => _dataAccess.GetStudyPlanItems(selectedPlan.Id))).ToList();
				_view.StudyPlanItemsList = _currentStudyPlanItemsMaster;
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки предметов плана: {ex.Message}");
			}
		}

		private void OnAddStudyPlan() => _view.ShowStudyPlanDialog();

		private async void OnAddStudyPlanItem()
		{
			if (_view.SelectedStudyPlan == null)
			{
				_view.ShowError("Сначала выберите учебный план в верхней таблице.");
				return;
			}

			var allDisciplines = (await Task.Run(() => _dataAccess.GetAllDisciplines())).ToList();
			_view.ShowStudyPlanItemDialog(_view.SelectedStudyPlan, allDisciplines);
		}

		private async void OnDeleteStudyPlanItem()
		{
			if (_view.SelectedStudyPlanItem == null)
			{
				_view.ShowError("Выберите предмет в нижней таблице для удаления.");
				return;
			}

			if (_view.ShowConfirmation($"Вы уверены, что хотите удалить предмет '{_view.SelectedStudyPlanItem.DisciplineName}' из учебного плана?"))
			{
				try
				{
					await Task.Run(() => _dataAccess.DeleteStudyPlanItem(_view.SelectedStudyPlanItem.Id));
					_view.ShowSuccess("Предмет успешно удален из плана.");
					OnStudyPlanSelected(_view.SelectedStudyPlan);
				}
				catch (Exception ex)
				{
					_view.ShowError($"Ошибка удаления: {ex.InnerException?.Message ?? ex.Message}");
				}
			}
		}

		private async void OnLoadWorkloadData()
		{
			try
			{
				var years = await Task.Run(() => _dataAccess.GetAcademicYears());
				_view.WorkloadAcademicYearsList = years.ToList();

				var classes = await Task.Run(() => _dataAccess.GetAllClassesWithDetails());
				_view.WorkloadClassesList = classes.ToList();
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки данных для вкладки 'Нагрузка': {ex.Message}");
			}
		}

		private async void OnWorkloadFilterChanged(int? classId, int? academicYearId)
		{
			if (classId.HasValue && academicYearId.HasValue)
			{
				try
				{
					var workload = await Task.Run(() => _dataAccess.GetWorkloadForClass(classId.Value, academicYearId.Value));
					_view.WorkloadList = workload.ToList();
				}
				catch (Exception ex)
				{
					_view.ShowError($"Ошибка загрузки нагрузки: {ex.Message}");
				}
			}
			else
			{
				_view.WorkloadList = new List<WorkloadView>();
			}
		}

		private async void OnAssignTeacherToWorkload()
		{
			if (_view.SelectedWorkloadClass == null || _view.SelectedWorkloadAcademicYear == null)
			{
				_view.ShowError("Сначала выберите класс и учебный год.");
				return;
			}
			if (_view.SelectedWorkloadItem == null)
			{
				_view.ShowError("Выберите предмет в таблице для назначения учителя.");
				return;
			}

			try
			{
				var allTeachers = (await Task.Run(() => _dataAccess.GetAllTeachers())).ToList();
				_view.ShowAssignTeacherDialog(_view.SelectedWorkloadItem, allTeachers);
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка: {ex.Message}");
			}
		}

		private async void OnRemoveTeacherFromWorkload()
		{
			var selectedWorkloadItem = _view.SelectedWorkloadItem;
			if (_view.SelectedWorkloadClass == null || _view.SelectedWorkloadAcademicYear == null)
			{
				_view.ShowError("Сначала выберите класс и учебный год.");
				return;
			}
			if (selectedWorkloadItem == null)
			{
				_view.ShowError("Выберите предмет в таблице, с которого нужно снять учителя.");
				return;
			}
			if (!selectedWorkloadItem.TeacherId.HasValue)
			{
				_view.ShowError("На этот предмет еще не назначен учитель.");
				return;
			}

			if (_view.ShowConfirmation($"Вы уверены, что хотите снять учителя {selectedWorkloadItem.TeacherFullName} с предмета '{selectedWorkloadItem.DisciplineName}'?"))
			{
				try
				{
					await Task.Run(() => _dataAccess.DeleteWorkload(_view.SelectedWorkloadClass.ClassId, selectedWorkloadItem.DisciplineId, _view.SelectedWorkloadAcademicYear.Id));
					_view.ShowSuccess("Учитель успешно снят с предмета.");
					OnWorkloadFilterChanged(_view.SelectedWorkloadClass.ClassId, _view.SelectedWorkloadAcademicYear.Id);
				}
				catch (Exception ex)
				{
					_view.ShowError($"Ошибка: {ex.InnerException?.Message ?? ex.Message}");
				}
			}
		}

		private async void OnLoadAcademicYears()
		{
			try
			{
				_academicYearsMaster = (await Task.Run(() => _dataAccess.GetAcademicYears())).ToList();
				_view.AcademicYearsList = _academicYearsMaster;
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка загрузки учебных годов: {ex.Message}");
			}
		}

		private async void OnUpdateYearStatus(int yearId, string newStatus)
		{
			try
			{
				if (newStatus == "Current" && _academicYearsMaster.Any(y => y.Status == "Current" && y.Id != yearId))
				{
					_view.ShowError("В системе уже есть активный учебный год. Сначала архивируйте его.");
					OnLoadAcademicYears();
					return;
				}

				await Task.Run(() => _dataAccess.UpdateAcademicYearStatus(yearId, newStatus));
				_view.ShowSuccess("Статус года успешно обновлен.");
				OnLoadAcademicYears();
			}
			catch (Exception ex)
			{
				_view.ShowError($"Ошибка обновления статуса: {ex.Message}");
				OnLoadAcademicYears();
			}
		}

		private async void OnPromoteStudents()
		{
			var currentYear = _academicYearsMaster.FirstOrDefault(y => y.Status == "Current");
			var upcomingYear = _academicYearsMaster.Where(y => y.Status == "Upcoming" && y.StartDate > currentYear?.EndDate).OrderBy(y => y.StartDate).FirstOrDefault();

			if (currentYear == null)
			{
				_view.ShowError("Не найден текущий учебный год для завершения.");
				return;
			}

			if (upcomingYear == null)
			{
				_view.ShowError("Не найден следующий (статус 'Upcoming') учебный год для перевода. Сначала создайте его.");
				return;
			}

			if (_view.ShowConfirmation($"Вы уверены, что хотите завершить {currentYear.Name} и перевести всех учеников в {upcomingYear.Name}?\n\nЭто действие необратимо."))
			{
				try
				{
					await Task.Run(() => _dataAccess.ExecuteStudentPromotion(currentYear.Id, upcomingYear.Id));
					_view.ShowSuccess("Ученики успешно переведены. Учебный год обновлен.");
					OnLoadAcademicYears();
					OnLoadClasses();
				}
				catch (Exception ex)
				{
					_view.ShowError($"Произошла ошибка во время перевода: {ex.InnerException?.Message ?? ex.Message}");
				}
			}
		}
	}
}