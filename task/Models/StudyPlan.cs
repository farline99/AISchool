namespace AISchool.Models
{
	public class StudyPlan
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int AcademicYearId { get; set; }
		public int ParallelId { get; set; }
		public int ParallelNumber { get; set; }
	}
}