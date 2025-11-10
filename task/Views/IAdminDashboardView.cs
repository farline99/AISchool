using AISchool.Models;
using static AISchool.Data.DataAccess;

namespace AISchool.Views
{
	public interface IAdminDashboardView
	{
		IList<TeacherDetails> TeachersList { set; }
		TeacherDetails? SelectedTeacher { get; }
		AppUser CurrentUser { get; }
		IList<ParentInfo> LinkedParentsList { set; }
		ParentInfo? SelectedParent { get; }
		event Action? LoadTeachers;
		event Action? AddTeacher;
		event Action? EditTeacher;
		event Action? DeleteTeacher;
		event Action<string>? SearchTeacher;
		event Action<StudentInfo?>? StudentSelected;

		IList<ClassDetails> ClassesList { set; }
		IList<StudentInfo> StudentsList { set; }
		ClassDetails? SelectedClass { get; }
		StudentInfo? SelectedStudent { get; }
		event Action? LoadClasses;
		event Action<ClassDetails?>? ClassSelected;
		event Action? AddClass;
		event Action? EditClass;
		event Action? AddStudent;
		event Action? EditStudent;
		event Action? ExpelStudent;
		event Action? TransferStudent;

		IList<StudyPlanView> StudyPlansList { set; }
		IList<StudyPlanItem> StudyPlanItemsList { set; }
		StudyPlanView? SelectedStudyPlan { get; }
		StudyPlanItem? SelectedStudyPlanItem { get; }
		event Action? LoadStudyPlans;
		event Action<StudyPlanView?>? StudyPlanSelected;
		event Action? AddStudyPlan;
		event Action? AddStudyPlanItem;
		event Action? DeleteStudyPlanItem;

		IList<AcademicYear> WorkloadAcademicYearsList { set; }
		IList<ClassDetails> WorkloadClassesList { set; }
		IList<WorkloadView> WorkloadList { set; }
		WorkloadView? SelectedWorkloadItem { get; }
		ClassDetails? SelectedWorkloadClass { get; }
		AcademicYear? SelectedWorkloadAcademicYear { get; }
		event Action? LoadWorkloadData;
		event Action<int?, int?>? WorkloadFilterChanged;
		event Action? AssignTeacherToWorkload;
		event Action? RemoveTeacherFromWorkload;

		IList<AcademicYear> AcademicYearsList { set; }
		AcademicYear? SelectedAcademicYear { get; }
		event Action? LoadAcademicYears;
		event Action? CreateNewYear;
		event Action? PromoteStudents;
		event Action<int, string>? UpdateYearStatus;

		event Action? AddNewParent;
		event Action? LinkExistingParent;
		event Action? EditParent;
		event Action? UnlinkParent;

		void ShowParentDialog(ParentInfo? parent, bool isNew);
		void ShowLinkParentDialog(List<ParentInfo> allParents);

		void ShowError(string message);
		void ShowSuccess(string message);
		bool ShowConfirmation(string message);
		void ShowTeacherDialog(TeacherDetails? teacher, bool isNew);
		void ShowClassDialog(List<TeacherDetails> allTeachers);
		void ShowHeadTeacherDialog(ClassDetails selectedClass, List<TeacherDetails> allTeachers);
		void ShowStudentDialog(StudentDetails student, bool isNew, List<ClassDetails> allClasses);
		void ShowTransferStudentDialog(StudentInfo selectedStudent, List<ClassDetails> allClasses);
		void ShowStudyPlanDialog();
		void ShowStudyPlanItemDialog(StudyPlanView selectedPlan, List<DisciplineInfo> allDisciplines);
		void ShowAssignTeacherDialog(WorkloadView selectedWorkload, List<TeacherDetails> allTeachers);
		void ClearStudentSearch();
	}
}