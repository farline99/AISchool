namespace AISchool.Models
{
	public class JournalStudent
	{
		public int StudentId { get; set; }
		public string LastName { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string? Patronymic { get; set; }
		public string FullName => $"{LastName} {FirstName} {Patronymic}".Trim();
	}

	public class JournalLesson
	{
		public int LessonId { get; set; }
		public short LessonNumber { get; set; }
		public DateTime? LessonDate { get; set; }
		public string? Topic { get; set; }
	}

	public class JournalGrade
	{
		public int StudentId { get; set; }
		public int LessonId { get; set; }
		public short? Grade { get; set; }
		public string? WorkType { get; set; }
	}

	public class JournalDataSource
	{
		public List<JournalStudent> Students { get; set; } = new();
		public List<JournalLesson> Lessons { get; set; } = new();
		public List<JournalGrade> Grades { get; set; } = new();
	}
}