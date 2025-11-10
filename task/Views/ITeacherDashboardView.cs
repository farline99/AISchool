using AISchool.Models;
using System.Data;
using static AISchool.Data.DataAccess;

namespace AISchool.Views
{
	public interface ITeacherDashboardView
	{
		AppUser CurrentUser { get; }
		IList<ClassInfo> ClassesList { set; }
		ClassInfo? SelectedClass { get; }
		DateTime GetCurrentDate();

		IList<DisciplineInfo> DisciplinesList { set; }
		DisciplineInfo? SelectedDiscipline { get; }
		void SetJournalGrid(DataTable journalData);

		event Action? LoadClasses;
		event Action? ClassSelected;
		event Action? DisciplineSelected;
		event Action<List<int>, int, int?, string, DateTime>? BulkGradeActionRequested;
		event Action<int, int, string, DateTime>? SingleGradeChanged;

		void ShowError(string message);
		void ShowSuccess(string message);
	}
}