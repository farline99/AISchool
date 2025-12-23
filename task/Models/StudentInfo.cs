namespace AISchool.Models
{
	public class StudentInfo
	{
		public int Id { get; set; }
		public string LastName { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string? Patronymic { get; set; }
		public string FullName => $"{LastName} {FirstName} {Patronymic}".Trim();
	}
}