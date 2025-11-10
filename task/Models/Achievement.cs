namespace AISchool.Models
{
	public class Achievement
	{
		public int Id { get; set; }
		public DateTime? EventDate { get; set; }
		public string EventName { get; set; } = string.Empty;
		public string? Level { get; set; }
		public int? Place { get; set; }
	}
}