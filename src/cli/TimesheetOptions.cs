record TimesheetOptions(
	DateMonth ReferencedDate,
	string UserName,
	(int row, int col) StartCell,
	int RowsSpace,
	int WorkHours,
	string DescriptionPlaceholder,
	string DateFormat,
	string? WorksheetName
);
