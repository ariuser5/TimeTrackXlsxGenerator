using System.Reflection;
using CommandLine;

namespace Timesheet.CommandLine;

class CommandLineOptions
{
	// private string? _worksheetName;
    

    // [Value()]
    // public string? Some { get; set; }

	[Option('d', "date", 
        Required = false,
        HelpText = $"Month for which to generate timesheet. Allowed formats:\n{DateMonth.Formats}.")]
	public string? ReferencedDate { get; set; }
	
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
    //     get => _worksheetName; 
    //     set {
    //         Console.WriteLine($"Setting worksheet name to '{value}'");
    //         _worksheetName = value?.Trim();
    //     }
    // }

	[Option("allow-missing-config",
        Required = false,
        Default = false,
        HelpText = "Do not generate config file if missing.")]
	public bool AllowMissingConfig { get; set; }
    
    
    public CommandLineOptions()
	{
		// ReferencedDate = new CommandLineOption<string?>(GetOptionsAttribute(nameof(ReferencedDate)));
        // OutputLocation = new CommandLineOption<string?>(GetOptionsAttribute(nameof(OutputLocation)));
        // UserName = new CommandLineOption<string?>(GetOptionsAttribute(nameof(UserName)));
        // StartCell = new CommandLineOption<string?>(GetOptionsAttribute(nameof(StartCell)));
        // RowsSpace = new CommandLineOption<int?>(GetOptionsAttribute(nameof(RowsSpace)));
        // WorkHours = new CommandLineOption<int?>(GetOptionsAttribute(nameof(WorkHours)));
        // DescriptionPlaceholder = new CommandLineOption<string?>(GetOptionsAttribute(nameof(DescriptionPlaceholder)));
        // DateFormat = new CommandLineOption<string?>(GetOptionsAttribute(nameof(DateFormat)));
        // WorksheetName = new CommandLineOption<string?>(GetOptionsAttribute(nameof(WorksheetName)));
        // AllowMissingConfig = new CommandLineOption<bool>(GetOptionsAttribute(nameof(AllowMissingConfig)));
	}
    
    private OptionAttribute GetOptionsAttribute(string propertyName)
    {
        OptionAttribute? attribute = this.GetType().GetProperty(propertyName)?.GetCustomAttribute<OptionAttribute>(true);
        
        if (attribute is null)
            throw new ArgumentException($"Property '{propertyName}' is not an option.");
        
        return attribute;
    }
}