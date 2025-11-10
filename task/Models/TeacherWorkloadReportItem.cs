namespace AISchool.Models
{
	public class TeacherWorkloadReportItem
	{
		public int TeacherId { get; set; }
		public string TeacherFullName { get; set; } = string.Empty;
		public long TotalLessonsCount { get; set; }
		public string WorkloadDetails { get; set; } = string.Empty;
	}
}