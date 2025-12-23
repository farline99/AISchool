using AISchool.Models;
using static AISchool.Data.DataAccess;

namespace AISchool.Views
{
	public interface IStudentProfileView
	{
		string UserRole { get; }
		int CurrentUserId { get; }
		int StudentId { get; }

		event Action<int>? LoadStudentData;

		void SetStudentProfile(StudentProfile profile);
		void SetClassmates(IList<StudentInfo> classmates);
		void SetDisciplines(IList<DisciplineInfo> disciplines);
		void SetLessons(IList<LessonInfo> lessons);
		void SetStats(IList<AverageGrade> stats);
		void SetAchievements(IList<Achievement> achievements);

		void ShowError(string message);
		void InvokeGoBack();
	}
}