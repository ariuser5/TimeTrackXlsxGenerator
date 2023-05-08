public partial class Options
{
	public DateMonth Date { get; set; }
	public string OutputLocation { get; set; }
	public string UserName { get; set; }
	public (int row, int col) StartCell { get; set; }
	public int RowsSpace { get; set; }
	public int WorkHours { get; set; }
	public string DescriptionPlaceholder { get; set; }
	public string DateFormat { get; set; }
	public string? WorksheetName { get; set; }
	public bool AllowMissingConfig { get; set; }
	
	public Options() {
		this.Date = DateMonth.Now();
		this.OutputLocation = Defaults.OutputLocation;
		this.UserName = Defaults.UserName;
		this.StartCell = Defaults.StartCell;
		this.RowsSpace = Defaults.RowsSpace;
		this.WorkHours = Defaults.WorkHours;
		this.DescriptionPlaceholder = Defaults.DescriptionPlaceholder;
		this.DateFormat = Defaults.DateFormat;
		this.WorksheetName = null;
		this.AllowMissingConfig = Defaults.AllowMissingConfig;
	}
}