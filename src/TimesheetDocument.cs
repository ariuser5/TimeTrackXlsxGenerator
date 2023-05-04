using OfficeOpenXml;

static class TimesheetDocument
{
	public const string TemplateFile = "template.xlsx";
	
	public static void Create(TimesheetParams args, DateTime referenceDate, string filePath)
	{
		bool isValidInput = ValidateInput(args);
		if (!isValidInput) 
		{
			Console.WriteLine("Error: could not create document.");
			return;
		}
		
		using ExcelPackage package = new ExcelPackage(new FileInfo(TemplateFile));
		ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
		
		if (args.WorksheetName is not null) 
		{
			// Change name of the worksheet
			worksheet.Name = args.WorksheetName;
		}
		
		DateTime targetMonth = new(referenceDate.Year, referenceDate.Month, 1);
		WriteDateCell(worksheet, targetMonth, args);
		WriteDaysEntries(worksheet, targetMonth, args);
		
		package.SaveAs(new FileInfo(filePath));
		Console.WriteLine($"Time track file generated: {filePath}");
	}

	static bool ValidateInput(TimesheetParams args)
	{
		// Check if start cell is valid
		if (args.StartCell.Length != 2)
		{
			Console.WriteLine("Error: start cell must be an array of two integers.");
			return false;
		}
		
		// Check if template file exists
		if (!File.Exists(TemplateFile))
		{
            Console.WriteLine($"Error: template file '{(TemplateFile)}' missing.");
			return false;
		}
		
		return true;
	}
	
	static void WriteDateCell(ExcelWorksheet worksheet, DateTime targetMonth, TimesheetParams args)
	{
		int daysInMonth = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);
		DateTime lastMonthDay = new(targetMonth.Year, targetMonth.Month, daysInMonth);
		int row = args.StartCell[0];
		int column = args.StartCell[1];
		worksheet.Cells[row, column].Value = $"01-{lastMonthDay.ToString(args.DateFormat)}";
	}

	static void WriteDaysEntries(ExcelWorksheet worksheet, DateTime targetMonth, TimesheetParams args)
	{
		int writtenDays = 0;
		int startRow = args.StartCell[0] + args.TableBeginAfterRows + 1;
		for (
			DateTime date = targetMonth; 
			date.Month == targetMonth.Month; 
			date = date.AddDays(1)
		) {
			// Skip weekend days
			if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
				continue;

			int currentRow = startRow + writtenDays;
			int currentColumn = args.StartCell[1];
			
			// Insert new row
			worksheet.InsertRow(currentRow, 1);
			
			// Write data to the row
			worksheet.Cells[currentRow, currentColumn++].Value = args.UserName;
			worksheet.Cells[currentRow, currentColumn++].Value = date.ToString(args.DateFormat);
			worksheet.Cells[currentRow, currentColumn++].Value = args.WorkHours;
			worksheet.Cells[currentRow, currentColumn++].Value = args.DescriptionPlaceholder;
			writtenDays++;
		}
		
		WriteTotalHours(worksheet, args, writtenDays);
	}
	
	private static void WriteTotalHours(ExcelWorksheet worksheet, TimesheetParams args, int writtenDays)
	{
		int referenceColumn = args.StartCell[1] + 2;
		string columnName = ConvertToExcelColumnName(referenceColumn);
		
		int firstDayRow = args.StartCell[0] + args.TableBeginAfterRows + 1;
		int lastDayRow = firstDayRow + writtenDays - 1;
		int writeRow = lastDayRow + 2;
		
		if (writtenDays > 0) {
			string excelFormula = $"=SUM({columnName}{firstDayRow}:{columnName}{lastDayRow})";
			worksheet.Cells[writeRow, referenceColumn].Formula = excelFormula;
		} else {
			worksheet.Cells[writeRow, referenceColumn].Value = 0;
		}
	}
	
	static string ConvertToExcelColumnName(int columnNumber)
	{
		string columnName = "";

		while (columnNumber > 0)
		{
			int modulo = (columnNumber - 1) % 26;
			columnName = Convert.ToChar('A' + modulo) + columnName;
			columnNumber = (columnNumber - modulo) / 26;
		} 

		return columnName;
	}
}