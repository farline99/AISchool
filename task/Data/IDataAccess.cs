using AISchool.Models;
using static AISchool.Data.DataAccess;

namespace AISchool.Data
{
	public interface IDataAccess
	{
		IEnumerable<AcademicYear> GetAcademicYears();
		void CreateAcademicYear(string name, DateTime startDate, DateTime endDate);
		void UpdateAcademicYearStatus(int yearId, string status);
		void AddBulkGrades(List<int> studentIds, int lessonId, int? grade, string workType, DateTime date);
		void ClearBulkGrades(List<int> studentIds, int lessonId);
		void ExecuteStudentPromotion(int completedYearId, int nextYearId);
		IEnumerable<TeacherWorkloadReportItem> GetTeacherWorkloadReport(int academicYearId);
		StudentMovementReport? GetStudentMovementReport(DateTime startDate, DateTime endDate);
		IEnumerable<WorkloadView> GetWorkloadForClass(int classId, int academicYearId);
		void UpsertWorkload(int classId, int disciplineId, int teacherId, int academicYearId);
		void DeleteWorkload(int classId, int disciplineId, int academicYearId);
		int AddStudyPlan(string name, int academicYearId, int parallelNumber);
		IEnumerable<StudyPlanView> GetAllStudyPlans();
		void ChangeUserPassword(int userId, string userRole, string newHash, string newSalt);
		IEnumerable<ClassDetails> GetAllClassesWithDetails();
		int AddGradeToLesson(int studentId, int lessonId, int disciplineId, int? grade, DateTime date, string workType);
		IEnumerable<StudyPlanItem> GetStudyPlanItems(int studyPlanId);
		void UpsertStudyPlanItem(int studyPlanId, int disciplineId, int lessonsCount);
		void DeleteStudyPlanItem(int itemId);
		IEnumerable<DisciplineInfo> GetAllDisciplines();
		int AddClass(string letter, int parallelNumber, int? headTeacherId);
		void UpdateClassHeadTeacher(int classId, int? newHeadTeacherId);
		IEnumerable<TeacherDetails> GetAllTeachers();
		int AddTeacher(TeacherDetails teacher);
		void UpdateTeacher(TeacherDetails teacher);
		void DeleteTeacher(int teacherId);
		AppUser? GetUserAuthDataByLogin(string login);
		void MigrateUserPassword(int userId, string userRole, string newHash, string newSalt);
		IEnumerable<DisciplineInfo> GetDisciplinesForStudent(int studentId, int teacherId);
		IEnumerable<DisciplineInfo> GetDisciplinesForStudent(int studentId);
		IEnumerable<LessonInfo> GetStudentLessonsAndGrades(int studentId, int disciplineId);
		void UpdateGrade(int gradebookId, int newGrade, string newWorkType);
		IEnumerable<ClassInfo> GetClassesForTeacher(int teacherId);
		IEnumerable<StudentInfo> GetStudentsInClass(int classId);
		StudentProfile? GetStudentProfile(int studentId);
		IEnumerable<AverageGrade> GetStudentAverageGrades(int studentId);
		IEnumerable<Achievement> GetStudentAchievements(int studentId);
		void DeleteGrade(int gradebookId);
		int AddAchievement(int studentId, Achievement achievement);
		void UpdateAchievement(Achievement achievement);
		void DeleteAchievement(int achievementId);
		void UpdateLessonDetails(int lessonId, DateTime? lessonDate, string topic);
		IEnumerable<GradeEntry> GetGradesForStudentLesson(int lessonId, int studentId);
		int AddStudent(StudentDetails student);
		void UpdateStudent(StudentDetails student);
		void ExpelStudent(int studentId);
		StudentDetails? GetStudentDetails(int studentId);
		void TransferStudent(int studentId, int newClassId);
		IEnumerable<ParentInfo> GetParentsForStudent(int studentId);
		IEnumerable<ParentInfo> GetAllParentsForLinking(int studentId);
		int AddParent(ParentInfo parent);
		void UpdateParent(ParentInfo parent);
		void LinkStudentToParent(int studentId, int parentId);
		void UnlinkStudentFromParent(int studentId, int parentId);
		AcademicYear? GetCurrentAcademicYear();
		IEnumerable<DisciplineInfo> GetDisciplinesForClass(int classId, int teacherId);
		JournalDataSource GetJournalDataSource(int classId, int disciplineId, int academicYearId, DateTime startDate, DateTime endDate);
		IEnumerable<AcademicPerformanceReportItem> GetAcademicPerformanceSummary(int academicYearId, DateTime startDate, DateTime endDate);
	}
}