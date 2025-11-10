namespace AISchool.Models
{
	public class StudyPlanItem
	{
		public int Id { get; set; }
		public int DisciplineId { get; set; }
		public string DisciplineName { get; set; } = string.Empty;
		public int LessonsCount { get; set; }
		public double AcademicHours => Math.Round(LessonsCount * 45.0 / 60.0, 1);
	}
}