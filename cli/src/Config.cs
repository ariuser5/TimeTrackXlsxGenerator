public record Config(
	DateMonth? Date = null,
	string? OutputLocation = null,
	string? UserName = null,
	(int row, int col)? StartCell = null,
	int? RowsSpace = null,
	int? WorkHours = null,
	string? DescriptionPlaceholder = null,
	string? DateFormat = null,
	string? WorksheetName = null
);
