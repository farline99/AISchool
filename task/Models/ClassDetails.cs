namespace AISchool.Models
{
	public class ClassDetails
	{
		public int ClassId { get; set; }
		public string ClassName { get; set; } = string.Empty;
		public int ParallelNumber { get; set; }
		public int? HeadTeacherId { get; set; }
		public string? HeadTeacherFullName { get; set; }
	}
}