namespace AISchool.Models
{
    public class TeacherWorkloadReportItem
    {
        public int TeacherId { get; set; }
        public string TeacherFullName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string DisciplineName { get; set; } = string.Empty;
        public int LessonsCount { get; set; }
    }
}