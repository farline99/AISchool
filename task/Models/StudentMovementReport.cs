using System.Text.Json.Serialization;

namespace AISchool.Models
{    public class StudentMovementInfo
	{
		[JsonPropertyName("fullName")]
		public string FullName { get; set; } = string.Empty;

		[JsonPropertyName("enrollmentDate")]
		public DateTime? EnrollmentDate { get; set; }

		[JsonPropertyName("departureDate")]
		public DateTime? DepartureDate { get; set; }

		[JsonPropertyName("className")]
		public string ClassName { get; set; } = string.Empty;
	}

	public class StudentMovementReport
	{
		[JsonPropertyName("totalAtStart")]
		public int TotalAtStart { get; set; }

		[JsonPropertyName("arrivedCount")]
		public int ArrivedCount { get; set; }

		[JsonPropertyName("departedCount")]
		public int DepartedCount { get; set; }

		[JsonPropertyName("totalAtEnd")]
		public int TotalAtEnd { get; set; }

		[JsonPropertyName("arrivedStudents")]
		public List<StudentMovementInfo> ArrivedStudents { get; set; } = new();

		[JsonPropertyName("departedStudents")]
		public List<StudentMovementInfo> DepartedStudents { get; set; } = new();
	}
}