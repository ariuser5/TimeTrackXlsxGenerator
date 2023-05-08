public partial class Options
{
	public static class Defaults
	{
		public const string OutputLocation = ".";
		public const string UserName = "User";
		public static (int row, int col) StartCell = (4, 2);
		public const int RowsSpace = 7;
		public const int WorkHours = 8;
		public const string DescriptionPlaceholder = "<Desc>";
		public const string DateFormat = "dd.MM.yyyy";
		public const bool AllowMissingConfig = false;
	}
}