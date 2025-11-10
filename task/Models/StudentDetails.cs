namespace AISchool.Models
{
	public class StudentDetails
	{
		public int Id { get; set; }
		public string LastName { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string? Patronymic { get; set; }
		public int ClassId { get; set; }
		public DateTime BirthDate { get; set; }
		public string? Notes { get; set; }
		public string FullName => $"{LastName} {FirstName} {Patronymic}".Trim();
	}
}