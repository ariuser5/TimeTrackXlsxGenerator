using System.Drawing;
using OfficeOpenXml;

public static class TimesheetDocument
{
	public const string TemplateFile = "template.xlsx";
	
	public static void Create(TimesheetDocmuentOptions options, string filePath)
	{
		bool isValidInput = ValidateInput(options);
		if (!isValidInput) 
		{
			Console.WriteLine("Error: could not create document.");
			return;
		}
		
		using ExcelPackage package = new ExcelPackage(new FileInfo(TemplateFile));
		ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
		
		if (options.WorksheetName is not null) 
		{
			// Change name of the worksheet
			worksheet.Name = options.WorksheetName;
		}
		
		DateTime targetMonth = new(options.Date.Year, options.Date.Month, 1);
		WriteDateCell(worksheet, targetMonth, options);
		WriteDaysEntries(worksheet, targetMonth, options, );
		
		package.SaveAs(new FileInfo(filePath));
		Console.WriteLine($"Time track file generated: {filePath}");
	}

	static bool ValidateInput(TimesheetDocmuentOptions options)
	{
		// Check if template file exists
		if (!File.Exists(TemplateFile))
		{
			Console.WriteLine($"Error: template file '{(TemplateFile)}' missing.");
			return false;
		}
		
		return true;
	}
	
	static void WriteDateCell(ExcelWorksheet worksheet, DateTime targetMonth, TimesheetDocmuentOptions options)
	{
		int daysInMonth = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);
		DateTime lastMonthDay = new(targetMonth.Year, targetMonth.Month, daysInMonth);
		int row = options.StartCell.row;
		int column = options.StartCell.col;
		worksheet.Cells[row, column].Value = $"01-{lastMonthDay.ToString(options.DateFormat)}";
	}

	static void WriteDaysEntries(
		ExcelWorksheet worksheet,
		DateTime targetMonth,
		TimesheetDocmuentOptions options,
		IHolidaysService? holidaysService = null)
	{
		int writtenDays = 0;
		int startRow = options.StartCell.row + options.RowsSpace + 1;
		for (
			DateTime date = targetMonth; 
			date.Month == targetMonth.Month; 
			date = date.AddDays(1)
		) {
			string? holidayName = holidaysService?.GetHolidayName(date);
			bool isHoliday = holidayName is not null;
			
			int currentRow = startRow + (writtenDays++);
			int currentColumn = options.StartCell.col;
			
			// Insert new row
			worksheet.InsertRow(currentRow, 1);
			
			// Write data to the row
			worksheet.Cells[currentRow, currentColumn++].Value = options.UserName;
			worksheet.Cells[currentRow, currentColumn++].Value = date.ToString(options.DateFormat);
			
			if (isHoliday) {
				// Color the row in yellow
				ExcelRange range = worksheet.Cells[currentRow, currentColumn - 2, currentRow, currentColumn + 2];
				range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
				range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
			}
			
			// Skip weekend days and holidays
			if (isHoliday ||
				date.DayOfWeek == DayOfWeek.Saturday ||
				date.DayOfWeek == DayOfWeek.Sunday
			) continue;
			
			worksheet.Cells[currentRow, currentColumn++].Value = options.WorkHours;
			worksheet.Cells[currentRow, currentColumn++].Value = options.DescriptionPlaceholder;
		}
		
		WriteTotalHours(worksheet, options, writtenDays);
	}
	
	private static void WriteTotalHours(ExcelWorksheet worksheet, TimesheetDocmuentOptions options, int writtenDays)
	{
		int referenceColumn = options.StartCell.col + 2;
		string columnName = ConvertToExcelColumnName(referenceColumn);
		
		int firstDayRow = options.StartCell.row + options.RowsSpace + 1;
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

class HolidaysService: IHolidaysService
{
	public string? GetHolidayName(DateTime date)
	{
		return "St. asd";
	}
}
