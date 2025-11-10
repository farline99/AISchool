namespace AISchool.Models
{
	public class WorkloadView
	{
		public int DisciplineId { get; set; }
		public string DisciplineName { get; set; } = string.Empty;
		public int LessonsCount { get; set; }
		public int? TeacherId { get; set; }
		public string? TeacherFullName { get; set; }
	}
}