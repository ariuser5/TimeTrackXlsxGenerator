using CommandLine;
using Timesheet.CommandLine;

public static class OptionsReader
{
	public static Options? Read(params string[] args)
	{
		return Read(args, (err) => {});
	}
	
	public static Options? Read(string[] args, Action<OptionParseException> onError)
	{
		Options? instance = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
			.MapResult<CommandLineOptions, Options?>(
				parsedFunc: (options) => ReadFromCommandLineOptions(options, onError),
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
		bool mustCreateIfMissing = !options.AllowMissingConfig;
		Lazy<Config?> configFile = new(() => ConfigFile.Read(mustCreateIfMissing));
		Options result = new();
		try {
			result.AllowMissingConfig = options.AllowMissingConfig;
			OverrideOptionsWithDeferredAlternative(
				result.OverrideDateFrom(options, configFile),
				result.OverrideUserNameFrom(options, configFile),
				result.OverrideStartCellFrom(options, configFile),
				result.OverrideRowsSpaceFrom(options, configFile),
				result.OverrideWorkHoursFrom(options, configFile),
				result.OverrideDescriptionPlaceholderFrom(options, configFile),
				result.OverrideDateFormatFrom(options, configFile),
				result.OverrideWorksheetNameFrom(options, configFile)
			);
		} catch (OptionParseException parseException) {
			onError?.Invoke(parseException);
			return null;
		}
		return result;
	}
	
	private static void OverrideOptionsWithDeferredAlternative(params Action?[] alternatives)
	{
		foreach (Action? overrideAction in alternatives) {
			overrideAction?.Invoke();
		}
	}
	
	private static Action? OverrideDateFrom(this Options it, CommandLineOptions options, Lazy<Config?> config)
	{
		if (options.Date is not null) {
			bool isParsed = DateMonth.TryParse(options.Date, out DateMonth parsedDate);
			if (isParsed) {
				it.Date = parsedDate;
			} else {
				throw new OptionParseException($"Invalid format for date '{options.Date}'.");
			}
		} else if (config.Value?.Date is not null) {
			return () => it.Date = config.Value.Date.Value;
		}
		return null;
	}
	
	private static Action? OverrideUserNameFrom(this Options it, CommandLineOptions options, Lazy<Config?> config)
	{
		if (options.UserName is not null) {
			it.UserName = options.UserName;
		} else if (config.Value?.UserName is not null) {
			it.UserName = config.Value.UserName;
		}
		return null;
	}
	
	private static Action? OverrideStartCellFrom(this Options it, CommandLineOptions options, Lazy<Config?> config)
	{
		if (options.StartCell is not null) {
			string[]? parts = options.StartCell.Split(':');
			if (parts is null) throw OptionParseException.FromOptionProperty(options, nameof(CommandLineOptions.StartCell));
			if (parts.Length != 2 ||
				!int.TryParse(parts[0], out int row) ||
				!int.TryParse(parts[1], out int col))
			{
				throw OptionParseException.FromOptionProperty(options, nameof(CommandLineOptions.StartCell));
			}
			it.StartCell = (row, col);
		} else if (config.Value?.StartCell is not null) {
			return () => it.StartCell = config.Value.StartCell.Value;
		}
		return null;
	}
	
	private static Action? OverrideRowsSpaceFrom(this Options it, CommandLineOptions options, Lazy<Config?> config)
	{
		if (options.RowsSpace is not null) {
			it.RowsSpace = options.RowsSpace.Value;
		} else if (config.Value?.RowsSpace is not null) {
			it.RowsSpace = config.Value.RowsSpace.Value;
		}
		return null;
	}
	
	private static Action? OverrideWorkHoursFrom(this Options it, CommandLineOptions options, Lazy<Config?> config)
	{
		if (options.WorkHours is not null) {
			it.WorkHours = options.WorkHours.Value;
		} else if (config.Value?.WorkHours is not null) {
			it.WorkHours = config.Value.WorkHours.Value;
		}
		return null;
	}
	
	private static Action? OverrideDescriptionPlaceholderFrom(this Options it, CommandLineOptions options, Lazy<Config?> config)
	{
		if (options.DescriptionPlaceholder is not null) {
			it.DescriptionPlaceholder = options.DescriptionPlaceholder;
		} else if (config.Value?.DescriptionPlaceholder is not null) {
			it.DescriptionPlaceholder = config.Value.DescriptionPlaceholder;
		}
		return null;
	}
	
	private static Action? OverrideDateFormatFrom(this Options it, CommandLineOptions options, Lazy<Config?> config)
	{
		if (options.DateFormat is not null) {
			it.DateFormat = options.DateFormat;
		} else if (config.Value?.DateFormat is not null) {
			it.DateFormat = config.Value.DateFormat;
		}
		return null;
	}
	
	private static Action? OverrideWorksheetNameFrom(this Options it, CommandLineOptions options, Lazy<Config?> config)
	{
		if (options.WorksheetName is not null) {
			it.WorksheetName = options.WorksheetName;
		} else if (config.Value?.WorksheetName is not null) {
			it.WorksheetName = config.Value.WorksheetName;
		}
		return null;
	}
}