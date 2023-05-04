
record Config : TimesheetParams
{
	public static class Defaults
	{
		public const string OutputLocation = ".";
		public const string UserName = "User";
		public static int[] StartCell = { 4, 2 };
		public static int TableBeginAfterRows = 7;
		public const int WorkHours = 8;
		public const string DescriptionPlaceholder = "<Desc>";
		public const string DateFormat = "dd.MM.yyyy";
	}
	
	public string OutputLocation { get; set; } = Defaults.OutputLocation;
	
	public Config() : base(
		Defaults.UserName, 
		Defaults.StartCell, 
		Defaults.TableBeginAfterRows, 
		Defaults.WorkHours, 
		Defaults.DescriptionPlaceholder, 
		Defaults.DateFormat, null
	) { }
}
