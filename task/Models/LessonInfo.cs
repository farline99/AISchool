namespace AISchool.Models
{
	public class LessonInfo
	{
		public int LessonId { get; set; }
		public short LessonNumber { get; set; }
		public DateTime? LessonDate { get; set; }
		public string? LessonTopic { get; set; }
		public string? GradesLine { get; set; }
	}
}