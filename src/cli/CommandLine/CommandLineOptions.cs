using CommandLine;

namespace Timesheet.CommandLine;

public record CommandLineOptions
{
	[Value(0, 
        Required = false,
        HelpText = $"Month for which to generate timesheet. Allowed formats:\n{DateMonth.InputFormats}.")]
	public string? Date { get; set; }
	
	[Option('o', "output-location",
        Required = false,
        HelpText = "Output location.")]
    public string? OutputLocation { get; set; }

    [Option('n', "name",
        Required = false,
        HelpText = "User name.")]
    public string? UserName { get; set; }

    [Option('c', "start-cell",
        Required = false,
        HelpText = "Start cell position.\n -c ROW:COL\n --start-cell ROW:COL")]
    public string? StartCell { get; set; }
    
    [Option('s', "space",
        Required = false,
        HelpText = "No. of rows between start cell and table begin")]
    public int? RowsSpace { get; set; }

    [Option('h', "workhours",
        Required = false,
        HelpText = "Work hours per day.")]
    public int? WorkHours { get; set; }

    [Option('p', "description-placeholder",
        Required = false,
        HelpText = "Description placeholder.")]
    public string? DescriptionPlaceholder { get; set; }

    [Option('f', "date-format",
        Required = false,
        HelpText = "Date format.")]
    public string? DateFormat { get; set; }

	[Option('w', "worksheet",
        Required = false,
        HelpText = "Worksheet name.")]
	public string? WorksheetName { get; set; }
    
	[Option("allow-missing-config",
        Required = false,
        Default = false,
        HelpText = "Do not generate config file if missing.")]
	public bool AllowMissingConfig { get; set; }
}