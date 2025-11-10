namespace AISchool.Models
{
	public class ParentInfo
	{
		public int Id { get; set; }
		public string LastName { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string? Patronymic { get; set; }
		public string? Phone { get; set; }
		public string Login { get; set; } = string.Empty;
		public string? Email { get; set; }
		public string? PasswordHash { get; set; }
		public string? PasswordSalt { get; set; }
		public string FullName => $"{LastName} {FirstName} {Patronymic}".Trim();
	}
}