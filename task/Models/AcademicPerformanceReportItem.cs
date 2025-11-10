namespace AISchool.Models
{
	public class AcademicPerformanceReportItem
	{
		public short ParallelNumber { get; set; }
		public string ClassName { get; set; } = string.Empty;
		public string DisciplineName { get; set; } = string.Empty;
		public decimal AvgGrade { get; set; }
		public decimal QualityPercent { get; set; }
		public decimal SuccessPercent { get; set; }
		public long TotalGrades { get; set; }
	}
}