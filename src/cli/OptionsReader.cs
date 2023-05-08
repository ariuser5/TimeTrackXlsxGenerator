using CommandLine;
using Timesheet.CommandLine;

static class OptionsReader
{
	public static Options? Read(params string[] args)
	{
		return Read(args, (err) => {});
	}
	
	public static Options? Read(string[] args, Action<OptionParseException> onError)
	{
		if (args is null) {
			return ConfigFile.Read(mustCreateIfMissing: true);
		}
		
		Options? instance = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
			.MapResult<CommandLineOptions, Options?>(
				parsedFunc: (o) => ReadFromCommandLineOptions(o, onError),
				notParsedFunc: (errors) => {
					foreach (Error err in errors) {
						OptionParseException? parseException = OptionParseException.From(err);
						if (parseException is null) continue;
						onError(parseException);
					}
					return null;
				}
			);
		
		return instance;
	}
	
	public static Options? ReadFromCommandLineOptions(
		CommandLineOptions options,
		Action<OptionParseException>? onError = null)
	{
		Options instance = new();
		bool mustCreateIfMissing = !options.AllowMissingConfig;
		Lazy<Options> configFile = new(() => ConfigFile.Read(mustCreateIfMissing));
		
		try {
			instance.ReferencedDate = ReadReferencedDate(options, configFile);
			instance.OutputLocation = options.OutputLocation ?? configFile.Value.OutputLocation;
			instance.UserName = options.UserName ?? configFile.Value.UserName;
			instance.StartCell = ReadStartCell(options, configFile);
			instance.RowsSpace = options.RowsSpace ?? configFile.Value.RowsSpace;
			instance.WorkHours = options.WorkHours ?? configFile.Value.WorkHours;
			instance.DescriptionPlaceholder = options.DescriptionPlaceholder ?? configFile.Value.DescriptionPlaceholder;
			instance.DateFormat = options.DateFormat ?? configFile.Value.DateFormat;
			
			if (options.WorksheetName is null) {
				Console.WriteLine("WorksheetName is " + "NULL");
			} else if (options.WorksheetName == string.Empty) {
				Console.WriteLine("WorksheetName is " + "EMPTY");
			}
			
			instance.WorksheetName = options.WorksheetName ?? configFile.Value.WorksheetName;	
		} catch (OptionParseException parseException) {
			onError?.Invoke(parseException);
			return null;
		}
		
		return instance;
	}
	
	private static DateMonth ReadReferencedDate(CommandLineOptions options, Lazy<Options> configFile)
	{
		if (options.ReferencedDate is not null) {
			bool isParsed = DateMonth.TryParse(options.ReferencedDate, out DateMonth parsedDate);
			if (!isParsed) {
				return parsedDate;
			} else {
				throw OptionParseException.FromOptionProperty(options, nameof(CommandLineOptions.ReferencedDate));
			}
		}
		return configFile.Value.ReferencedDate;
	}
	
	private static (int row, int col) ReadStartCell(CommandLineOptions options, Lazy<Options> configFile)
	{
		string[]? parts = options.StartCell?.Split(':');
		if (parts is null) return configFile.Value.StartCell;
		if (parts.Length != 2 ||
			!int.TryParse(parts[0], out int row) ||
			!int.TryParse(parts[1], out int col))
		{
			throw OptionParseException.FromOptionProperty(options, nameof(CommandLineOptions.StartCell));
		}
		return (row, col);
	}
}