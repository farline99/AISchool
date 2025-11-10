namespace AISchool.Models
{
	public class AppUser
	{
		public int Id { get; set; }
		public string LastName { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string? Patronymic { get; set; }
		public string Login { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty; 
		public int? RelatedStudentId { get; set; }
		public string? PasswordPlain { get; set; }
		public byte[]? PasswordHash { get; set; }
		public byte[]? PasswordSalt { get; set; }
	}
}