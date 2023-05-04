record TimesheetParams(
	string UserName, 
	int[] StartCell, 
	int TableBeginAfterRows, 
	int WorkHours, 
	string DescriptionPlaceholder, 
	string DateFormat, 
	string? WorksheetName
);
