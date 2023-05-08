public record TimesheetDocmuentOptions(
	DateMonth Date,
	string UserName,
	(int row, int col) StartCell,
	int RowsSpace,
	int WorkHours,
	string DescriptionPlaceholder,
	string DateFormat,
	string? WorksheetName
) {
	public static implicit operator TimesheetDocmuentOptions(
		Options options
	) => new TimesheetDocmuentOptions(
		options.Date,
		options.UserName,
		options.StartCell,
		options.RowsSpace,
		options.WorkHours,
		options.DescriptionPlaceholder,
		options.DateFormat,
		options.WorksheetName
	);
}