namespace AISchool.Models
{
	public class GradeEntry
	{
		public int? GradebookId { get; set; }
		public int StudentId { get; set; }
		public string StudentFullName { get; set; } = string.Empty;
		public short? Grade { get; set; }
		public string? WorkType { get; set; }
		public string DisplayValue => $"{Grade} - {WorkType}";
	}
}