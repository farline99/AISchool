using AISchool.Views;

namespace AISchool
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}