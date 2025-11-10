namespace AISchool.Models
{
	public class StudentProfile
	{
		public int Id { get; set; }
		public string LastName { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string? Patronymic { get; set; }
		public string ClassName { get; set; } = string.Empty;
		public DateTime BirthDate { get; set; }
		public string? Notes { get; set; }
		public string FullName => $"{LastName} {FirstName} {Patronymic}".Trim();
		public int ClassId { get; set; }
	}
}